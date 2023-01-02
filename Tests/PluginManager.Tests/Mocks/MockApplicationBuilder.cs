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
 *  Product:  PluginManager.Tests
 *  
 *  File: MockApplicationBuilder.cs
 *
 *  Purpose:  Mock IApplicationBuilder for unit tests
 *
 *  Date        Name                Reason
 *  31/03/2021  Simon Carter        Initially Created
 *
 * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace PluginManager.Tests.Mocks
{
    [ExcludeFromCodeCoverage]
    public class MockApplicationBuilder : IApplicationBuilder
    {
        #region Private Members

        private readonly List<Func<RequestDelegate, RequestDelegate>> _middleware = new List<Func<RequestDelegate, RequestDelegate>>();
        private IServiceProvider _serviceProvider;

        #endregion Private Members

        #region Constructors

        public MockApplicationBuilder()
        {
            UseCalled = false;
            UseCalledCount = 0;
            _serviceProvider = new MockServiceProvider(new Dictionary<Type, object>());
        }

        public MockApplicationBuilder(IServiceProvider serviceProvider)
        {
            UseCalled = false;
            UseCalledCount = 0;
            _serviceProvider = serviceProvider;
        }

        #endregion Constructors

        #region Public Properties

        public List<Func<RequestDelegate, RequestDelegate>> Middleware => _middleware;

        #endregion Public Properties

        #region IApplicationBuilder Methods/Properties

        public IServiceProvider ApplicationServices
        {
            get
            {
                return _serviceProvider;
            }

            set
            {
                _serviceProvider = value;
            }
        }

        public IFeatureCollection ServerFeatures => throw new NotImplementedException();

        public IDictionary<string, object> Properties => throw new NotImplementedException();

        public RequestDelegate Build()
        {
            throw new NotImplementedException();
        }

        public IApplicationBuilder New()
        {
            throw new NotImplementedException();
        }

        public IApplicationBuilder Use(Func<RequestDelegate, RequestDelegate> middleware)
        {
            UseCalled = true;
            UseCalledCount++;
            _middleware.Add(middleware);

            if (!UseMvcCalled)
                UseMvcCalled = middleware.Method.ToString().Contains("<UseMiddleware>");

            return this;
        }

        public bool UseMvcCalled { get; private set; }

        public IRouteBuilder GetRouteBuilder()
        {
            return new RouteBuilder(this);
        }

        public ActionDescriptorCollection GetActionDescriptors()
        {

            IActionDescriptorCollectionProvider actionDescriptorProvider = ApplicationServices.GetService<IActionDescriptorCollectionProvider>();
            return actionDescriptorProvider.ActionDescriptors;
        }

        public MvcOptions GetMvcOptions()
        {
            Microsoft.Extensions.Options.ConfigureNamedOptions<MvcOptions> config = (Microsoft.Extensions.Options.ConfigureNamedOptions<MvcOptions>)ApplicationServices.GetService<Microsoft.Extensions.Options.IConfigureOptions<MvcOptions>>();
            MvcOptions mvcOptions = new MvcOptions();
            config.Action.Invoke(mvcOptions);

            return mvcOptions;
        }

        #endregion IApplicationBuilder Methods/Properties

        #region Public Properties

        public bool UseCalled { get; set; }


        public int UseCalledCount { get; private set; }

        #endregion Public Properties
    }
}
