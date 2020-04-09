using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Shared.Classes;
using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.PluginFeatures
{
    internal class BaseControllerWrapper : BaseController
    {
        public BaseControllerWrapper()
        {
            ControllerContext.HttpContext = new TestHttpContext();
        }

        internal string TestGetCoreSessionId()
        {
            return GetCoreSessionId();
        }

        internal ShoppingCartSummary TestGetShoppingCartSummary()
        {
            return GetCartSummary();
        }

        internal void TestCalculatePageOffsets<T>(List<T> items, int page, int pageSize,
            out int startItem, out int endItem, out int availablePages)
        {
            CalculatePageOffsets<T>(items, page, pageSize, out startItem, out endItem, out availablePages);
        }

        internal void TestCalculatePageOffsets(int totalItems, int page, int pageSize, 
            out int startItem, out int endItem, out int availablePages)
        {
            CalculatePageOffsets(totalItems, page, pageSize, out startItem, out endItem, out availablePages);
        }
    }

    public class TestSession : ISession
    {
        public Boolean IsAvailable
        {
            get
            {
                return true;
            }
        }

        public String Id
        {
            get
            {
                return "abc123";
            }
        }

        public IEnumerable<String> Keys
        {
            get
            {
                return new List<string>();
            }
        }

        public void Clear()
        {
            
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(String key)
        {
            
        }

        public void Set(String key, Byte[] value)
        {
            
        }

        public Boolean TryGetValue(String key, out Byte[] value)
        {
            value = Array.Empty<Byte>();
            return false;
        }
    }

    public class TestHttpContext : HttpContext
    {
        public override ConnectionInfo Connection
        {
            get
            {
                return null;
            }
        }


        public override IFeatureCollection Features
        {
            get
            {
                return null;
            }
        }


        public override IDictionary<Object, Object> Items
        {
            get
            {
                return new Dictionary<object, object>
                {
                    { Constants.BasketSummary, new ShoppingCartSummary(1, 0, 0, 0, 0, 0, new System.Globalization.CultureInfo("en-GB"), "GBP") },
                    { Constants.UserSession, new UserSession() { InternalSessionID = DateTime.Now.Ticks} }
                };
            }

            set
            {

            }
        }


        public override HttpRequest Request
        {
            get
            {
                return null;
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
                return null;
            }

            set
            {
            }
        }


        public override HttpResponse Response
        {
            get
            {
                return null;
            }
        }


        public override ISession Session
        {
            get
            {
                return new TestSession();
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
                return new ClaimsPrincipal();
            }

            set 
            { 

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
    }
}
