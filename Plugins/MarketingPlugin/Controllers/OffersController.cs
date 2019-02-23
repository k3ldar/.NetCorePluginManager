/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2019 Simon Carter.  All Rights Reserved.
 *
 *  Product:  MarketingPlugin
 *  
 *  File: OffersController.cs
 *
 *  Purpose:  Marketing Controller
 *  
 *  Date        Name                Reason
 *  21/02/2019  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MarketingPlugin.Models;

using SharedPluginFeatures;

namespace MarketingPlugin.Controllers
{
    public class OffersController : BaseController
    {
        #region Private Members


        #endregion Private Members

        #region Constructors


        #endregion Constructors

        #region Public Action Methods

        public IActionResult Index()
        {
            return View();
        }

        [Route("Offers/{campaignName}/{id}/")]
        public IActionResult Offers(string campaignName, int id)
        {
            return View();
        }

        #endregion Public Action Methods
    }
}
