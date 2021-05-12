using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSPractica2.Helpers
{
    public class UploadHelper
    {
        PathProvider pathProvider;
        public UploadHelper(PathProvider pathprovider)
        {
            this.pathProvider = pathprovider;
        }
        public async Task<String> UploadFileAsync(IFormFile File,Folders folder)
        {
            String filename = File.FileName;
            String path = this.pathProvider.MapPath(filename, Folders.Images);
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await File.CopyToAsync(stream);
            }
            return path;
        }
    }
}
