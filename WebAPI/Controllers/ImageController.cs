using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace WebAPI.Controllers
{
    public class ImageController : ApiController
    {
        [HttpPost]
        [Route("API/UploadImage")]
        public HttpResponseMessage UploadImage()
        {
            string imageName = null;
            var httpRequest = HttpContext.Current.Request;
            //Upload Image
            var postedFile = httpRequest.Files["Image"];
            //Create custom filename
            imageName = new string(Path.GetFileNameWithoutExtension(postedFile.FileName).ToArray());
            string devicesn = imageName.Split('_')[0].ToString();
            var filePath = HttpContext.Current.Server.MapPath("~/Images/"+ devicesn);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            imageName = imageName.Split('_')[1].ToString() + Path.GetExtension(postedFile.FileName);
            var file = filePath + "/"+ imageName;
            postedFile.SaveAs(file);
            //Save to DB; V2
            return Request.CreateResponse(HttpStatusCode.Created);
        }


    }
}
