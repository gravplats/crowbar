using System.Threading.Tasks;
using Crowbar.Demo.Mvc.Async.Application.Models;

namespace Crowbar.Demo.Mvc.Async.Application
{
    public interface IExternalRequestAsync
    {
        Task<Response> Execute();
    }
}