// Copyright © 2022 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.Runtime.CompilerServices;

using Serilog;

namespace EgonsoftHU.Extensions.Logging.Autofac.UnitTests
{
    internal static class ILoggerExtensions
    {
        internal static ILogger Here(this ILogger logger, [CallerMemberName] string callerMemberName = "")
        {
            return logger.ForContext("SourceMember", callerMemberName);
        }
    }
}
