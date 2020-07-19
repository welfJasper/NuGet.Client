using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceHub.Framework;
using Microsoft.Test.Apex.VisualStudio;
using Microsoft.VisualStudio.Shell.ServiceBroker;
using Newtonsoft.Json;
using NuGet.VisualStudio.Contracts;

namespace NuGet.Tests.Apex.Apex
{
    [Export(typeof(NuGetProjectServiceService))]
    public class NuGetProjectServiceService : VisualStudioTestService<NuGetApexVerifier>
    {
        public async Task<string> GetInstalledPackagesAsync(Guid project)
        {
            var brokeredServiceContainer = VisualStudioObjectProviders.GetService<SVsBrokeredServiceContainer, IBrokeredServiceContainer>();
            IServiceBroker serviceBroker = brokeredServiceContainer.GetFullAccessServiceBroker();

            INuGetProjectService nuGetProjectService = await serviceBroker.GetProxyAsync<INuGetProjectService>(NuGetServices.NuGetProjectServiceV1);
            using (nuGetProjectService as IDisposable)
            using (var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
            {
                InstalledPackagesResult result = await nuGetProjectService.GetInstalledPackagesAsync(project, cancellationTokenSource.Token);
                var json = JsonConvert.SerializeObject(result);
                return json;
            }
        }
    }
}
