using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;

namespace UserAccount.Plugin.Controllers
{
    [LoggedIn]
    public partial class AccountController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}