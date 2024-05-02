using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace BusTickitingApi.Models
{
    public class MultipartFormDataFormatter : MediaTypeFormatter
    {
        public MultipartFormDataFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(PurchaseRequest);
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var multipartData = await content.ReadAsMultipartAsync();
            var orderData = new PurchaseRequest();

            foreach (var contentPart in multipartData.Contents)
            {
                var fieldName = contentPart.Headers.ContentDisposition.Name.Trim('\"');

                if (fieldName == "Purchase")
                {
                    var orderContent = await contentPart.ReadAsStringAsync();
                    orderData.Purchase = JsonConvert.DeserializeObject<Purchase>(orderContent);
                }
                else if (fieldName == "ImageFile")
                {
                    orderData.ImageFile = await contentPart.ReadAsByteArrayAsync();
                    orderData.ImageFileName = contentPart.Headers.ContentDisposition.FileName;
                }
            }

            return orderData;
        }
    }
}