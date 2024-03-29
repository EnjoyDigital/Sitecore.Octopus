﻿using NUnit.Framework;
using Sitecore.Octopus.Business.Services;
using Sitecore.Octopus.Business.Settings;

namespace Sitecore.Octopus.Business.Tests
{
    [TestFixture]
    public class SitecoreContentPackageGeneratorTests
    {
        [Test]
        public void ContentPackageGenerator_CreatesContentPackage()
        {
            var generator = new ContentPackageGenerator(new GitHubService(new GitSettings()), new OctopusDeployService(new OctopusDeploySettings()), new OctopusDeploySettings());
            var artifactDetails = generator.CreatePackage("C:\\Projects\\Sitecore.Octopus\\data\\currentserialization");

            Assert.AreEqual("GeneratedContentPackage.update", artifactDetails.ContentPackageFilePath);
            Assert.IsTrue(System.IO.File.Exists("GeneratedContentPackage.update"));
        }

        [Test]
        public void ContentPackageGenerator_CreatesItemToPublishJsonFile()
        {
            var generator = new ContentPackageGenerator(new GitHubService(new GitSettings()), new OctopusDeployService(new OctopusDeploySettings()), new OctopusDeploySettings());
            var artifactDetails = generator.CreatePackage("C:\\Projects\\Sitecore.Octopus\\data\\currentserialization");

            Assert.AreEqual("ItemsToPublish.json", artifactDetails.ItemsToPublishFilePath);
            Assert.IsTrue(System.IO.File.Exists("ItemsToPublish.json"));
        }
    }
}
