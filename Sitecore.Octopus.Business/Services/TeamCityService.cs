using System.Linq;
using Sitecore.Octopus.Business.Contracts;
using TeamCitySharp;
using TeamCitySharp.Locators;

namespace Sitecore.Octopus.Business.Services
{
    public class TeamCityService
    {
        private readonly ITeamCitySettings _teamCitySettings;
        private ITeamCityClient _client;

        public TeamCityService(ITeamCitySettings teamCitySettings)
        {
            _teamCitySettings = teamCitySettings;
            _client = new TeamCityClient(teamCitySettings.Url);
            _client.Connect(teamCitySettings.Username, teamCitySettings.Password);
        }

        public void DownloadSerlizationArtifact(string buildNumber)
        {
            var buildLocator = new BuildLocator();

          //  _client.Artifacts.DownloadArtifactsByBuildId(buildNumber, Download);
          
        }
    }
}
