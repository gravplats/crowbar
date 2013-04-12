using System.Linq;
using Ninject;
using Ninject.Modules;

namespace Crowbar.Demo.Mvc.Raven.Tests
{
    public static class KernelExtensions
    {
        public static void Reload(this IKernel kernel, params INinjectModule[] modules)
        {
            foreach (string name in modules.Select(module => module.Name).Where(kernel.HasModule))
            {
                kernel.Unload(name);
            }

            kernel.Load(modules);
        }
    }
}