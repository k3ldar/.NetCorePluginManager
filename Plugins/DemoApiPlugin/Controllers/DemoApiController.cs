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
 *  Copyright (c) 2018 Simon Carter.  All Rights Reserved.
 *
 *  Product:  Demo Api Plugin
 *  
 *  File: DemoApiController.cs
 *
 *  Purpose:  
 *
 *  Date        Name                Reason
 *  22/09/2018  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using SharedPluginFeatures;
using Shared.Classes;

namespace DemoApiPlugin.Controllers
{
    [Route("api/DemoApi")]
    [ApiController]
    public class DemoApiController : ControllerBase
    {
        #region Private Members

        private readonly IMemoryCache _memoryCache;

        #endregion Private Members

        #region Constructors

        public DemoApiController(IMemoryCache memoryCache)
        {
            // Memory Cache is initialised during the Plugin Manager and set to be injected in using DI
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        #endregion Constructors

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            CacheItem hitCount = _memoryCache.GetCache().Get("Demo API Controller, Get Count");

            if (hitCount == null)
            {
                hitCount = new CacheItem("Demo API Controller, Get Count", new HitCount());
                _memoryCache.GetCache().Add("Demo API Controller, Get Count", hitCount);
            }

            ((HitCount)hitCount.Value).Count++;

            return new string[] { "Demo Api Plugin", $"This Api has been called {((HitCount)hitCount.Value).Count} times" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
