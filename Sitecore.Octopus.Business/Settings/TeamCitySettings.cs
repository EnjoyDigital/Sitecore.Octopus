using System.Configuration;
using Sitecore.Octopus.Business.Contracts;

namespace Sitecore.Octopus.Business.Settings
{
    public class TeamCitySettings : ITeamCitySettings
    {
        public string Url
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("TeamCity.Url");
            }
        }

        public string Password
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("TeamCity.Password");
            }
        }

        public string ProjectId
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("TeamCity.ProjectId");
            }
        }

        public string SerilizationArtifactName
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("TeamCity.SerilizationArtifactName");
            }
        }

        public string Username
        {
            get
            {
                return ConfigurationManager.AppSettings.Get("TeamCity.Username");
            }
        }
    }
}