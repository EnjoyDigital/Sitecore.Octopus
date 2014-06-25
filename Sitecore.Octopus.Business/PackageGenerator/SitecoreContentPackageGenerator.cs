using Sitecore.Octopus.Business.Contracts;
using Sitecore.Update;

namespace Sitecore.Octopus.Business.PackageGenerator
{
    public class SitecoreContentPackageGenerator
    {
        private readonly ISitecoreSerilizationDiffGenerator _sitecoreSerilizationDiffGenerator;

        public SitecoreContentPackageGenerator(ISitecoreSerilizationDiffGenerator sitecoreSerilizationDiffGenerator)
        {
            _sitecoreSerilizationDiffGenerator = sitecoreSerilizationDiffGenerator;
        }

        public void CreateContentPackage(string sourcePath, string targetPath)
        {
            var commands = _sitecoreSerilizationDiffGenerator.GetDiffCommands(sourcePath, targetPath);
            var diff = new DiffInfo(commands, "Sitecore Courier Package", string.Empty,  string.Format("Diff between folders '{0}' and '{1}'", sourcePath, targetPath));

            var outputPath = string.Format("GeneratedContentPackage.update");

            Update.Engine.PackageGenerator.GeneratePackage(diff, string.Empty, outputPath);
        }
    }
}
