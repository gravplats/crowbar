using System.Threading.Tasks;
using Crowbar.Demo.Mvc.Async.Application;
using Crowbar.Demo.Mvc.Async.Application.Models;

namespace Crowbar.Demo.Mvc.Async.Tests
{
    public class AppProxyExternalRequestAsync : IExternalRequestAsync
    {
        public async Task<ResponseContainer> Execute()
        {
            return await Task.Run(() => new ResponseContainer());
        }
    }
}