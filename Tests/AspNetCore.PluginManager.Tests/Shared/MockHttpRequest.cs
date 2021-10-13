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
 *  Copyright (c) 2018 - 2021 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockHttpRequest.cs
 *
 *  Purpose:  Mock HttpRequest class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using sp = SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests
{
    [ExcludeFromCodeCoverage]
    public class MockHttpRequest : HttpRequest
    {
        #region Private Members

        private IRequestCookieCollection _requestCookieCollection;
        private HttpContext _httpContext;
        private IHeaderDictionary _headerDictionary;
        private readonly List<KeyValuePair<string, StringValues>> _queryCollection;
        private readonly QueryString _queryString;
        private HostString _hostString = new HostString("http://localhost/");

        #endregion Private Members

        #region Constructors

        public MockHttpRequest()
        {
            Path = "/";
            _queryCollection = new List<KeyValuePair<string, StringValues>>();
            _queryString = new QueryString();
        }

        public MockHttpRequest(IRequestCookieCollection cookies)
            : this()
        {
            _requestCookieCollection = cookies ?? throw new ArgumentNullException(nameof(cookies));
        }

        public MockHttpRequest(HttpContext context, IRequestCookieCollection cookies)
            : this(cookies)
        {
            _httpContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion Constructors

        #region HttpRequest Methods

        public override Stream Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Int64? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override String ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IRequestCookieCollection Cookies
        {
            get
            {
                if (_requestCookieCollection == null)
                    _requestCookieCollection = new MockRequestCookieCollection();

                return _requestCookieCollection;
            }

            set
            {
                _requestCookieCollection = value;
            }
        }
        public override IFormCollection Form { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override Boolean HasFormContentType => throw new NotImplementedException();

        public override IHeaderDictionary Headers
        {
            get
            {
                if (_headerDictionary == null)
                {
                    _headerDictionary = new MockHeaderDictionary();
                    _headerDictionary.Add("User-Agent", "No valid user agent has been set");

                }

                return _headerDictionary;
            }
        }

        public override HostString Host
        {
            get
            {
                return _hostString;
            }

            set => throw new NotImplementedException();
        }

        public override HttpContext HttpContext
        {
            get
            {
                return _httpContext;
            }
        }

        public override Boolean IsHttps { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override String Method { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override PathString Path { get; set; }
        public override PathString PathBase
        {
            get
            {
                return new PathString();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override String Protocol { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override IQueryCollection Query
        {
            get
            {
                return _queryCollection as IQueryCollection;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override QueryString QueryString
        {
            get
            {
                return _queryString;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public override String Scheme
        {
            get
            {
                return IsHttpsScheme ? "https" : "http";
            }

            set => throw new NotImplementedException();
        }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }


        #endregion HttpRequest Methods

        #region Public Methods

        public void SetContext(HttpContext context)
        {
            _httpContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void SetHost(HostString hostString)
        {
            _hostString = hostString;
        }

        public string IpAddress { get; set; }

        public string UserAgent
        {
            get
            {
                return Headers[sp.Constants.UserAgent];
            }
            set
            {
                Headers[sp.Constants.UserAgent] = value;
            }
        }

        public bool IsHttpsScheme { get; set; }

        #endregion Public Methods
    }
}
