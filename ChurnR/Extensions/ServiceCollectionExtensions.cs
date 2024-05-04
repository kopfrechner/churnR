﻿using ChurnR.Core;
using ChurnR.Core.Analyzer;
using ChurnR.Core.CutoffProcessor;
using ChurnR.Core.Reporter;
using ChurnR.Core.Support;
using ChurnR.Core.VcsAdapter;
using ChurnR.Options;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace ChurnR.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddChurnR(this IServiceCollection serviceCollection, OptionsBase gitOptions)
    {
        // Logger
        serviceCollection.AddSingleton(Log.Logger = new LoggerConfiguration()
            // add console as logging target
            .WriteTo.Console()
            // set default minimum level
            .MinimumLevel.Debug()
            .CreateLogger());
        
        // engine
        serviceCollection.AddTransient<Engine>();
        
        // options
        serviceCollection.AddSingleton(gitOptions);
        
        // processor
        serviceCollection.AddTransient<IProcessor>(provider =>
        {
            var options = provider.GetRequiredService<OptionsBase>();
            return float.TryParse(options.MinimalChurnRate, out var minChurnPercent)
                ? new PercentCutoffProcessor(minChurnPercent)
                : int.TryParse(options.MinimalChurnRate, out var minChurn)
                    ? new MinimalCutoffProcessor(minChurn)
                    : new MinimalCutoffProcessor(0);
        });

        // reporter
        serviceCollection.AddTransient(_ => Console.Out);
        serviceCollection.AddTransient<ChartJsReporter>();
        serviceCollection.AddTransient<CsvReporter>();
        serviceCollection.AddTransient<SimpleReporter>();
        serviceCollection.AddTransient<TableReporter>();
        serviceCollection.AddTransient<XmlReporter>();
        serviceCollection.AddTransient<IReporter>(provider =>
        {
            var options = provider.GetRequiredService<OptionsBase>();
            return options.Reporter switch
            {
                Reporter.Table => provider.GetRequiredService<TableReporter>(),
                Reporter.Xml => provider.GetRequiredService<XmlReporter>(),
                Reporter.Csv => provider.GetRequiredService<CsvReporter>(),
                Reporter.ChartJs => provider.GetRequiredService<ChartJsReporter>(),
                Reporter.Simple => provider.GetRequiredService<SimpleReporter>(),
                _ => throw new ArgumentOutOfRangeException(nameof(options.Reporter), options.Reporter, null)
            };
        });

        // vcs adapter
        serviceCollection.AddTransient<GitAdapter>();
        serviceCollection.AddTransient<SvnAdapter>();
        serviceCollection.AddTransient<IVcsAdapter>(provider =>
        {
            var options = provider.GetRequiredService<OptionsBase>();

            return options.TargetVcs switch
            {
                Vcs.Git => provider.GetRequiredService<GitAdapter>(),
                Vcs.Svn => provider.GetRequiredService<SvnAdapter>(),
                _ => throw new ArgumentOutOfRangeException(nameof(options.TargetVcs), options.TargetVcs, null)
            };
        });
        
        // analyzer
        serviceCollection.AddTransient<IAnalyzer, Analyzer>();
        
        // adapter source
        serviceCollection.AddTransient<IAdapterDataSource, AdapterDataSource>();
    } 
}