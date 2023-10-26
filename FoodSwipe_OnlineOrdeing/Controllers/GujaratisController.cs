using FoodOrderingSystem.Models;
using FoodOrderingSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Controllers
{
    public class GujaratisController : Controller
    {
        private readonly AppDbContext _context;

        public GujaratisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Gujaratis
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gujarati.ToListAsync());
        }

        // GET: Gujaratis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gujarati = await _context.Gujarati
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gujarati == null)
            {
                return NotFound();
            }

            return View(gujarati);
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

        // GET: Gujaratis/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gujaratis/Create
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
                Gujarati gujarati = new Gujarati
                {
                    Item = model.Item,
                    Price = model.Price,
                    PhotoPath = uniqueFileName
                };
                _context.Add(gujarati);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Gujaratis/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gujarati = await _context.Gujarati.FindAsync(id);
            if (gujarati == null)
            {
                return NotFound();
            }
            GeneralViewModel generalViewModel = new GeneralViewModel
            {
                Id = gujarati.Id,
                Item = gujarati.Item,
                Price = gujarati.Price,
                ExistingPhotoPath = gujarati.PhotoPath
            };
            return View(generalViewModel);
        }

        // POST: Gujaratis/Edit/5
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
                    var gujarati = await _context.Gujarati.FindAsync(id);
                    gujarati.Item = model.Item;
                    gujarati.Price = model.Price;
                    if (model.Photo != null)
                    {
                        gujarati.PhotoPath = ProcessUploadedFile(model);

                        // If a new photo is uploaded, the existing photo must be
                        // deleted. So check if there is an existing photo and delete
                        if (model.ExistingPhotoPath != null)
                        {
                            string filePath = "wwwroot/images/" + model.ExistingPhotoPath;
                            System.IO.File.Delete(filePath);
                        }
                    }
                    _context.Update(gujarati);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GujaratiExists(model.Id))
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

        // GET: Gujaratis/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gujarati = await _context.Gujarati
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gujarati == null)
            {
                return NotFound();
            }

            return View(gujarati);
        }

        // POST: Gujaratis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gujarati = await _context.Gujarati.FindAsync(id);
            if (gujarati.PhotoPath != null)
            {
                string filePath = "wwwroot/images/" + gujarati.PhotoPath;
                System.IO.File.Delete(filePath);
            }
            _context.Gujarati.Remove(gujarati);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GujaratiExists(int id)
        {
            return _context.Gujarati.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddCart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gujarati = await _context.Gujarati
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gujarati == null)
            {
                return NotFound();
            }

            HttpContext.Session.SetString("Item", gujarati.Item);
            ViewBag.var1 = HttpContext.Session.GetString("Item");
            HttpContext.Session.SetString("Price", gujarati.Price);
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

            return RedirectToRoute(new { controller = "Gujaratis", action = "Index" });
        }
    }
}
