using System;
using System.Threading.Tasks;
using Ensek.Domain;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Ensek.Serverless.Functions
{
    public class Validate
    {
        private readonly IDataImpoterFactory _factory;

        public Validate(IDataImpoterFactory factory)
        {
            _factory = factory;

        }

        [FunctionName("Validate")]
        public async Task RunAsync([TimerTrigger("*/30 * * * * *"
             #if DEBUG
            , RunOnStartup= true
            #endif
            )
            ] TimerInfo myTimer, 
            ILogger log)
        {
            log.LogInformation($"{nameof(Validate)} function executed at: {DateTime.Now}");
            var dataImporters = await _factory.BuildAll(DataImporterStatus.Loaded);

            foreach (var dataImporter in dataImporters)
            {
                var (ItemsRead, ItemsAccepted) = await dataImporter.Validate();
                log.LogInformation($"dataImporter {dataImporter.Id} {ItemsRead}/{ItemsAccepted}");
            }
        }
    }
}
