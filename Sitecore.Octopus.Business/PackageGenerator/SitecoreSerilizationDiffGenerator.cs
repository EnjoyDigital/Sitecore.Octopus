using System.Collections.Generic;
using Sitecore.Octopus.Business.Contracts;
using Sitecore.Update.Configuration;
using Sitecore.Update.Data;
using Sitecore.Update.Interfaces;

namespace Sitecore.Octopus.Business.PackageGenerator
{
    /* This is solten from sitecore Courier.  */
    public class SitecoreSerilizationDiffGenerator : ISitecoreSerilizationDiffGenerator
    {
        public List<ICommand> GetDiffCommands(string sourcePath, string targetPath)
        {
            var targetManager = Factory.Instance.GetSourceDataManager();
            var sourceManager = Factory.Instance.GetTargetDataManager();

            sourceManager.SerializationPath = sourcePath;
            targetManager.SerializationPath = targetPath;
            IDataIterator sourceDataIterator = sourceManager.ItemIterator;
            IDataIterator targetDataIterator = targetManager.ItemIterator;

            var engine = new DataEngine();

            var commands = new List<ICommand>();
            commands.AddRange(GenerateDiff(sourceDataIterator, targetDataIterator));
            engine.ProcessCommands(ref commands);
            return commands;
        }

        private IEnumerable<ICommand> GenerateDiff(IDataIterator sourceIterator, IDataIterator targetIterator)
        {
            var commands = new List<ICommand>();
            IDataItem sourceDataItem = sourceIterator.Next();
            IDataItem targetDataItem = targetIterator.Next();

            while (sourceDataItem != null || targetDataItem != null)
            {
                int compareResult = Compare(sourceDataItem, targetDataItem);
                commands.AddRange((sourceDataItem ?? targetDataItem).GenerateDiff(sourceDataItem, targetDataItem, compareResult));
                if (compareResult < 0)
                {
                    sourceDataItem = sourceIterator.Next();
                }
                else
                {
                    if (compareResult > 0)
                    {
                        targetDataItem = targetIterator.Next();
                    }
                    else
                    {
                        if (compareResult == 0)
                        {
                            targetDataItem = targetIterator.Next();
                            sourceDataItem = sourceIterator.Next();
                        }
                    }
                }
            }

            return commands;
        }

        private int Compare(IDataItem sourceItem, IDataItem targetItem)
        {
            if (sourceItem == null && targetItem == null)
            {
                return 0;
            }

            if (sourceItem == null)
            {
                return 1;
            }

            if (targetItem == null)
            {
                return -1;
            }

            return sourceItem.CompareTo(targetItem);
        }
    }
}
