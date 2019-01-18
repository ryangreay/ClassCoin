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

        private async Task SendEmailConfirmationAsync(User user, string subject)
        {
            //instead of this message, we want to use an already created html template, which is located at Home/EmailConfirmationTest.cshtml
            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            string body = string.Format("Dear {0}, <br /> Thank you for registering to ClassCoin, please click on the " +
               "below link to complete your registration: <a href =\"{1}\" title =\"User Email Confirm\">{1}</a>", user.Firstname, Url.Action("ConfirmEmail", "Home",
                new { userId = user.Id, code = code }, Request.Url.Scheme));

            //IdentityMessage message = new IdentityMessage { Body = body, Destination = user.Email, Subject = subject};
            //await UserManager.EmailService.SendAsync(message);
            await UserManager.SendEmailAsync(user.Id, subject, body);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> InstructorSignup(InstructorRegistrationViewModel signupVM)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Firstname = signupVM.Firstname,
                    Lastname = signupVM.Lastname,
                    UserName = signupVM.Email,
                    Email = signupVM.Email
                };

                var result = await UserManager.CreateAsync(user, signupVM.Password);

                if (result.Succeeded)
                {
                    Instructor instructor = new Instructor() { Institution = signupVM.Institution, UserId = user.Id };
                    db.Instructors.Add(instructor);
                    db.SaveChanges();

                    await SendEmailConfirmationAsync(user, "ClassCoin Email Confirmation");

                    return RedirectToAction("Confirm", "Home", new { Message = "Thank you for signing up. \nPlease check your email to confirm your account. \n If the email does not arrive, please check your spam." });
                }
            }
            return PartialView("InstructorSignup_Partial");

        }      

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> StudentSignup(StudentRegistrationViewModel signupVM)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Firstname = signupVM.Firstname,
                    Lastname = signupVM.Lastname,
                    UserName = signupVM.Email,
                    Email = signupVM.Email
                };

                var result = await UserManager.CreateAsync(user, signupVM.Password);

                if (result.Succeeded)
                {
                    Student student = new Student() { Funds = 0.00, UserId = user.Id};
                    db.Students.Add(student);
                    db.SaveChanges();

                    return RedirectToAction("Confirm", "Home", new { Message = "Thank you for signing up. \n Please <a href=\"" + Url.Action("Login", "Home", null) + "\">login here</a>."});
                }
            }

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
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Confirm", "Home", new { Message = "Something went wrong and we could not register you. Please try again. If this issue persists, please contact us." });
            }

            var result = await UserManager.ConfirmEmailAsync(userId, code);         

            if (result.Succeeded)
                return RedirectToAction("Confirm", "Home", new { Message = "You have successfully registered. You can now <a href=\"" + Url.Action("Login", "Home", null) + "\">log in</a>." });
            else
                return RedirectToAction("Confirm", "Home", new { Message = "Something went wrong and we could not register you. Please try again. If this issue persists, please contact us." });

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            // Require the user to have a confirmed email before they can log on.
            // var user = await UserManager.FindByNameAsync(model.Email);
            var user = UserManager.Find(loginVM.Username, loginVM.Password);
            if (user != null)
            {
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    await SendEmailConfirmationAsync(user, "Confirm your account-Resend");

                    ViewBag.Message = "You must have a confirmed email to log on. "
                                         + "The confirmation has been resent to your email account.";
                    return View(loginVM);
                }
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(loginVM.Username, loginVM.Password, loginVM.RememberMe, shouldLockout: true);
            bool isTeacher = true;
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction(isTeacher ? "TeacherDashboard" : "StudentDashboard", "Dashboard", null);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(loginVM);
            }
        }

        

        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            ViewBag.Message = code == null ? "There was an error resetting your password. Please try again." : "";
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("Confirm", "Home", new { Message = ""});
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Confirm", "Home", new { Message = "Password reset was successful. You can now <a href=\"" + Url.Action("Login", "Home", null) + "\">log in</a>."});
            }
            AddErrors(result);
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
            //students need to contact the instructor to get their new password

            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(forgotPassVM.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View();
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Home", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, "Reset Password",
                   "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                TempData["ViewBagLink"] = callbackUrl;
                return RedirectToAction("Confirm", "Home", new { Message = "Please check your email to reset your password." });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Landing", "Home");
        }

        [AllowAnonymous]
        public ActionResult EmailConfirmationTest()
        {
            return View();
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
               : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

    }
}