// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System;

using Autofac;
using Autofac.Core.Registration;

using EgonsoftHU.Extensions.Logging.Serilog;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Xunit;
using Xunit.Abstractions;

namespace EgonsoftHU.Extensions.Logging.Autofac.UnitTests
{
    public abstract class LoggerInjectionUnitTestBase<T> : UnitTest<T>
        where T : LoggerInjectionUnitTestBase<T>
    {
        public LoggerInjectionUnitTestBase(ILifetimeScope lifetimeScope, ITestOutputHelper output, LoggingFixture<T> fixture)
            : base(lifetimeScope, output, fixture)
        {
            Logger.Here().Verbose("UnitTest class created");
        }

        [Fact]
        public void DirectlyResolvedGenericLoggerShouldBeSameAsTheInjectedNonGenericLogger()
        {
            Logger.Here().Verbose("Running test");

            // Arrange
            ILogger<TestService> expected = Scope.Resolve<ILogger<TestService>>();

            // Act
            TestService sut = Scope.Resolve<TestService>();

            // Assert
            sut.LoggerNonGeneric.Should().BeSameAs(expected, "because ILogger<> service type is registered as singleton");
        }

        [Fact]
        public void InTestServiceGenericLoggerShouldBeSameAsNonGenericLogger()
        {
            Logger.Here().Verbose("Running test");

            // Arrange

            // Act
            TestService sut = Scope.Resolve<TestService>();

            // Assert
            sut.LoggerNonGeneric.Should().BeSameAs(sut.LoggerGeneric, "because ILogger<> service type is registered as singleton");
        }

        [Fact]
        public void ResolveILoggerDirectlyShouldFail()
        {
            Logger.Here().Verbose("Running test");

            // Arrange

            // Act
            Func<ILogger> sut = () => { return Scope.Resolve<ILogger>(); };

            // Assert
            sut.Should().Throw<ComponentNotRegisteredException>("because ILogger service type is not registered");
        }
    }
}
