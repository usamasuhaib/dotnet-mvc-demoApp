using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using MVC.DemoProject.Controllers;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using MVC.DemoProject.Models;

namespace MVC.DemoProject.Controllers
{
    public class EmailController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public EmailController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public void OnGet()
        {

        }

        [HttpPost]
        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.GetUserAsync(User);


            var name = Request.Form["name"];
            var userEmail = user.Email;
            var subject = Request.Form["subject"];
            var message = Request.Form["message"];

            try
            {
                SendEmail(name, userEmail, subject, message);
                TempData["EmailStatus"] = "sent"; // Set TempData indicating successful sending
            }
            catch (Exception ex)
            {
                // Handle exception
                TempData["EmailStatus"] = "failed"; // Set TempData indicating failure
            }
            return RedirectToAction("Index", "Home");
        }
        public bool SendEmail(string name, string userEmail,string subject, string message)
        {
            MailMessage message1 = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();

            message1.From = new MailAddress(userEmail);

            message1.To.Add("usamabalti3377@gmail.com");
            message1.Subject = subject;
            message1.IsBodyHtml = true;
            //message1.Body = "<p> Name:"+ name+ "</p>" + "<p> Email:" + email + "</p>" + "<p> Message:" + message + "</p>";
            message1.Body = message;



            smtpClient.Port = 587;
            smtpClient.Host = "smtp.gmail.com";
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential("usamabalti3377@gmail.com", "soynxzibcnylhoyl");
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Send(message1);

            return true;


        }
    }
}
