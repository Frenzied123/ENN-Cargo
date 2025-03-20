using ENN_Cargo.Core;
using Microsoft.AspNetCore.Mvc;

namespace ENN_Cargo.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly EmailService _emailService;

        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendContactEmail(string name, string email, string subject, string message)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(message))
            {
                return Json(new { success = false, message = "Please fill in all required fields." });
            }
                var emailBody = $"<h3>New Contact Form Submission</h3>" +
                                $"<p><strong>Name:</strong> {name}</p>" +
                                $"<p><strong>Email:</strong> {email}</p>" +
                                $"<p><strong>Subject:</strong> {subject}</p>" +
                                $"<p><strong>Message:</strong> {message}</p>";

                await _emailService.SendEmailAsync("enncargo@gmail.com", subject, emailBody);
                return Json(new { success = true, message = "Your message has been sent successfully!" });
        }
    }
}