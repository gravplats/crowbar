using Newtonsoft.Json;

namespace Crowbar.Demo.Mvc.Async.Application.Models
{
    public class Response
    {
        [JsonProperty("demo")]
        public string Demo { get; set; }
    }
}