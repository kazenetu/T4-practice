using System.Reflection;
using System.Runtime.Loader;

namespace LoadAssembly
{
  public class MyAssemblyLoadContext : AssemblyLoadContext
  {
    public MyAssemblyLoadContext() : base(isCollectible: true)
    {
    }

    protected override Assembly Load(AssemblyName name)
    {
      return null;
    }
  }
}
