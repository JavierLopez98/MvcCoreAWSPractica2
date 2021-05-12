using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MvcCoreAWSPractica2.Services
{
    public class ServiceAWSS3Files
    {
        private String bucketName;
        private IAmazonS3 awsClient;
        public ServiceAWSS3Files(IAmazonS3 awsClient,IConfiguration config)
        {
            this.awsClient = awsClient;
            this.bucketName = config["AWSS3:BucketName"];
        }
        public async Task UploadFile(IFormFile file,String nombre)
        {
            using (var client = new AmazonS3Client("AKIAXWIS2DQZPUYBOYQB", "O/1ioQaW7Mwcmnze3blWEV9bTcy5SrPWj4GAyRfP", RegionEndpoint.USEast1))
            {
                using (var newMemoryStream = new MemoryStream())
                {
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = nombre+file.FileName,
                        BucketName = "fotos-practica",
                        CannedACL = S3CannedACL.PublicRead
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                }
            }
        }
        public async Task<List<String>> GetS3Files(String nombre)
        {
            ListVersionsResponse response = await awsClient.ListVersionsAsync(this.bucketName);
            return response.Versions.Select(x => x.Key).Where(x=>x.StartsWith(nombre)).ToList();
        }
        public async Task<bool> DeleteFile(String filename)
        {
            DeleteObjectResponse response = await this.awsClient.DeleteObjectAsync(this.bucketName, filename);
            if (response.HttpStatusCode == HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<Stream> GetFile(String filename)
        {
            GetObjectResponse response = await this.awsClient.GetObjectAsync(this.bucketName, filename);
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                return response.ResponseStream;
            }else
            {
                return null;
            }
        }
    }
}
