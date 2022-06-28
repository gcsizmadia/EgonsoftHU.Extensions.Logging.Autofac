// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;

using EgonsoftHU.Extensions.DependencyInjection.Autofac;
using EgonsoftHU.Extensions.Logging.Serilog;

using Serilog;

using Xunit;
using Xunit.Abstractions;
using Xunit.Frameworks.Autofac;

namespace EgonsoftHU.Extensions.Logging.Autofac.UnitTests
{
    public abstract class AutofacLoggingTestFrameworkBase<T> : AutofacTestFramework, IClassFixture<LoggingFixture<T>>
        where T : AutofacLoggingTestFrameworkBase<T>
    {
        private readonly LoggingFixture<T> fixture;
        private readonly IMessageSink diagnosticMessageSink;

        protected AutofacLoggingTestFrameworkBase(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
            this.diagnosticMessageSink = diagnosticMessageSink;

            fixture = new LoggingFixture<T>();
            fixture.InitializeLogger(diagnosticMessageSink);

            DisposalTracker.Add(fixture);

            Logger.Here().Verbose("TestFramework created");
        }

        protected ILogger Logger => fixture.Logger ?? fixture.InitializeLogger(diagnosticMessageSink);

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            builder.UseDefaultAssemblyRegistry(nameof(EgonsoftHU));
            builder.RegisterModule<DependencyInjection.Autofac.DependencyModule>();
        }
    }
}
