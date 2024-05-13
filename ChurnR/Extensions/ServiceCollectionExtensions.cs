using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;
using ChurnR.Core.Reporter;
using ChurnR.Core.Support;
using ChurnR.Core.VcsAdapter;
using ChurnR.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ChurnR.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddChurnR(this IServiceCollection serviceCollection, Options gitOptions)
    {
        // Logger
        serviceCollection.AddSingleton(SerilogSetup.Setup());
        
        // engine
        serviceCollection.AddTransient<Engine>();
        
        // options
        serviceCollection.AddSingleton(gitOptions);
        
        // processor
        serviceCollection.AddTransient<MinimalCutoffProcessor>();
        serviceCollection.AddTransient<PercentCutoffProcessor>();
        serviceCollection.AddTransient<IProcessor>(provider =>
        {
            var options = provider.GetRequiredService<Options>();
            return float.TryParse(options.MinimalChurnRate, out _)
                ? provider.GetRequiredService<PercentCutoffProcessor>()
                : provider.GetRequiredService<MinimalCutoffProcessor>();
        });

        // reporter
        serviceCollection.AddTransient(provider =>
        {
            var options = provider.GetRequiredService<Options>();
            return options.Output == null 
                ? Console.Out 
                : new StreamWriter(options.Output);
        });
        serviceCollection.AddTransient<ChartJsReporter>();
        serviceCollection.AddTransient<CsvReporter>();
        serviceCollection.AddTransient<SimpleReporter>();
        serviceCollection.AddTransient<TableReporter>();
        serviceCollection.AddTransient<XmlReporter>();
        serviceCollection.AddTransient<IReporter>(provider =>
        {
            var options = provider.GetRequiredService<Options>();
            return options.Reporter switch
            {
                Reporter.table => provider.GetRequiredService<TableReporter>(),
                Reporter.xml => provider.GetRequiredService<XmlReporter>(),
                Reporter.csv => provider.GetRequiredService<CsvReporter>(),
                Reporter.chartjs => provider.GetRequiredService<ChartJsReporter>(),
                Reporter.simple => provider.GetRequiredService<SimpleReporter>(),
                _ => throw new ArgumentOutOfRangeException(nameof(options.Reporter), options.Reporter, null)
            };
        });

        // vcs adapter
        serviceCollection.AddTransient<GitAdapter>();
        serviceCollection.AddTransient<SvnAdapter>();
        serviceCollection.AddTransient<IVcsAdapter>(provider =>
        {
            var options = provider.GetRequiredService<Options>();
            var directory = options.ExecutionDirectory ?? "./";
            
            if (Path.Exists(Path.Combine(directory, ".git")))
            {
                provider.GetRequiredService<GitAdapter>();
            }
            else if (Path.Exists(Path.Combine(directory, ".svn")))
            {
                provider.GetRequiredService<SvnAdapter>();
            }

            var logger = provider.GetRequiredService<ILogger>();
            logger.Error("No VCS found in '{0}'", directory);
            throw new InvalidOperationException("No VCS found in '{0}'");
        });
        
        // analyzer
        serviceCollection.AddTransient<IAnalyzer, Analyzer>();
        
        // adapter source
        serviceCollection.AddTransient<IAdapterDataSource, AdapterDataSource>();
    } 
}