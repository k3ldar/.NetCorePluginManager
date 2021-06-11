using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.MVCMiddlewareTests
{
    [ExcludeFromCodeCoverage]
    public class SmokeTestMockA
    {
        public SmokeTestMockA(int count)
        {

        }

        [SmokeTest]
        public WebSmokeTestItem TestThatWillFailAsItHasParameters(int a, int b)
        {
            return null;
        }
    }

    [ExcludeFromCodeCoverage]
    public class SmokeTestMockB
    {
        public SmokeTestMockB()
        {

        }

        [SmokeTest]
        public WebSmokeTestItem TestThatWillFailAsItHasParameters(int a, int b)
        {
            return null;
        }
    }
}
