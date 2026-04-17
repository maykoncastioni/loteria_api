using Serilog.Filters;
using Serilog;
using Serilog.Exceptions;
using Serilog.Enrichers.Span;

namespace Loteria.API.Extension
{
    public static class SerilogExtension
    {
        public static void AddSerilogApi(IConfiguration configuration)
        {
            var logCfg = new LoggerConfiguration();

            var enviromentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            Boolean IsDevelopment = enviromentName == "Development";
            var configuracao = ObterConfiguracaoDeLogDoArquivoDeConfiguracao();

            logCfg = logCfg
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithMachineName()
                .Enrich.WithSpan()
                .Enrich.WithCorrelationId()
                .Enrich.WithProperty("ApplicationName", $"API Loteria - {enviromentName}")
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.StaticFiles"))
                .Filter.ByExcluding(z => z.MessageTemplate.Text.Contains("Business error"))
                .ReadFrom.Configuration(configuracao);


            try
            {
                //Loga no console em todos os ambientes (necessário para Railway/containers capturarem stdout)
                logCfg = logCfg.WriteTo.Async(
                    wt => wt.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}"));
            }
            catch { }


            Log.Logger = logCfg.CreateLogger();
        }

        public static IConfigurationRoot ObterConfiguracaoDeLogDoArquivoDeConfiguracao()
        {
            return new ConfigurationBuilder()
              .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
              .AddJsonFile(
                  path: $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                  optional: true)
              .Build();
        }
    }
}
