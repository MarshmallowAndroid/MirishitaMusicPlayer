using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace MirishitaMusicPlayer
{
    class PluginLoadContext : AssemblyLoadContext
    {
        private readonly List<Stream> assemblyStreams = new();

        private AssemblyDependencyResolver resolver;

        public PluginLoadContext(string path) : base(isCollectible: true)
        {
            resolver = new AssemblyDependencyResolver(path);
        }

        public void DisposeAllAssemblies()
        {
            assemblyStreams.ForEach(s => s.Dispose());
            assemblyStreams.Clear();
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath is not null)
            {
                FileStream assemblyStream = File.OpenRead(assemblyPath);
                assemblyStreams.Add(assemblyStream);
                return LoadFromStream(assemblyStream);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath is not null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }
}
