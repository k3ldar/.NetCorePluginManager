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
 *  Copyright (c) 2018 - 2020 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: TestHttpResponse.cs
 *
 *  Purpose:  Mock HttpResponse class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace AspNetCore.PluginManager.Tests
{
    public class TestHttpResponse : HttpResponse
    {
        #region Private Members

        private readonly TestResponseCookies _cookies = new TestResponseCookies();

        #endregion Private Members

        public TestHttpResponse()
        {
            StatusCode = 200;
        }

        #region HttpResponse Methods

        public override Stream Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Int64? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override String ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IResponseCookies Cookies
        {
            get
            {
                return _cookies;
            }
        }

        public override Boolean HasStarted => throw new NotImplementedException();

        public override IHeaderDictionary Headers => throw new NotImplementedException();

        public override HttpContext HttpContext => throw new NotImplementedException();

        public override Int32 StatusCode { get; set; }

        public override void OnCompleted(Func<Object, Task> callback, Object state)
        {
            throw new NotImplementedException();
        }

        public override void OnStarting(Func<Object, Task> callback, Object state)
        {
            throw new NotImplementedException();
        }

        public override void Redirect(String location, Boolean permanent)
        {
            throw new NotImplementedException();
        }

        #endregion HttpResponse Methods
    }
}
