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
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
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
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Shared.Classes;

using SharedPluginFeatures;

#pragma warning disable IDE0060, IDE0079, S3400

namespace DemoApiPlugin.Controllers
{
    [Route("api/DemoApi")]
    [DenySpider]
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

        [HttpGet]
        [Route("/api/Demo/", Name = "About")]
        public string About()
        {
            return ("Test");
        }

        [HttpGet]
        [Route("/api/Demo/About1")]
        [DenySpider]
        public string About1()
        {
            return ("Test");
        }

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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Forms part of route name")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Forms part of route name")]
        public void Post([FromBody] string value)
        {
			// demo method
		}

		// PUT api/values/5
		[HttpPut("{id}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Forms part of route name")]
        public void Put(int id, [FromBody] string value)
        {
			// demo method
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Forms part of route name")]
        public void Delete(int id)
        {
			// demo method
		}

		[HttpGet]
        [Route("/api/ex")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Used as a route to raise an exception")]
        public void RaiseError()
        {
            List<string> list = null;

            list.Add("oops");
        }

        [HttpGet]
        [Route("/api/invaliduserNoPolicy")]
        [ApiAuthorization]
        public IActionResult RequiresAuthorization(string s)
        {
            return new JsonResult(new { response = $"If you can see this you passed the security without a policy: {s}" });
        }

        [Route("/api/invaliduserPolicy")]
        [ApiAuthorization("Policy")]
        public IActionResult RequiresAuthorizationPolicy()
        {
            return new JsonResult(new { response = "If you can see this you passed the security with a policy" });
        }

        [HttpGet]
        [Route("/api/testApi")]
        public IActionResult TestApi(string merchantId, string apiKey, string secret)
        {
            ulong nonce = (ulong)DateTime.UtcNow.Ticks;
            long timestamp = HmacGenerator.EpochDateTime();

            using (HttpClient client = new HttpClient())
            {
                string auth = HmacGenerator.GenerateHmac(apiKey, secret, timestamp, nonce, merchantId, String.Empty);

                client.DefaultRequestHeaders.Add("apikey", apiKey);
                client.DefaultRequestHeaders.Add("merchantId", merchantId);
                client.DefaultRequestHeaders.Add("nonce", nonce.ToString());
                client.DefaultRequestHeaders.Add("timestamp", timestamp.ToString());
                client.DefaultRequestHeaders.Add("authcode", auth);
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/4.0 (Compatible; Windows NT 5.1; MSIE 6.0)");
                Task<HttpResponseMessage> result = client.GetAsync("https://localhost:5001/api/invaliduserPolicy");
                string response = result.Result.Content.ReadAsStringAsync().Result;

                return Content(response);
            }

        }
    }
}

#pragma warning restore IDE0060, IDE0079, S3400