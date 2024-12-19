/* * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * *
 *  .Net Core Plugin Manager is distributed under the GNU General Public License version 3 and  
 *  is also available under alternative licenses negotiated directly with Simon Carter.  
 *  If you obtained Service Manager under the GPL, then the GPL applies to all loadable 
 *  Service Manager modules used on your system as well. The GPL (version 3) is 
 *  available at https://opensource.org/licenses/GPL-3.0
 *
 *  This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY,
 *  without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
 *  See the GNU General Public License for more details.
 *
 *  The Original Code was created by Simon Carter (s1cart3r@gmail.com)
 *
 *  Copyright (c) 2018 - 2023 Simon Carter.  All Rights Reserved.
 *
 *  Product:  AspNetCore.PluginManager.Tests
 *  
 *  File: MockHttpContext.cs
 *
 *  Purpose:  Mock HttpContext class
 *
 *  Date        Name                Reason
 *  20/04/2020  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;
using System.Threading;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

using Shared.Classes;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.Shared
{
    [ExcludeFromCodeCoverage]
    public class MockHttpContext : HttpContext
    {
        #region Private Members

        private readonly MockHttpRequest _httpRequest;
        private readonly MockHttpResponse _httpResponse;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<BreadcrumbItem> _breadcrumbs;
        private IDictionary<object, object> _items;
        private ClaimsPrincipal _user;

        #endregion Private Members

        #region Constructors

        public MockHttpContext()
        {
            _httpRequest = new MockHttpRequest();
            _httpResponse = new MockHttpResponse();
            CreateSession = true;
        }

        public MockHttpContext(in MockHttpRequest httpRequest, in MockHttpResponse httpResponse,
            List<BreadcrumbItem> breadcrumbs = null)
        {
            _httpRequest = httpRequest ?? throw new ArgumentNullException(nameof(httpRequest));
            _httpResponse = httpResponse ?? throw new ArgumentNullException(nameof(httpResponse));
            CreateSession = true;
            _breadcrumbs = breadcrumbs;
        }

        public MockHttpContext(in MockHttpRequest httpRequest, in MockHttpResponse httpResponse,
            IServiceProvider serviceProvider)
            : this(httpRequest, httpResponse)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public MockHttpContext(in MockHttpRequest httpRequest, in MockHttpResponse httpResponse,
            IServiceProvider serviceProvider, List<BreadcrumbItem> breadcrumbs)
            : this(httpRequest, httpResponse, serviceProvider)
        {
            _breadcrumbs = breadcrumbs;
        }

        #endregion Constructors

        #region Properties

        public bool CreateSession { get; set; }

        public bool LogUserIn { get; set; }

        #endregion Properties

        #region HttpContext Methods

        public override ConnectionInfo Connection
        {
            get
            {
                string ip = _httpRequest.IpAddress;

                if (String.IsNullOrEmpty(ip))
                    ip = "10.11.12.13";

                return new MockConnectionInfo()
                {
                    RemoteIpAddress = IPAddress.Parse(ip)
                };
            }
        }


        public override IFeatureCollection Features
        {
            get
            {
                return null;
            }
        }


        public override IDictionary<object, object> Items
        {
            get
            {
                if (_items == null)
                {
                    _items = new Dictionary<object, object>
                    {
                        { Constants.BasketSummary, new ShoppingCartSummary(1, 0, 0, 0, 0, 0, new System.Globalization.CultureInfo("en-GB"), "GBP") },
                    };

                    if (CreateSession)
                        _items.Add(Constants.UserSession, new UserSession() { InternalSessionID = DateTime.Now.Ticks });

					if (CreateSession && LogUserIn)
					{
						((UserSession)_items[Constants.UserSession]).UserName = "john.doe@test.com";
						((UserSession)_items[Constants.UserSession]).UserEmail = "john.doe@test.com";
					}

                    if (_breadcrumbs != null)
                    {
                        _items.Add(Constants.Breadcrumbs, _breadcrumbs);
                    }
                }

                return _items;
            }

            set
            {

            }
        }


        public override HttpRequest Request
        {
            get
            {
                return _httpRequest;
            }
        }


        public override CancellationToken RequestAborted
        {
            get
            {
                return CancellationToken.None;
            }

            set
            {
            }
        }

        public override IServiceProvider RequestServices
        {
            get
            {
                return _serviceProvider;
            }

            set
            {
                throw new InvalidOperationException();
            }
        }


        public override Microsoft.AspNetCore.Http.HttpResponse Response
        {
            get
            {
                return _httpResponse;
            }
        }


        public override ISession Session
        {
            get
            {
                return new MockSession();
            }

            set
            {
            }
        }

        public override String TraceIdentifier
        {
            get
            {
                return null;
            }

            set
            {
            }
        }

        public override ClaimsPrincipal User
        {
            get
            {
                if (_user == null)
                    return new ClaimsPrincipal();

                return _user;
            }

            set
            {
                _user = value;
            }
        }

        public override WebSocketManager WebSockets
        {
            get
            {
                return null;
            }
        }


        public override void Abort()
        {

        }

        #endregion HttpContext Methods
    }
}
