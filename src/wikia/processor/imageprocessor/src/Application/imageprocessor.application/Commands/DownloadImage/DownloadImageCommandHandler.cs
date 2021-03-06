﻿using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using imageprocessor.core.Helpers;
using imageprocessor.core.Services;
using MediatR;
using MimeTypes;

namespace imageprocessor.application.Commands.DownloadImage
{
    public class DownloadImageCommandHandler : IRequestHandler<DownloadImageCommand, CommandResult>
    {
        private readonly IFileSystemService _fileSystemService;
        private readonly IValidator<DownloadImageCommand> _validator;

        public DownloadImageCommandHandler(IFileSystemService fileSystemService, IValidator<DownloadImageCommand> validator)
        {
            _fileSystemService = fileSystemService;
            _validator = validator;
        }

        public async Task<CommandResult> Handle(DownloadImageCommand request, CancellationToken cancellationToken)
        {
            var commandResult = new CommandResult();

            var validationResult = _validator.Validate(request);

            if (validationResult.IsValid)
            {
                request.ImageFileName = request.ImageFileName.SanitizeFileName();

                var imageFileFullPathWithoutExtension = Path.Combine(request.ImageFolderPath, Path.GetFileNameWithoutExtension(request.ImageFileName));

                var downloadedFileResult = await _fileSystemService.Download(request.RemoteImageUrl, imageFileFullPathWithoutExtension);

                var imageFileFullPathWithExtension = string.Concat(imageFileFullPathWithoutExtension, GetDefaultExtension(downloadedFileResult.ContentType));

                if (_fileSystemService.Exists(imageFileFullPathWithExtension))
                    _fileSystemService.Delete(imageFileFullPathWithExtension);

                _fileSystemService.Rename(imageFileFullPathWithoutExtension, imageFileFullPathWithExtension);

                commandResult.Data = downloadedFileResult;
                commandResult.IsSuccessful = true;
            }
            else
            {
                commandResult.Errors = validationResult.Errors.Select(err => err.ErrorMessage).ToList();
            }

            return commandResult;
        }

        private static string GetDefaultExtension(string mimeType)
        {
            return MimeTypeMap.GetExtension(mimeType);
        }
    }
}