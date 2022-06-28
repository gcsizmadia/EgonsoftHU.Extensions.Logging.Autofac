// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Reflection;

using Autofac;
using Autofac.Features.ResolveAnything;

using Xunit.Abstractions;
using Xunit.Frameworks.Autofac;
using Xunit.Frameworks.Autofac.TestFramework;
using Xunit.Sdk;

namespace EgonsoftHU.Extensions.Logging.Autofac.UnitTests
{
    public class AutofacLoggingTestFramework : AutofacLoggingTestFrameworkBase<AutofacLoggingTestFramework>
    {
        public AutofacLoggingTestFramework(IMessageSink diagnosticMessageSink)
            : base(diagnosticMessageSink)
        {
        }

        /// <inheritdoc/>
        /// <remarks>
        /// This method is overridden so that it uses the <see cref="CreateContainer"/> method instead of the method used by the base class.
        /// </remarks>
        protected override ITestFrameworkExecutor CreateExecutor(AssemblyName assemblyName)
        {
            return new AutofacTestFrameworkExecutor(assemblyName, CreateContainer(), SourceInformationProvider, DiagnosticMessageSink);
        }

        /// <remarks>
        /// Although this implementation is the same as in the <see cref="AutofacTestFramework"/> base class,
        /// this method had to be implemented in this project as well due to the following reasons:
        /// <list type="bullet">
        /// <item>This project references <c>Autofac 5.2.0</c> nuget package.</item>
        /// <item>The <c>xunit.frameworks.autofac</c> nuget package does not have a version that references <c>Autofac 5.*</c> nuget package.</item>
        /// <item>The <c>xunit.framework.autofac 0.3.0</c> nuget package references <c>Autofac 4.8.1</c> nuget package.</item>
        /// <item>
        /// There is a breaking change between <c>Autofac 4</c> and <c>Autofac 5</c> nuget packages:
        /// <list type="bullet">
        /// <item>
        /// In <c>Autofac 4</c> the <c>ContainerBuilder.RegisterSource()</c> extension method is declared in <see cref="RegistrationExtensions"/> static class.
        /// </item>
        /// <item>
        /// In <c>Autofac 5</c> the <c>ContainerBuilder.RegisterSource()</c> extension method is declared in <see cref="SourceRegistrationExtensions"/> static class.
        /// </item>
        /// </list>
        /// </item>
        /// </list>
        /// </remarks>
        private IContainer CreateContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            builder.RegisterType<TestOutputHelper>().As<ITestOutputHelper>().AsSelf().InstancePerTest();

            ConfigureContainer(builder);

            return builder.Build();
        }
    }
}
