// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Collections.Generic;

using Autofac;
using Autofac.Core;
using Autofac.Core.Registration;
using Autofac.Core.Resolving.Pipeline;

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
            new(
                (parameter, context) => typeof(ILogger) == parameter.ParameterType,
                (parameter, context) => context.Resolve(typeof(ILogger<>).MakeGenericType(parameter.Member.DeclaringType ?? typeof(object)))
            );

        /// <inheritdoc/>
        protected override void AttachToComponentRegistration(IComponentRegistryBuilder componentRegistry, IComponentRegistration registration)
        {
            registration.PipelineBuilding += (sender, e) =>
            {
                e.Use(
                    "Microsoft.Extensions.Logging.ILogger instance injection middleware",
                    PipelinePhase.ParameterSelection,
                    MiddlewareInsertionMode.StartOfPhase,
                    (context, next) =>
                    {
                        var parameters = new List<Parameter>(context.Parameters) { loggerParameter };

                        context.ChangeParameters(parameters);

                        next.Invoke(context);
                    }
                );
            };
        }
    }
}
