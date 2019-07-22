using System;

namespace archetypeprocessor.core.Models
{
    public class DownloadImage
    {
        public Uri RemoteImageUrl { get; set; }
        public string ImageFileName { get; set; }
        public string ImageFolderPath { get; set; }

    }
}