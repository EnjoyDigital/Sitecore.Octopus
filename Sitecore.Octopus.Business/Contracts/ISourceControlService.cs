﻿using System.Collections.Generic;
using Sitecore.Octopus.Business.Domain;

namespace Sitecore.Octopus.Business.Contracts
{
    public interface ISourceControlService
    {
        List<Commit> GetCommitsBetweenTag_AndCommit(string tagName, string commitId);
        string DownloadSerilizationAsset(string tagName);
        void CreateSerilizationAsset(string tagName, string folderPath);
    }
}
