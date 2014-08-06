using Sitecore.Octopus.Business;
using Sitecore.Octopus.Business.Services;
using Sitecore.Octopus.Business.Settings;
using Sitecore.Octopus.Business.Stratergies;

namespace Sitecore.Octopus.ContentPackageGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var argumentProcessor = new ArgumentProcessor(args);
            var contentPackageGenerator = new Business.ContentPackageGenerator(new GitHubService(new GitSettings()), new OctopusDeployService(new OctopusDeploySettings()), new OctopusDeploySettings());
            var artifactDetails = contentPackageGenerator.CreatePackage(argumentProcessor.SerilizationFolder);
           
            var releaseNotesGenereator = new ReleaseNotesGenerator(new BasicOctopusToTeamcityMappingStratergy(), new OctopusDeployService(new OctopusDeploySettings()), new OctopusDeploySettings(), new BasicBuildIdToTagNameStratergy(), new GitHubService(new GitSettings()), new JiraService(new JiraSettings())  );
            var releaseNotesFilePath = releaseNotesGenereator.CreateReleaseNotes(argumentProcessor.CurrentCommitId);
            artifactDetails.ReleaseNotesFilePath = releaseNotesFilePath;

            ArtifactMover.Move(artifactDetails, argumentProcessor.PackageDestinationFolder);
        }
    } 
}
