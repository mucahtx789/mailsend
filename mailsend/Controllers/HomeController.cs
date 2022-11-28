using mailsend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;

namespace mailsend.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(email model)
        {
            //gmail güvenlik ayarları --> 2 adımda doğrulama açıp ardından uygulama şifresi oluşturup bu random şifre oturum açmada kullanılır.


            MailMessage mailtobesend = new MailMessage();
            //To.Add birden fazla gönderim şeklinde
            mailtobesend.To.Add ("gönderilecek mail hesabı @gmail.com");
            mailtobesend.From=new MailAddress("alıcı mail hesabı@gmail.com");
            mailtobesend.Subject = model.title;
            mailtobesend.Body =model.fullname+"<br>"+ model.content;
            mailtobesend.IsBodyHtml = true;
            
            //gönderimi yapıcak gmail hesabı ,uygulama şifresi 
            //modern port secure smpt 587 (yaygın kullanılır), 25 standart port,465 out-of date(deprecated) ,2525 alternative non-standart
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential("gönderimi yapıcak mail hesabı@gmail.com", "uygulama şifresi");
            smtp.Port = 587;
            //gmail kullanılıyor
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl= true;

            

            try
            {
                smtp.Send(mailtobesend);
                TempData["Message"] = "mail gönderimi başarılı:";
            }
            catch (Exception ex)
            {

                TempData["Message"] = "mail gönderimi başarısız hata mesajı:" + ex.Message;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}