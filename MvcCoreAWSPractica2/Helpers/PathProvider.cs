using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MvcCoreAWSPractica2.Helpers
{
    public enum Folders
    {
        Images = 0,
    }
    public class PathProvider
    {
        IWebHostEnvironment environment;
        public PathProvider(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        public String MapPath(String filename, Folders folder)
        {
            String carpeta = "";
            if (folder == Folders.Images)
            {
                carpeta = "Images";
            }

            String path = Path.Combine(this.environment.WebRootPath, carpeta, filename);
            return path;
        }
    }
}
