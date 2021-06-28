using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Mail;
using System.Threading.Tasks;
using WebPortal.Models;
using WebPortal.Models.ViewModel;

namespace WebPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationIdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationIdentityUser> _signInManager;
        private readonly DataContext context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationIdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationIdentityUser> signInManager, DataContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            this.context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated) return RedirectToAction("LandingPage");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                if (loginVM.Username.Contains("@"))
                {
                    var checkemail = await _userManager.FindByEmailAsync(loginVM.Username);
                    if(checkemail != null)
                    {
                        loginVM.Username = checkemail.UserName;
                    }
                }
                var result = await _signInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password
                     , false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("LandingPage");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpVM signUPVM)
        {
            var checkemail = await _userManager.FindByEmailAsync(signUPVM.Email);
            if (checkemail != null)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationIdentityUser
                {
                    FirstName = signUPVM.FirstName,
                    LastName = signUPVM.LastName,
                    UserName = signUPVM.Username,
                    Email = signUPVM.Email,
                };
                var result = await _userManager.CreateAsync(user, signUPVM.Password);
                if (result.Succeeded)
                {
                    var resultLogin = await _signInManager.PasswordSignInAsync(signUPVM.Username, signUPVM.Password
                     , false, false);
                    if (resultLogin.Succeeded)
                    {
                        return RedirectToAction("LandingPage");
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult LandingPage()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetReservations()
        {
            var reservations = context.Reservations.ToList();

            return Ok(reservations);
        }
        public IActionResult SchduleTrip()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SchduleTrip(ReservationsVM reservations)
        {
            reservations.CreatedOn = DateTime.Now;
            if (reservations.WillCall)
            {
                var date = DateTime.Now;
                var resDate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 0);
                reservations.ReturnTime = resDate;
            }
            string APIKEY = ""; //ENTER GOOGLE API KEY HERE
            if(APIKEY != "")
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/");
                client.DefaultRequestHeaders.Accept.Clear();
                HttpResponseMessage response = await client.GetAsync("geocode/json?address=" + reservations.PickupCity.Replace(" ", "+") + "&key=" + APIKEY);
                if (response.IsSuccessStatusCode)
                {
                    var res = response.Content.ReadAsStringAsync().Result;
                    var djson = JsonConvert.DeserializeObject<Root>(res);
                    var latFrom = djson.results[0].geometry.viewport.northeast.lat;
                    var lngFrom = djson.results[0].geometry.viewport.northeast.lng;
                    HttpResponseMessage response1 = await client.GetAsync("geocode/json?address=" + reservations.DestinationCity.Replace(" ", "+") + "&key=" + APIKEY);
                    if (response1.IsSuccessStatusCode)
                    {
                        var res1 = response1.Content.ReadAsStringAsync().Result;
                        var djson1 = JsonConvert.DeserializeObject<Root>(res1);
                        var latTo = djson1.results[0].geometry.viewport.northeast.lat;
                        var lngTo = djson1.results[0].geometry.viewport.northeast.lng;
                        var R = 6371; // Radius of the earth in km
                        var dLat = (latFrom - latTo) * (Math.PI / 180);  // deg2rad below
                        var dLon = (lngFrom - lngTo) * (Math.PI / 180);
                        var a =
                          Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                          Math.Cos(latFrom * (Math.PI / 180)) * Math.Cos(latTo * (Math.PI / 180)) *
                          Math.Sin(dLon / 2) * Math.Sin(dLon / 2)
                          ;
                        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                        var d = R * c; // Distance in km
                        reservations.Distance = d.ToString();
                    }
                }
            }
            else
            {
                reservations.Distance = "0";
            }
            Reservations resdata = new Reservations
            {
                ReservationUI = reservations.ReservationUI,
                Department = reservations.Department,
                TransportDate = reservations.TransportDate,
                CostCenterNumber = reservations.CostCenterNumber,
                MRN = reservations.MRN,
                PatientFirstName = reservations.PatientFirstName,
                PatientLastName = reservations.PatientLastName,
                PickupAddress = reservations.PickupAddress,
                PickupCity = reservations.PickupCity,
                ContactPhone = reservations.ContactPhone,
                DestinationAddress = reservations.DestinationAddress,
                DestinationCity = reservations.DestinationCity,
                OfficePhone = reservations.OfficePhone,
                TransportType = reservations.TransportType,
                PickupTime = reservations.PickupTime,
                AppointmentTime = reservations.AppointmentTime,
                ReturnTime = reservations.ReturnTime,
                Comments = reservations.Comments,
                RequestedBy = reservations.RequestedBy,
                CallBackNumber = reservations.CallBackNumber,
                CreatedOn = reservations.CreatedOn,
                Mileage = reservations.Distance
            };
            context.Reservations.Add(resdata);
            context.SaveChanges();
            return RedirectToAction("LandingPage");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError("", "Invalid Password Reset Token");
            }
            var model = new ResetPasswordVM
            {
                Email = email,
                Token = token
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        [Route("forgotpassword")]
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(ForgetPasswordVM model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                try
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //var data = JsonConvert.DeserializeObject<string>(token);
                    var passwordResetLink = Url.Action("ResetPassword","Home", new { email = model.Email, token = token }, Request.Scheme);
                    MailMessage message = new MailMessage();
                    message.From = new MailAddress("cthings01@gmail.com");
                    message.To.Add(new MailAddress(model.Email));
                    message.Subject = "Password Reset Link";
                    var emailTemplate = "<!DOCTYPE html><html><head> <title></title> <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/> <meta name='viewport' content='width=device-width, initial-scale=1'> <meta http-equiv='X-UA-Compatible' content='IE=edge'/> <style type='text/css'> @media screen{@font-face{font-family: 'Lato'; font-style: normal; font-weight: 400; src: local('Lato Regular'), local('Lato-Regular'), url(https://fonts.gstatic.com/s/lato/v11/qIIYRU-oROkIk8vfvxw6QvesZW2xOQ-xsNqO47m55DA.woff) format('woff');}@font-face{font-family: 'Lato'; font-style: normal; font-weight: 700; src: local('Lato Bold'), local('Lato-Bold'), url(https://fonts.gstatic.com/s/lato/v11/qdgUG4U09HnJwhYI-uK18wLUuEpTyoUstqEm5AMlJo4.woff) format('woff');}@font-face{font-family: 'Lato'; font-style: italic; font-weight: 400; src: local('Lato Italic'), local('Lato-Italic'), url(https://fonts.gstatic.com/s/lato/v11/RYyZNoeFgb0l7W3Vu1aSWOvvDin1pK8aKteLpeZ5c0A.woff) format('woff');}@font-face{font-family: 'Lato'; font-style: italic; font-weight: 700; src: local('Lato Bold Italic'), local('Lato-BoldItalic'), url(https://fonts.gstatic.com/s/lato/v11/HkF_qI1x_noxlxhrhMQYELO3LdcAZYWl9Si6vvxL-qU.woff) format('woff');}}/* CLIENT-SPECIFIC STYLES */ body, table, td, a{-webkit-text-size-adjust: 100%; -ms-text-size-adjust: 100%;}table, td{mso-table-lspace: 0pt; mso-table-rspace: 0pt;}img{-ms-interpolation-mode: bicubic;}/* RESET STYLES */ img{border: 0; height: auto; line-height: 100%; outline: none; text-decoration: none;}table{border-collapse: collapse !important;}body{height: 100% !important; margin: 0 !important; padding: 0 !important; width: 100% !important;}/* iOS BLUE LINKS */ a[x-apple-data-detectors]{color: inherit !important; text-decoration: none !important; font-size: inherit !important; font-family: inherit !important; font-weight: inherit !important; line-height: inherit !important;}/* MOBILE STYLES */ @media screen and (max-width:600px){h1{font-size: 32px !important; line-height: 32px !important;}}/* ANDROID CENTER FIX */ div[style*='margin: 16px 0;']{margin: 0 !important;}</style></head><body style='background-color: #F4F4F4; margin: 0 !important; padding: 0 !important;'><table border='0' cellpadding='0' cellspacing='0' width='100%'> <tr> <td bgcolor='#5B3AD3' align='center'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td align='center' valign='top' style='padding: 40px 10px 40px 10px;'> </td></tr></table> </td></tr><tr> <td bgcolor='#5B3AD3' align='center' style='padding: 0px 10px 0px 10px;'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td bgcolor='#FFFFFF' align='center' valign='top' style='padding: 40px 20px 20px 20px; border-radius: 4px 4px 0px 0px; color: #111111; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 48px; font-weight: 400; letter-spacing: 4px; line-height: 48px;'> <h1 style='font-size: 48px; font-weight: 400; margin: 2rem;'>Welcome!</h1>  </td></tr></table> </td></tr><tr> <td bgcolor='#F4F4F4' align='center' style='padding: 0px 10px 0px 10px;'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td bgcolor='#FFFFFF' align='left' style='padding: 20px 30px 30px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'> <p style='margin: 1rem;font-weight: bold;color: #462BC8;'><span>Dear </span><span>User</span>,</p><p style='margin: 1rem'>Welcome back to Webportal.</p><p style='margin-left: 1rem;margin-right: 1rem;'>To reset your password, you must click on the button.</p><p style='margin: 1rem; font-weight: bold;color:#FFA73B;font-size: 14px; '>Simply click on reset My Password button or the link below to reset your password.</p></td></tr><tr> <td bgcolor='#FFFFFF' align='left'> <table width='100%' border='0' cellspacing='0' cellpadding='0'> <tr> <td bgcolor='#FFFFFF' align='center' style='padding: 20px 30px 60px 30px;'> <table border='0' cellspacing='0' cellpadding='0'> <tr> <td align='center' style='border-radius: 3px;' bgcolor='#462BC8'><a href='" + passwordResetLink + "' target='_blank' style='font-size: 15px; font-family: Helvetica, Arial, sans-serif; color: #FFFFFF; text-decoration: none; color: #FFFFFF; text-decoration: none; padding: 15px 25px; border-radius: 2px; border: 1px solid #5B3AD3;border-radius: 10px; display: inline-block;'>Reset My Password</a></td></tr></table> </td></tr></table> </td></tr><tr> <td bgcolor='#FFFFFF' align='left' style='padding: 0px 30px 0px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'> <p style='margin-left:1rem;margin-right:1rem;'>If the above does not work, copy and paste the following URL into a browser window:</p></td></tr><tr> <td bgcolor='#FFFFFF' align='left' style='padding: 20px 30px 20px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'> <p style='margin-left:1rem;margin-right:1rem;'>Your verification link:</p><p style='margin-left:1rem;margin-right:1rem;'><a href='" + passwordResetLink + "' target='_blank' style='color: #462BC8;'>" + passwordResetLink + "</a></p></td></tr><tr> <td bgcolor='#FFFFFF' align='left' style='padding: 0px 30px 40px 30px; border-radius: 0px 0px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'> <p style='margin-left:1rem;margin-right:1rem;'>Have A Great Day!<br>Webportal</p></td></tr></table> </td></tr><tr> <td bgcolor='#F4F4F4' align='center' style='padding: 30px 10px 0px 10px;'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td bgcolor='#E0E0E0' align='center' style='padding: 30px 30px 30px 30px; border-radius: 4px 4px 4px 4px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 18px; font-weight: 400; line-height: 25px;'> <h2 style='font-size: 20px; font-weight: 400; color: #111111; margin: 0;'>Need more help?</h2> <p style='margin: 0;'>We are here to help! If you have any questions or specific service requirements, please contact your territory sales manager at <span style='color: #462BC8;'>702-420-8903 </span>or send an email to <a href='mailto:info@webportal.com'>info@webportal.com</a></p></td></tr></table> </td></tr><tr> <td bgcolor='#F4F4F4' align='center' style='padding: 0px 10px 0px 10px;'> <table border='0' cellpadding='0' cellspacing='0' width='100%' style='max-width: 600px;'> <tr> <td bgcolor='#F4F4F4' align='left' style='padding: 0px 30px 30px 30px; color: #666666; font-family: 'Lato', Helvetica, Arial, sans-serif; font-size: 14px; font-weight: 400; line-height: 18px;'> <br></td></tr></table> </td></tr></table></body></html>";
                    message.IsBodyHtml = true;
                    message.Body = emailTemplate;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("cthings01@gmail.com", "Noman@143");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                    return View("Index");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
            }
            return NoContent();

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
