using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

using SharedPluginFeatures;
using Shared.Classes;

using UserAccount.Plugin.Models;

namespace UserAccount.Plugin.Controllers
{
    public partial class AccountController : BaseController
    {
		[HttpGet]
		public IActionResult ChangePassword()
        {
            UserSession userSession = GetUserSession();
            ChangePasswordViewModel model = new ChangePasswordViewModel();
            return View();
        }

		[HttpPost]
		public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            return View();
        }
    }
}
