using System;
using System.Web;

namespace Crowbar
{
    public interface IMvcApplicationProxy
    {
        void Initialize(SerializableDelegate<Func<HttpApplication>> initialize, string directory);
    }
}