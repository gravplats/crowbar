using System.IO;
using System.Threading.Tasks;
using Crowbar.Demo.Mvc.Async.Application;
using Crowbar.Demo.Mvc.Async.Application.Models;
using Newtonsoft.Json;

namespace Crowbar.Demo.Mvc.Async.Tests
{
    public class ExternalRequestStub : IExternalRequestAsync
    {
        private readonly string testBaseDirectory;

        public ExternalRequestStub(string testBaseDirectory)
        {
            this.testBaseDirectory = testBaseDirectory;
        }

        public async Task<Response> Execute()
        {
            return await Task.Run(() =>
            {
                string path = Path.Combine(testBaseDirectory, "request.json");

                using (var stream = new FileStream(path, FileMode.Open))
                using (var reader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(reader))
                    {
                        var serializer = new JsonSerializer();
                        return serializer.Deserialize<Response>(jsonTextReader);
                    }
                }
            });
        }
    }
}