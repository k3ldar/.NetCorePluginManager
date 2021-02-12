using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Middleware;

using Shared.Classes;

namespace AspNetCore.PluginManager.Tests.MiddlewareTests
{
    [ExcludeFromCodeCoverage]
    public class MockSmokeTestProvider : ISmokeTestProvider
    {
        private readonly NVPCodec _codec;

        public MockSmokeTestProvider(NVPCodec codec)
        {
            _codec = codec ?? throw new ArgumentNullException(nameof(codec));
        }

        public MockSmokeTestProvider()
        {
            _codec = null;
        }

        public void SmokeTestEnd()
        {
            EndCalled = true;
        }

        public NVPCodec SmokeTestStart()
        {
            StartCalled = true;
            return _codec;
        }

        public bool StartCalled { get; private set; }

        public bool EndCalled { get; private set; }
    }
}
