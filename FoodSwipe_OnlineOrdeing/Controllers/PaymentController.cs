using FoodOrderingSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace FoodOrderingSystem.Controllers
{

    public class PaymentController : Controller
    {
        private readonly AppDbContext _context;
        public PaymentController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Payment(int? id)
        {
            HttpContext.Session.SetInt32("tp", (int)id);

            return View();
        }
        [HttpPost]
        public IActionResult Payment_Online(Payment payment)
        {
            ViewBag.TPOnline = HttpContext.Session.GetInt32("tp");
            if (ModelState.IsValid)
            {
                return View();

            }
            return View("~/Views/Payment/Payment.cshtml");

        }
        [HttpPost]
        public IActionResult Payment_Offline(Payment payment)
        {
            ViewBag.TPOffline = HttpContext.Session.GetInt32("tp");
            if (ModelState.IsValid)
            {
                return View();
            }
            return View("~/Views/Payment/Payment.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> Success_OnlineAsync(Payment_Online paymentonline)
        {
            ViewBag.TPOnline = HttpContext.Session.GetInt32("tp");
            if (ModelState.IsValid)
            {
                List<OrderList> orderlist = await _context.OrderList.Where(b => b.Email == User.Identity.Name).ToListAsync();

                try
                {
                    //Create the msg object to be sent
                    MailMessage msg = new MailMessage();
                    var sb = new StringBuilder();
                    int billAmount = 0;

                    sb.Append("<table class=\"table\">   <thead style=\\\"border: 0; border-bottom: 1px dashed #ccc;\\> <tr><th>Item  </th><th>Quantity</th><th>Total Price</th><th></th></tr></thead><tbody style=\\\"border: 0; border-bottom: 1px dashed #ccc;\\>");
                    if (orderlist.Count > 0)
                    {
                        foreach (OrderList order in orderlist)
                        {
                            sb.AppendLine("<tr>");
                            sb.AppendLine("<td style=\"text-align:center;\" width=\"200\">" + order.Item + "</td>");
                            sb.AppendLine("<td style=\"text-align:center;\" width=\"200\">" + order.Quantity + "</td>");
                            sb.AppendLine("<td style=\"text-align:center;\" width=\"200\">" + order.TotalPrice + "</td>");
                            sb.AppendLine("</tr>");
                            billAmount += int.Parse(order.TotalPrice);
                            _context.OrderList.Remove(order);
                        }
                        await _context.SaveChangesAsync();
                    }
                    string cardNumber = paymentonline.CardNumber.ToString();
                    sb.AppendLine("<h3 style=\"font-size:20px\"> Bill Amount: " + billAmount + " </h3>");
                    sb.AppendLine("<h4>Payment is done through card ends with: " + cardNumber.Substring(12) + "</h4>");
                    //Add your email address to the recipients
                    msg.To.Add(User.Identity.Name.ToString());
                    //Configure the address we are sending the mail from
                    MailAddress address = new MailAddressmanupatel20022005");
                    msg.From = address;
                    msg.Subject = "Order Successfully Placed!!";
                    msg.Body = sb.ToString();
                    msg.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = true;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new System.Net.NetworkCredentialmanupatel20022005", "Manu@2004");
                    smtp.Send(msg);
                    Console.WriteLine("Mail Sent");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    //"Your message failed to send, please try again."
                }
                return View();
            }
            return View("~/Views/Payment/Payment_Online.cshtml");

        }

        public async Task<IActionResult> Success_Offline()
        {
            ViewBag.TPOnline = HttpContext.Session.GetInt32("tp");
            List<OrderList> orderlist = await _context.OrderList.Where(b => b.Email == User.Identity.Name).ToListAsync();

            try
            {
                //Create the msg object to be sent
                MailMessage msg = new MailMessage();
                var sb = new StringBuilder();
                int billAmount = 0;

                sb.Append("<table class=\"table\">   <thead style=\\\"border: 0; border-bottom: 1px dashed #ccc;\\> <tr><th>Item  </th><th>Quantity</th><th>Total Price</th><th></th></tr></thead><tbody style=\\\"border: 0; border-bottom: 1px dashed #ccc;\\>");
                if (orderlist.Count > 0)
                {
                    foreach (OrderList order in orderlist)
                    {
                        sb.AppendLine("<tr>");
                        sb.AppendLine("<td style=\"text-align:center;\" width=\"200\">" + order.Item + "</td>");
                        sb.AppendLine("<td style=\"text-align:center;\" width=\"200\">" + order.Quantity + "</td>");
                        sb.AppendLine("<td style=\"text-align:center;\" width=\"200\">" + order.TotalPrice + "</td>");
                        sb.AppendLine("</tr>");
                        billAmount += int.Parse(order.TotalPrice);
                        _context.OrderList.Remove(order);
                    }
                    await _context.SaveChangesAsync();
                }
                sb.AppendLine("<h3 style=\"font-size:20px\"> Bill Amount: " + billAmount + " </h3>");
                //Add your email address to the recipients
                msg.To.Add(User.Identity.Name.ToString());
                //Configure the address we are sending the mail from
                MailAddress address = new MailAddressmanupatel20022005");
                msg.From = address;
                msg.Subject = "Order Successfully Placed!!";
                msg.Body = sb.ToString();
                msg.IsBodyHtml = true;

                //Configure an SmtpClient to send the mail.
                //System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
                //client.DeliveryMethod = SmtpDeliveryMethod.Network;
                //client.EnableSsl = false;
                //client.Host = "relay-hosting.secureserver.net";
                //client.Port = 25;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                smtp.Port = 587;
                smtp.UseDefaultCredentials = true;
                smtp.EnableSsl = true;
                smtp.Credentials = new System.Net.NetworkCredential("manupatel20052002@gmail.com", "Manu@2004");
                smtp.Send(msg);




                //Display some feedback to the user to let them know it was sent
                Console.WriteLine("Mail Sent");



            }
            catch (Exception ex)
            {
                //If the message failed at some point, let the user know
                Console.WriteLine(ex.ToString());
                //"Your message failed to send, please try again."
            }

            return View();
        }
    }
}
