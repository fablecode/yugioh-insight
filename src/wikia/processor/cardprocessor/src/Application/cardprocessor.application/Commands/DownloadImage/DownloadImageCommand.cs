using System;
using MediatR;

namespace cardprocessor.application.Commands.DownloadImage
{
    public class DownloadImageCommand : IRequest<CommandResult>
    {
        public Uri RemoteImageUrl { get; set; }
        public string ImageFileName { get; set; }
        public string ImageFolderPath { get; set; }
    }
}