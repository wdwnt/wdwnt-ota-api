using Nancy;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace wdwnt_ota_api
{
    public class CachedResponse : Response
    {
        private readonly Response response;

        public CachedResponse(Response response)
        {
            this.response = response;

            ContentType = response.ContentType;
            Headers = response.Headers;
            StatusCode = response.StatusCode;
            Contents = response.Contents; //GetContents();
        }

        public override Task PreExecute(NancyContext context)
        {
            return response.PreExecute(context);
        }

        private Action<Stream> GetContents()
        {
            return stream =>
            {
                using (var memoryStream = new MemoryStream())
                {
                    response.Contents.Invoke(memoryStream);
                    var contents = Encoding.UTF8.GetString(memoryStream.GetBuffer());
                    //var contents = Encoding.ASCII.GetString(memoryStream.GetBuffer());
                    var writer = new StreamWriter(stream) { AutoFlush = true };
                    writer.Write(contents);
                }
            };
        }
    }
}