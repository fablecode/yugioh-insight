using System;
using System.Threading.Tasks;
using cardprocessor.core.Models;
using cardprocessor.core.Services;
using cardprocessor.domain.SystemIO;

namespace cardprocessor.domain.Services
{
    public class FileSystemService : IFileSystemService
    {
        private readonly IFileSystem _fileSystem;

        public FileSystemService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public Task<DownloadedFile> Download(Uri remoteFileUrl, string localFileFullPath)
        {
            return _fileSystem.Download(remoteFileUrl, localFileFullPath);
        }

        public void Delete(string localFileFullPath)
        {
            _fileSystem.Delete(localFileFullPath);
        }

        public void Rename(string oldNameFullPath, string newNameFullPath)
        {
            _fileSystem.Rename(oldNameFullPath, newNameFullPath);
        }

        public string[] GetFiles(string path, string searchPattern)
        {
            return _fileSystem.GetFiles(path, searchPattern);
        }

        public bool Exists(string localFileFullPath)
        {
            return _fileSystem.Exists(localFileFullPath);
        }
    }
}