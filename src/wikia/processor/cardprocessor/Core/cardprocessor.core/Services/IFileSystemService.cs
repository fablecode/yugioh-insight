using System;
using System.Threading.Tasks;
using cardprocessor.core.Models;

namespace cardprocessor.core.Services
{
    public interface IFileSystemService
    {
        Task<DownloadedFile> Download(Uri remoteFileUrl, string localFileFullPath);
        void Delete(string localFileFullPath);
        void Rename(string oldNameFullPath, string newNameFullPath);
        string[] GetFiles(string path, string searchPattern);
        bool Exists(string localFileFullPath);
    }
}