using System;
using System.Threading.Tasks;
using Ensek.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Ensek.Serverless.Functions
{
    public class Import
    {
        private readonly IDataImpoterFactory _factory;

        public Import(IDataImpoterFactory factory)
        {
            _factory = factory;

        }

        [FunctionName("Import")]
        public async Task RunAsync(
            [TimerTrigger("0 */5 * * * *"
            #if DEBUG
            , RunOnStartup= true
            #endif
            )]
        TimerInfo myTimer,
            ILogger log)
        {
            log.LogInformation($"{nameof(Import)} function executed at: {DateTime.Now}");

            var dataImporters = await _factory.BuildAll(DataImporterStatus.Validated);

            foreach (var dataImporter in dataImporters)
            {                
                var (ItemsRead, ItemsAccepted) = await dataImporter.Import();
                log.LogInformation($"dataImporter {dataImporter.Id} {ItemsRead}/{ItemsAccepted}");
            }
        }
    }
}
