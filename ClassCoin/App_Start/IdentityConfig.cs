using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using ClassCoin.Models;
using SendGrid;
using System.Net;
using System.Configuration;
using System.Diagnostics;
using System.Web.Configuration;
using SendGrid.Helpers.Mail;
using ClassCoin.DAL;

namespace ClassCoin
{
   public class EmailService : IIdentityMessageService
   {
      public async Task SendAsync(IdentityMessage message)
      {
         await configSendGridasync(message);
      }

      // Use NuGet to install SendGrid (Basic C# client lib) 
      private async Task configSendGridasync(IdentityMessage message)
      {
          var client = new SendGridClient(WebConfigurationManager.AppSettings["SendGridAPI"]);
       
          var msg = new SendGridMessage()
          {
              From = new EmailAddress("classcoinco@gmail.com", "ClassCoin Team"),
              Subject = message.Subject,
              HtmlContent = message.Body
          };
          msg.AddTo(new EmailAddress(message.Destination));
          var response = await client.SendEmailAsync(msg);
      }
   }

   // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
   public class ApplicationUserManager : UserManager<User>
   {
      public ApplicationUserManager(IUserStore<User> store)
         : base(store)
      {
      }

      public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
      {
         var manager = new ApplicationUserManager(new UserStore<User>(context.Get<ClassCoinContext>()));
         // Configure validation logic for usernames
         manager.UserValidator = new UserValidator<User>(manager)
         {
            AllowOnlyAlphanumericUserNames = false,
            RequireUniqueEmail = true
         };

         // Configure validation logic for passwords
         //manager.PasswordValidator = new PasswordValidator
         //{
         //    RequiredLength = 6,
         //    RequireNonLetterOrDigit = true,
         //    RequireDigit = true,
         //    RequireLowercase = true,
         //    RequireUppercase = true,
         //};

         // Configure user lockout defaults
         manager.UserLockoutEnabledByDefault = true;
         manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
         manager.MaxFailedAccessAttemptsBeforeLockout = 12;

         manager.EmailService = new EmailService();
         var dataProtectionProvider = options.DataProtectionProvider;
         if (dataProtectionProvider != null)
         {
            manager.UserTokenProvider =
                new DataProtectorTokenProvider<User>(dataProtectionProvider.Create("ASP.NET Identity"));
         }
         return manager;
      }
   }

   // Configure the application sign-in manager which is used in this application.
   public class ApplicationSignInManager : SignInManager<User, string>
   {
      public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
         : base(userManager, authenticationManager)
      {
      }

      public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
      {
         return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
      }

      public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
      {
         return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
      }
   }
}
