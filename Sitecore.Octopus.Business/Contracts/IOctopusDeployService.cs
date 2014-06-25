namespace Sitecore.Octopus.Business.Contracts
{
    public interface IOctopusDeployService
    {
        string FindCurrentlyDeployedProductionVersionNumber(string projectName, string environmentName);
    }
}
