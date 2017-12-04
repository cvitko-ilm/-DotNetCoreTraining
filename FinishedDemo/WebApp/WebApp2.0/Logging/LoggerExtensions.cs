using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp2_0.Logging
{
    public static class LoggerExtensions
    {
        private static readonly Action<ILogger, Exception> _viewRequested;
        private static readonly Action<ILogger, string, int, Exception> _viewRequestedOptions;

        static LoggerExtensions()
        {
            _viewRequested = LoggerMessage.Define(LogLevel.Error,
                new EventId(1, nameof(ViewRequested)),
                "Issue with View Requested");

            _viewRequestedOptions = LoggerMessage.Define<string, int>(LogLevel.Error,
                new EventId(1, nameof(ViewRequestedOptions)),
                "Issue with View Requested (Options: {Options}, Id: {Id})");
        }

        public static void ViewRequested(this ILogger logger)
        {
            _viewRequested(logger, null);
        }

        public static void ViewRequestedOptions(this ILogger logger, string options, int id = 10)
        {
            _viewRequestedOptions(logger, options, id, null);
        }
    }
}
