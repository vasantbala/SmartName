using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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
            string cachedImage = MemCache.Instance.Get("bingImage") as string;

            if (string.IsNullOrEmpty(cachedImage))
            {
                string bingImage = await Utils.GetAsync("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1");
                dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(bingImage);
                string imgUrl = System.IO.Path.Join("https://www.bing.com", json.images[0].url);
                byte[] imgBytes = await Utils.GetBytesAsync(imgUrl);
                cachedImage = Convert.ToBase64String(imgBytes);
                MemCache.Instance.AddOrReplace("bingImage", cachedImage);
            }
            return cachedImage;
        }
    }
}