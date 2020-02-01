using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace DTH_Upload.Controllers
{
    [Route("api/Streaming")]
    [ApiController]
    public class StreamingController : ControllerBase
    {

        private readonly string _targetFilePath;
        private readonly IFileProvider _fileProvider;
        public IFileInfo RemoveFile { get; private set; }

        public StreamingController(IConfiguration config, IFileProvider fileProvider)
        {
            // To save physical files to a path provided by configuration:
            _targetFilePath = config.GetValue<string>("StoredFilesPath");
            _fileProvider = fileProvider;
        }


        [HttpGet("{fileName}")]
        public IActionResult GetDownloadPhysical(string fileName)
        {
            var downloadFile = _fileProvider.GetFileInfo(fileName);

            return PhysicalFile(downloadFile.PhysicalPath, MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost("CreateFolder/{folderName}")]
        public IActionResult PostNewFolder(string folderName)
        {
            var filePath = _targetFilePath + "\\" + folderName;
            System.IO.Directory.CreateDirectory(filePath);

            return Ok();
        }

        // POST: api/Streaming
        [HttpPost("{folderName}")]
        public async Task<IActionResult> PostNewFile(string folderName)
        {
            var postedFile = Request.Form.Files[0];
            var filePath = _targetFilePath;

            if (folderName != "Root")
            {
                filePath = _targetFilePath + "\\" + folderName;
            }

            using (var targetStream = System.IO.File.Create(
                            Path.Combine(filePath, postedFile.FileName)))
            {
                await postedFile.CopyToAsync(targetStream);

            }

            return Ok();
        }


        // DELETE: api/ApiWithActions/5
        [HttpDelete("{folderName}/{fileName}")]
        public IActionResult Delete(string folderName, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest();
            }

            string filePath;
            if (folderName != "Root")
            {
                //This a file in a sub folder
                filePath = _targetFilePath + "\\" + folderName + "\\" + fileName;

                System.IO.File.Delete(filePath);
            }
            else
            {
                //this is a file or folder in root
                filePath = _targetFilePath + "\\" + fileName;
                RemoveFile = _fileProvider.GetFileInfo(fileName);

                if (RemoveFile.Exists)
                {
                    System.IO.File.Delete(filePath);
                }
                else
                {
                    System.IO.Directory.Delete(filePath, true);
                }
            }

            return Ok();
        }
    }
}
