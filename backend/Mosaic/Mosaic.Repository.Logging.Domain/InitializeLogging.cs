using Microsoft.Extensions.Logging;
using Mosaic.Repository.Logging.Adapter.Interface;
using NLog.Config;
using NLog.Filters;
using NLog.Targets;
using NLog;
using NLog.Config;
using NLog.Filters;
using NLog.Targets;
using NLog.Targets.Wrappers;
using LogLevel = NLog.LogLevel;

namespace Mosaic.Repository.Logging.Domain;

public class InitializeLogging: IInitializeLogging {
    private InitializeLogging() {}

    public static IInitializeLogging Create() => new InitializeLogging();

    public Task RunAsync() {
        var loggingConfiguration = new LoggingConfiguration();
        
        
        var coloredConsoleTarget = new ColoredConsoleTarget {
            AutoFlush = true,
            UseDefaultRowHighlightingRules = true
        };
        
        var cassandraTarget = new CassandraTarget();

        var bufferingTargetWrapper = new BufferingTargetWrapper() {
            BufferSize = 2,
            WrappedTarget = cassandraTarget,
            Name = "Cassandra",
            FlushTimeout = 10000,
            SlidingTimeout = true,
        };
        
        loggingConfiguration.AddTarget("console", coloredConsoleTarget);
        loggingConfiguration.AddTarget("db", bufferingTargetWrapper);
        
        var filter = new WhenMethodFilter(static info => {
            var result = info.Level.Ordinal switch {
                // Trace;
                // Debug;
                // Info;
                0 or 1 or 2 => LoggerNameIsMicrosoft(info.LoggerName ?? "")
                    ? FilterResult.IgnoreFinal
                    : FilterResult.Log,

                // Warn;
                // Error;
                // Fatal;
                3 or 4 or 5 => FilterResult.Log,
                // Off;
                6 => FilterResult.IgnoreFinal,
                _ => FilterResult.IgnoreFinal
            };

            return result;
        });
        
        var ruleColoredConsoleTarget = new LoggingRule("*", LogLevel.Trace, LogLevel.Fatal, coloredConsoleTarget) { Filters = { filter } };
        var ruleCassandraTarget = new LoggingRule("*", LogLevel.Trace, LogLevel.Fatal, cassandraTarget) { Filters = { filter } };
        var ruleBufferingTargetWrapper = new LoggingRule("*", LogLevel.Trace, LogLevel.Fatal, bufferingTargetWrapper) { Filters = { filter } };
        
        loggingConfiguration.AddRule(ruleColoredConsoleTarget);
        loggingConfiguration.AddRule(ruleCassandraTarget);
        loggingConfiguration.AddRule(ruleBufferingTargetWrapper);
        
        var logger =  NLog.LogManager
            .Setup()
            .LoadConfiguration(loggingConfiguration)
            .GetCurrentClassLogger();
        
        return Task.FromResult(true);
    }
    
    private static bool LoggerNameIsMicrosoft(string loggerName) 
        => loggerName.StartsWith("Microsoft.", StringComparison.Ordinal);
}