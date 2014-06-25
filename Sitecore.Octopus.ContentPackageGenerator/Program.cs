using Sitecore.Octopus.Business.PackageGenerator;
using Sitecore.Octopus.Business.Services;
using Sitecore.Octopus.Business.Settings;

namespace Sitecore.Octopus.ContentPackageGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Step 1. Get Current Production Release Number from OD
            var octopusSettings = new OctopusDeploySettings();
            var octopusDeployService = new OctopusDeployService(octopusSettings);
            octopusDeployService.FindCurrentlyDeployedProductionVersionNumber(octopusSettings.ProjectName, octopusSettings.EnvironmentName);

            //Step 2. Get Build Number From TC
            var teamCityService = new TeamCityService();

            //Step 3. Get Serlization folder that you have stored as an artifact
            var sourcePath = "";
            var targetPath = "";

            //Step 4. Generate content package via Diff based on the old serlization  compared to new one (Courier!)

            var packageGenerator = new SitecoreContentPackageGenerator(new SitecoreSerilizationDiffGenerator());
            packageGenerator.CreateContentPackage(sourcePath, targetPath);

            //Step 5. Generate ItemsToPublish.json for Sitecore.Ship
        }
    }
}
