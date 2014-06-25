namespace Sitecore.Octopus.Business.Contracts
{
    public interface ITeamCitySettings
    {
        string Url { get; }
        string Username { get; }
        string Password { get; }
        string ProjectId { get; }
        string SerilizationArtifactName { get; }
    }
}
