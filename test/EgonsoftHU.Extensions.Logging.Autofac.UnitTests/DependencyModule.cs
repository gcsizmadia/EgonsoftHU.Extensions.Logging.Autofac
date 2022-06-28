// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

using Xunit.Frameworks.Autofac;

namespace EgonsoftHU.Extensions.Logging.Autofac.UnitTests
{
    public class DependencyModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
            services.AddLogging();

            builder.Populate(services);

            builder
                .RegisterType<TestService>()
                .AsSelf()
                .InstancePerTest();
        }
    }
}
