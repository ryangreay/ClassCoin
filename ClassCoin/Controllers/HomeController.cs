using SendGrid;
using SendGrid.Helpers.Mail;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassCoin.ViewModels;
using ClassCoin.Models;
using ClassCoin.DAL;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;

namespace ClassCoin.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ClassCoinContext db = new ClassCoinContext();
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }

        public HomeController()
        {
        }

        public HomeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Landing()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> InstructorSignup(InstructorRegistrationViewModel signupVM)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("InstructorSignup_Partial");
            }

            User user = new User()
            {
                Firstname = signupVM.Firstname,
                Lastname = signupVM.Lastname,
                Email = signupVM.Email,
                //Password = signupVM.Password,
                //UserConfirmed = false
            };
            db.Users.Add(user);
            db.SaveChanges();

            Instructor instructor = new Instructor() { Institution = signupVM.Institution/*, UserID = user.Id*/};
            db.Instructors.Add(instructor);
            db.SaveChanges();

            var client = new SendGridClient(WebConfigurationManager.AppSettings["SendGridAPI"]);
            string body = string.Format("Dear {0}, <br /> Thank you for registering to ClassCoin, please click on the " +
               "below link to complete your registration: <a href =\"{1}\" title =\"User Email Confirm\">{1}</a>", user.Firstname, Url.Action("ConfirmEmail", "Home",
                new { Token = user.Id, Email = user.Email }, Request.Url.Scheme));
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("classcoinco@gmail.com", "ClassCoin Team"),
                Subject = "ClassCoin Email Confirmation",
                HtmlContent = body
            };
            msg.AddTo(new EmailAddress(user.Email, user.Firstname + " " + user.Lastname));
            var response = await client.SendEmailAsync(msg);

            return RedirectToAction("Confirm", "Home", new { Message = "Thank you for signing up. \nPlease check your email to confirm your account. \n If the email does not arrive, please check your spam." });

            //return PartialView("InstructorSignup_Partial");
        }      

        [HttpPost]
        [AllowAnonymous]
        public ActionResult StudentSignup(StudentRegistrationViewModel signupVM)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("StudentSignup_Partial");
            }

            //if ()
            //{
            User user = new User()
            {
                Firstname = signupVM.Firstname,
                Lastname = signupVM.Lastname,
                Email = signupVM.Email,
                //Password = signupVM.Password,
                //UserConfirmed = false
            };
            db.Users.Add(user);
            db.SaveChanges();

            Student student = new Student() { Funds = 0.00/*, UserID = user.UserID */};
            db.Students.Add(student);
            db.SaveChanges();
                
            //automatically log user in and direct to student dashboard
            ViewBag.RegisterSuccess = true;
            //}

            return PartialView("StudentSignup_Partial");
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DoesUserNameExist(string userName)
        {           
            return Json(!db.Users.Where(val => val.UserName == userName).Any()); //user == null
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult DoesEmailExist(string email)
        {
            return Json(!db.Users.Where(val => val.Email == email).Any()); //user == null
        }

        [AllowAnonymous]
        public ActionResult Confirm(string Message)
        {
            ViewBag.Message = Message;
            return View();
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string Token, string Email)
        {
            //int userID = int.Parse(Token);
            User user = db.Users.Where(val => val.Id == Token).FirstOrDefault();
            if (user != null)
            {
                if (user.Email == Email)
                {
                    user.EmailConfirmed = true;
                    db.SaveChanges();
                    return RedirectToAction("Confirm", "Home", new { Message = "You have successfully registered with " + Email + ". You can now log in." });
                }
                else
                {
                    return RedirectToAction("Confirm", "Home", new { Message = "Something went wrong and we could not register you. Please try again. If this issue persists, please contact us." });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Home", new { Message = "Something went wrong and we could not register you. Please try again. If this issue persists, please contact us." });
            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel loginVM)
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel forgotPassVM)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(forgotPassVM.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password",
                   "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                TempData["ViewBagLink"] = callbackUrl;
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(forgotPassVM);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult EmailConfirmationTest()
        {
            return View();
        }
    }
}