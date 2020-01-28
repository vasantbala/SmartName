using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartNamePlate.Web.Common;

namespace SmartNamePlate.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [Route("GetImageOfTheDay")]
        public async Task<string> GetImageOfTheDay()
        {
            string filePath = "bingImage.jpg";
			string bingImagePath = ".\\wwwroot\\images\\bingImage.jpg";
			byte[] imgBytes = await System.IO.File.ReadAllBytesAsync(".\\wwwroot\\images\\defaultImage.jpg");

			try
            {
				string bingImage = await Utils.GetAsync("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1");
				dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(bingImage);
				string imgUrl = System.IO.Path.Join("https://www.bing.com", json.images[0].url);
				imgBytes = await Utils.GetBytesAsync(imgUrl);

				//System.IO.FileInfo fileInfo = new System.IO.FileInfo(bingImagePath);
				
    //            if (System.IO.File.Exists(bingImagePath) == false
				//	|| fileInfo.CreationTime.Date < DateTime.Now.Date)
    //            {
				//	if (System.IO.File.Exists(bingImagePath)) 
				//	{
				//		System.IO.File.Delete(bingImagePath);
				//	}

    //                string bingImage = await Utils.GetAsync("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1");
    //                dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(bingImage);
    //                string imgUrl = System.IO.Path.Join("https://www.bing.com", json.images[0].url);
    //                imgBytes = await Utils.GetBytesAsync(imgUrl);
				//	//await System.IO.File.WriteAllBytesAsync(bingImagePath, imgBytes);
    //                //System.IO.File.SetLastWriteTime(bingImagePath, DateTime.Now.AddDays(-1));
    //            }
            }
            catch(Exception ex)
            {
                filePath = "defaultImage.jpg";
				Console.WriteLine(ex.Message);
				Console.WriteLine(ex.StackTrace);
			}
            return Convert.ToBase64String(imgBytes);
        }

        [Route("GetBingImageDetails")]
        public async Task<string> GetBingImageDetails()
        {
            string returnValue = string.Empty;
            string bingImagePath = ".\\wwwroot\\images\\bingImage.jpg";
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(bingImagePath);
                stringBuilder.Append(string.Format("Name: {0}", fileInfo.Name));
                stringBuilder.Append(string.Format("Full Name: {0}", fileInfo.FullName));
                stringBuilder.Append(string.Format("LastWriteTime.Date: {0}", fileInfo.LastWriteTime.Date));
                stringBuilder.Append(string.Format("DateTime.Now.Date: {0}", DateTime.Now.Date));
                stringBuilder.Append(string.Format("CreationTime.Date: {0}", fileInfo.CreationTime.Date));
                returnValue = stringBuilder.ToString();
            }
            catch (Exception ex)
            {
                returnValue = ex.Message + ex.StackTrace;
            }
            return returnValue;
        }
    }
}