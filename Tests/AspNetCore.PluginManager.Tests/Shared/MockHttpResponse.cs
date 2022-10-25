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
 *  Copyright (c) 2018 - 2022 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockHttpResponse.cs
 *
 *  Purpose:  Mock HttpResponse class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockHttpResponse : Microsoft.AspNetCore.Http.HttpResponse
    {
        #region Private Members

        private readonly MockResponseCookies _cookies = new MockResponseCookies();
        private readonly IHeaderDictionary _headerDictionary = new MockHeaderDictionary();
        private readonly Stream _body;
        private readonly PipeWriter _pipeWriter;

        #endregion Private Members

        public MockHttpResponse()
        {
            _body = new MemoryStream();
            _pipeWriter = PipeWriter.Create(_body);
            StatusCode = 200;
            RedirectPermanent = null;
            RedirectCount = 0;
        }

        #region HttpResponse Methods

        public override Stream Body
        {
            get
            {
                return _body;
            }

            set => throw new NotImplementedException();
        }

        public override Int64? ContentLength { get => _body.Length; set => throw new NotImplementedException(); }
        public override String ContentType { get; set; }

        public override IResponseCookies Cookies
        {
            get
            {
                return _cookies;
            }
        }

        public override Boolean HasStarted => ContentLength > 0 || TestHasStarted;

        public override IHeaderDictionary Headers
        {
            get
            {
                return _headerDictionary;
            }
        }

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
            RedirectLocation = location;
            RedirectPermanent = permanent;
            RedirectCount++;
        }

        public override Task StartAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public override Task CompleteAsync()
        {
            return Task.CompletedTask;
        }

        public override PipeWriter BodyWriter => _pipeWriter;

        #endregion HttpResponse Methods

        public int RedirectCount { get; set; }

        public bool? RedirectPermanent { get; set; }

        public string RedirectLocation { get; set; }

        public bool TestHasStarted { get; set; }
    }
}
