using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using HelpdeskPlugin.Models;

using SharedPluginFeatures;

namespace HelpdeskPlugin.Controllers
{
    public class HelpdeskController : BaseController
    {
        #region Private Members


        #endregion Private Members

        #region Public Controller Methods

        [Breadcrumb(nameof(Languages.LanguageStrings.Helpdesk))]
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel(GetBreadcrumbs(), GetCartSummary(),
                true, true, true, String.Empty);

            return View(model);
        }

        #endregion Public Controller Methods
    }
}