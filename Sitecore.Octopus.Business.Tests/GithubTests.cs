using System;
using System.IO;
using Moq;
using NUnit.Framework;
using Sitecore.Octopus.Business.Contracts;
using Sitecore.Octopus.Business.Services;

namespace Sitecore.Octopus.Business.Tests
{
    [TestFixture]
    public class GithubTests
    {
        [Test]
        public void GithubService_GetCommitsBetweenTag_AndHead()
        {
            var tag = "V1.0";
            var commitId = "6e83241948a8f923b1cb2dcd8e5df4d074f2edc9";
            var settings = new Mock<IGitSettings>();
            settings.SetupGet(x => x.GithubToken).Returns("a147fac4a7d179deeee0e41f27581324a819c236");
            settings.SetupGet(x => x.RepositoryName).Returns("SampleSitecoreReposiotryForTesting");
            settings.SetupGet(x => x.OrganisationName).Returns("leethomascook");
            var service = new GitHubService(settings.Object);

            var commits = service.GetCommitsBetweenTag_AndCommit(tag, commitId);

            Assert.Greater(commits.Count, 0);
        }

        [Test]
        public void GithubService_CreateSerilizationArtifact()
        {
            var settings = new Mock<IGitSettings>();
            settings.SetupGet(x => x.GithubToken).Returns("a147fac4a7d179deeee0e41f27581324a819c236");
            settings.SetupGet(x => x.RepositoryName).Returns("SampleSitecoreReposiotryForTesting");
            settings.SetupGet(x => x.OrganisationName).Returns("leethomascook");
            var service = new GitHubService(settings.Object);

            service.CreateSerilizationAsset("v.0.0.1" + DateTime.Now.Ticks, "C:\\Projects\\Sitecore.Octopus\\Sitecore.Octopus.Business.Tests\\SerliazedItems\\Added"); 
        }

        [Test]
        public void GithubService_CreateSerilizationArtifact_ThrowsErrorIfGithubReturnsAnError()
        {
            Assert.Throws<Exception>(delegate { CallService(DateTime.Now.Ticks.ToString()); });
        }

        [Test]
        public void GithubService_CreateSerilizationArtifact_ThrowsErrorIfErrorUploadingReleaseArtifact()
        {
            Assert.Throws<Exception>(delegate { CallService(""); });
        }

        private void CallService(string tagName)
        {
            var settings = new Mock<IGitSettings>();
            settings.SetupGet(x => x.GithubToken).Returns("a147fac4d179deeee0e41f27581324a819c236");
            settings.SetupGet(x => x.RepositoryName).Returns("SampleSitecoreReposiotryForTesting");
            settings.SetupGet(x => x.OrganisationName).Returns("leethomascook");
            var service = new GitHubService(settings.Object);
            service.CreateSerilizationAsset("v0.0.1" + tagName, "C:\\Projects\\Sitecore.Octopus\\Sitecore.Octopus.Business.Tests\\SerliazedItems\\Added");
        }

        [Test]
        public void GithubService_DownloadSerilizationArtifact()
        {
            var settings = new Mock<IGitSettings>();
            settings.SetupGet(x => x.GithubToken).Returns("a147fac4a7d179deeee0e41f27581324a819c236");
            settings.SetupGet(x => x.RepositoryName).Returns("SampleSitecoreReposiotryForTesting");
            settings.SetupGet(x => x.OrganisationName).Returns("leethomascook");
            var service = new GitHubService(settings.Object);

            service.DownloadSerilizationAsset("v0.1");

            Assert.IsTrue(System.IO.File.Exists(Directory.GetCurrentDirectory() + "\\" + GitHubService.ZIP_FILE_NAME));
        }
    }
}
