using System.Collections.Generic;
using System.IO;
using System.Linq;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    public class DirectoryBrowser
    {
        /// <summary>
        /// Get all the files from specified  path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<FileBrowserEntry> GetContent(string path, string filter)
        {
            return GetFiles(path, filter).Concat(GetDirectories(path));
        }

        /// <summary>
        /// Get all the files from specified  path
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        private IEnumerable<FileBrowserEntry> GetFiles(string path, string filter)
        {
            var directory = new DirectoryInfo(path);

            var extensions = (filter ?? "*").Split(",|;".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);

            return extensions.SelectMany(directory.GetFiles)
                .Select(file => new FileBrowserEntry
                {
                    Name = file.Name,
                    Type = EntryType.File
                });
        }

        /// <summary>
        /// Get all the directories from specified  path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private IEnumerable<FileBrowserEntry> GetDirectories(string path)
        {
            var directory = new DirectoryInfo(path);

            return directory.GetDirectories()
                .Select(subDirectory => new FileBrowserEntry
                {
                    Name = subDirectory.Name,
                    Type = EntryType.Directory
                });
        }

    }
}