using System;
using System.Reflection;

using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace SharedPluginFeatures
{
    public interface IRouteDataService
    {
        string GetRouteFromClass(Type type, IActionDescriptorCollectionProvider routeProvider);

        string GetRouteFromMethod(in MethodInfo method, in IActionDescriptorCollectionProvider routeProvider);
    }
}
