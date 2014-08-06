using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using Octokit;
using RestSharp;
using Sitecore.Octopus.Business.Contracts;
using Commit = Sitecore.Octopus.Business.Domain.Commit;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace Sitecore.Octopus.Business.Services
{
    public class GitHubService : ISourceControlService
    {
        private readonly IGitSettings _settings;
        public const string ZIP_FILE_NAME = "Serilization.zip";

        public GitHubService(IGitSettings settings)
        {
            _settings = settings;
        }

        public List<Commit>  GetCommitsBetweenTag_AndCommit(string tagName, string commitId)
        {
            var credentials = new Credentials(_settings.GithubToken);
            var connection = new Connection(new ProductHeaderValue(_settings.RepositoryName))
            {
                Credentials = credentials
            };

            var client = new GitHubClient(connection);
           
            var results = client.Repository.Commits.Compare(_settings.OrganisationName, _settings.RepositoryName, tagName, commitId);

            return results.GetAwaiter().GetResult().Commits.Select(commit => new Commit()
            {
                Message = commit.Commit.Message, Authour = commit.Author.Login, CommitId = commit.Commit.Sha
            }).ToList();
        }

        public string DownloadSerilizationAsset(string tagName)
        {
            var requestUrl = string.Format("/{0}/{1}/releases/download/{2}/{3}", _settings.OrganisationName, _settings.RepositoryName, tagName, ZIP_FILE_NAME);
            string tempFile = Directory.GetCurrentDirectory() + "\\" + ZIP_FILE_NAME;
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
            using (var writer = File.OpenWrite(tempFile))
            {
                var client = new RestClient("https://github.com");
                var request = new RestRequest(string.Format("{0}?access_token={1}", requestUrl, _settings.GithubToken), Method.GET)
                {
                    ResponseWriter = (responseStream) => responseStream.CopyTo(writer)
                };

                client.DownloadData(request);
            }

            return tempFile;
        }

        public void CreateSerilizationAsset(string tagName, string folderPath)
        {
            try
            {
                var credentials = new Credentials(_settings.GithubToken);
                var connection = new Connection(new ProductHeaderValue(_settings.RepositoryName))
                {
                    Credentials = credentials
                };

                var client = new GitHubClient(connection);
                var release = new ReleaseUpdate(tagName);
                var results =
                    client.Release.Create(_settings.OrganisationName, _settings.RepositoryName, release)
                        .GetAwaiter()
                        .GetResult();

                File.Delete(ZIP_FILE_NAME);
               

                ZipFile.CreateFromDirectory(folderPath, ZIP_FILE_NAME, CompressionLevel.Fastest, true);

                UploadBinaryAssetToRelease(results.UploadUrl, ZIP_FILE_NAME);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void UploadBinaryAssetToRelease(string uploadUrl, string zip)
        {
            var restClient = new RestClient("https://uploads.github.com");
            var requestUrl = uploadUrl.Substring(26);//deducting base address e.g https://uploads.github.com
            var request = new RestRequest(requestUrl.Replace("{?name}", string.Format("?name={0}&access_token={1}", ZIP_FILE_NAME, _settings.GithubToken)), Method.POST)
            {
                AlwaysMultipartFormData = false
            };

            request.AddHeader("Content-Type", "application/zip");
            request.AddFile(zip, string.Concat(Directory.GetCurrentDirectory(), "\\", zip));
            restClient.Execute(request);
        }
    }
}
