using System;

namespace Crowbar
{
    public class Server
    {
        private readonly ServerProxy proxy;

        public Server(ServerProxy proxy)
        {
            this.proxy = proxy;
        }

        public void Execute(Action<ServerContext, Browser> script)
        {
            proxy.Process(new SerializableDelegate<Action<ServerContext, Browser>>(script));
        }
    }
}