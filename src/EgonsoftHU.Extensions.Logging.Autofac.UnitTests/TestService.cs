// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Microsoft.Extensions.Logging;

namespace EgonsoftHU.Extensions.Logging.Autofac.UnitTests
{
    public class TestService
    {
        public TestService(ILogger loggerNonGeneric, ILogger<TestService> loggerGeneric)
        {
            LoggerNonGeneric = loggerNonGeneric;
            LoggerGeneric = loggerGeneric;
        }

        public ILogger LoggerNonGeneric { get; }

        public ILogger<TestService> LoggerGeneric { get; }
    }
}
