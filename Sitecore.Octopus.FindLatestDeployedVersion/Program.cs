using System;
using System.Configuration;
using System.Linq;
using Octopus.Client;

namespace Sitecore.Octopus.FindLatestDeployedVersion
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = ConfigurationManager.AppSettings.Get("OctopusDeploy.Url");
            var apiKey = ConfigurationManager.AppSettings.Get("OctopusDeploy.ApiKey"); 

            var endpoint = new OctopusServerEndpoint(server, apiKey);
            var repository = new OctopusRepository(endpoint);

            string[] projectIdList = new string[1];
            string[] environments = new string[1];

            var projectName = args[0];
            var environmentName = args[1];

            projectIdList[0] = repository.Projects.FindByName(projectName).Id;
            environments[0] = repository.Environments.FindByName(environmentName).Id;

            var productionDeployments = repository.Deployments.FindAll(projectIdList, environments);

            var releaseId = productionDeployments.Items.First().ReleaseId;
            var release = repository.Releases.FindOne(x => x.Id == releaseId);

            Console.Write(release.Version);
        }
    }
}
