using System;
using System.Collections.Generic;
using System.Text;

using SharedPluginFeatures;

namespace AspNetCore.PluginManager.Tests.MVCMiddlewareTests
{
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
