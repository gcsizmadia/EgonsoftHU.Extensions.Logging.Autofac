// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Linq;

using Autofac;
using Autofac.Core;

using EgonsoftHU.Extensions.Bcl;

using Microsoft.Extensions.Logging;

namespace EgonsoftHU.Extensions.Logging.Autofac
{
    /// <summary>
    /// A dependency module (derived from <see cref="Module"/>) that enables injecting
    /// the same contextual logger as <see cref="ILogger"/> instead of <see cref="ILogger{TCategoryName}"/>.
    /// </summary>
    public class DependencyModule : Module
    {
        private static readonly ResolvedParameter loggerParameter =
            new ResolvedParameter(
                (parameter, context) => typeof(ILogger) == parameter.ParameterType,
                (parameter, context) => context.Resolve(typeof(ILogger<>).MakeGenericType(parameter.Member.DeclaringType))
            );

        /// <inheritdoc/>
        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Preparing +=
                (sender, e) =>
                {
                    e.Parameters = e.Parameters.Union(loggerParameter.AsEnumerable()).ToList();
                };
        }
    }
}
