using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Account.Plugin.Controllers.Account
{
    public partial class AccountController : Controller
    {
        #region Public Action Methods

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        #endregion Public Action Methods
    }
}