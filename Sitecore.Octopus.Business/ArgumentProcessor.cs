namespace Sitecore.Octopus.Business
{
    public class ArgumentProcessor
    {
        public ArgumentProcessor(string[] args)
        {
            SerilizationFolder = args[0];
            PackageDestinationFolder = args[1];
            CurrentCommitId = args[2];
        }

        public string PackageDestinationFolder { get; set; }
        public string SerilizationFolder { get; set; }
        public string CurrentCommitId { get; set; }
    }
}