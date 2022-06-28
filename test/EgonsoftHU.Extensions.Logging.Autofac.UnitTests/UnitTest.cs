// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;

using Autofac;

using Serilog;

using Xunit;
using Xunit.Abstractions;
using Xunit.Frameworks.Autofac;

namespace EgonsoftHU.Extensions.Logging.Autofac.UnitTests
{
    [UseAutofacTestFramework]
    public abstract class UnitTest<T> : IClassFixture<LoggingFixture<T>>, IDisposable
    {
        private readonly LoggingFixture<T> fixture;

        protected UnitTest(ILifetimeScope lifetimeScope, ITestOutputHelper output, LoggingFixture<T> fixture)
        {
            Scope = lifetimeScope.BeginLifetimeScope();

            this.fixture = fixture;
            fixture.InitializeLogger(output);
        }

        protected ILifetimeScope Scope { get; private set; }

        protected ILogger Logger => fixture.Logger;

        #region Dispose pattern implementation

        private bool isDisposed;

        protected virtual void Dispose(bool isDisposing)
        {
            if (!isDisposed)
            {
                if (isDisposing)
                {
                    Scope.Dispose();
                    Scope = null;
                }

                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(isDisposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
