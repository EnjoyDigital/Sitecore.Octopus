﻿using System.Configuration;
using System.Linq;
using Octopus.Client;
using Sitecore.Octopus.Business.Contracts;

namespace Sitecore.Octopus.Business.Services
{
    public class OcopusDeployService : IOctopusDeployService
    {
        private readonly IOctopusDeploySettings _octopusDeploySettings;

        public OcopusDeployService(IOctopusDeploySettings octopusDeploySettings)
        {
            _octopusDeploySettings = octopusDeploySettings;
        }

        public string FindCurrentlyDeployedProductionVersionNumber(string projectName, string environmentName)
        {
            var endpoint = new OctopusServerEndpoint(_octopusDeploySettings.SevrerUrl, _octopusDeploySettings.ApiKey);
            var repository = new OctopusRepository(endpoint);

            string[] projectIdList = new string[1];
            string[] environments = new string[1];

            projectIdList[0] = repository.Projects.FindByName(projectName).Id;
            environments[0] = repository.Environments.FindByName(environmentName).Id;

            var productionDeployments = repository.Deployments.FindAll(projectIdList, environments);

            var releaseId = productionDeployments.Items.First().ReleaseId;
            var release = repository.Releases.FindOne(x => x.Id == releaseId);

            return release.Version;
        }
    }
}
