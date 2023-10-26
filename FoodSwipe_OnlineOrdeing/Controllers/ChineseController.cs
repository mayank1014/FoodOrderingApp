﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodOrderingSystem.Models;
using Microsoft.AspNetCore.Http;
using FoodOrderingSystem.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FoodOrderingSystem.Controllers
{
    public class ChineseController : Controller
    {
        private readonly AppDbContext _context;

        public ChineseController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Chinese
        public async Task<IActionResult> Index()
        {
            return View(await _context.Chinese.ToListAsync());
        }

        // GET: Chinese/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chinese = await _context.Chinese
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chinese == null)
            {
                return NotFound();
            }

            return View(chinese);
        }
        private string ProcessUploadedFile(GeneralViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                // The image must be uploaded to the images folder in wwwroot
                // To get the path of the wwwroot folder we are directly providing path 
                string uploadsFolder = "wwwroot/images";

                // To make sure the file name is unique we are appending a new
                // GUID value and and an underscore to the file name
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        // GET: Chinese/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Chinese/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(GeneralViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (model.Photo != null)
                {
                    uniqueFileName = ProcessUploadedFile(model);
                }
                Chinese chinese = new Chinese
                {
                    Item = model.Item,
                    Price = model.Price,
                    PhotoPath = uniqueFileName
                };
                _context.Add(chinese);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Chinese/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chinese = await _context.Chinese.FindAsync(id);
            if (chinese == null)
            {
                return NotFound();
            }
            GeneralViewModel generalViewModel = new GeneralViewModel
            {
                Id = chinese.Id,
                Item = chinese.Item,
                Price = chinese.Price,
                ExistingPhotoPath = chinese.PhotoPath
            };
            return View(generalViewModel);
        }

        // POST: Chinese/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, GeneralViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var chinese = await _context.Chinese.FindAsync(id);
                    chinese.Item = model.Item;
                    chinese.Price = model.Price;
                    if (model.Photo != null)
                    {
                        chinese.PhotoPath = ProcessUploadedFile(model);

                        // If a new photo is uploaded, the existing photo must be
                        // deleted. So check if there is an existing photo and delete
                        if (model.ExistingPhotoPath != null)
                        {
                            string filePath = "wwwroot/images/" + model.ExistingPhotoPath;
                            System.IO.File.Delete(filePath);
                        }
                    }
                    _context.Update(chinese);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChineseExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Chinese/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chinese = await _context.Chinese
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chinese == null)
            {
                return NotFound();
            }

            return View(chinese);
        }

        // POST: Chinese/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chinese = await _context.Chinese.FindAsync(id);
            if (chinese.PhotoPath != null)
            {
                string filePath = "wwwroot/images/" + chinese.PhotoPath;
                System.IO.File.Delete(filePath);
            }
            _context.Chinese.Remove(chinese);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChineseExists(int id)
        {
            return _context.Chinese.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chinese = await _context.Chinese
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chinese == null)
            {
                return NotFound();
            }

            HttpContext.Session.SetString("Item", chinese.Item);
            ViewBag.var1 = HttpContext.Session.GetString("Item");
            HttpContext.Session.SetString("Price", chinese.Price);
            ViewBag.var2 = HttpContext.Session.GetString("Price");
            return View();
        }

        [HttpPost, ActionName("AddCart")]
        [ValidateAntiForgeryToken]
        public IActionResult AddCart()
        {
            OrderList ol = new OrderList();
            ol.Item = HttpContext.Request.Form["Item"].ToString();
            ol.Email = User.Identity.Name;
            ol.Quantity = Convert.ToInt32(HttpContext.Request.Form["Quantity"]);
            ol.TotalPrice = (Convert.ToInt32(HttpContext.Request.Form["TotalPrice"]) * ol.Quantity).ToString();

            _context.OrderList.Add(ol);
            _context.SaveChanges();

            return RedirectToRoute(new { controller = "Chinese", action = "Index" });
        }
    }
}
