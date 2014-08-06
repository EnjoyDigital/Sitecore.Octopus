using System.IO;
using System.IO.Compression;
using Sitecore.Octopus.Business.Contracts;
using Sitecore.Octopus.Business.Domain;
using Sitecore.Octopus.Business.PackageGenerator;
using Sitecore.Octopus.Business.Stratergies;

namespace Sitecore.Octopus.Business
{
    public class ContentPackageGenerator
    {
        private readonly ISourceControlService _sourceControlService;
        private readonly IOctopusDeployService _octopusDeployService;
        private readonly IOctopusDeploySettings _octopusDeploySettings;

        public ContentPackageGenerator(ISourceControlService sourceControlService, IOctopusDeployService octopusDeployService, IOctopusDeploySettings octopusDeploySettings)
        {
            _sourceControlService = sourceControlService;
            _octopusDeployService = octopusDeployService;
            _octopusDeploySettings = octopusDeploySettings;
        }

        public ArtifactDetails CreatePackage(string currentSerilizationFolder)
        {
            //Step 1. Get Current Production Release Number from OD
            var version = _octopusDeployService.FindCurrentlyDeployedProductionVersion(_octopusDeploySettings.ProjectName, _octopusDeploySettings.EnvironmentName);

            //Step 2. Get Production Build Number From TC
            var buildNumber = new BasicOctopusToTeamcityMappingStratergy().GetTeamCityBuildNumberFromOctopusReleaseNumber(version.VersionNumber);

            //Step 3. Get Serlization folder that you have stored as an artifact
            var sourceZip =_sourceControlService.DownloadSerilizationAsset("v" + buildNumber);
            var sourcePath = Directory.GetCurrentDirectory() + "\\ExtractedZip";

            if (Directory.Exists(sourcePath))
            {
                Directory.Delete(sourcePath, true);
            }

            Directory.CreateDirectory(sourcePath);
         
            ZipFile.ExtractToDirectory(sourceZip, sourcePath);

            //Step 4. Generate content package via Diff based on the old serlization  compared to new one (Courier!)
            //Step 5. Generate ItemsToPublish.json for Sitecore.Ship

            var packageGenerator = new SitecoreContentPackageGenerator(new SitecoreSerilizationDiffGenerator());
            var artifactDetails = packageGenerator.CreateArtifacts(sourcePath, currentSerilizationFolder);

            return artifactDetails;
        }
    }
}
