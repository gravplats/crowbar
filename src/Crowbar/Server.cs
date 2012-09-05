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

        public void Execute(Action<ServerContext> script)
        {
            proxy.Process(new SerializableDelegate<Action<ServerContext>>(script));
        }
    }
}