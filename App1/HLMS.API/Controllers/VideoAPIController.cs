using HLMS.API.Helpers;
using HLMS.API.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HLMS.API.Controllers
{
    public class VideoAPIController : ApiController
    {
        [MimeMultipart]
        public string Get()
        {
            return "Hello";
        }

        [MimeMultipart]
        public string Get(string keywords)
        {
            return "Hello";
        }

        [MimeMultipart]
        public async void Post()
        {
            // Read the MIME multipart asynchronously
            var stream = await Request.Content.ReadAsMultipartAsync();
            //Request.GetUrlHelper()
            foreach (HttpContent content in stream.Contents)
            {
                var filename = content.Headers.ContentDisposition.FileName;
                var filesize = content.Headers.ContentDisposition.Size;
            }
        }
    }
}