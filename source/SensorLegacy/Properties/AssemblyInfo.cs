using System;
using System.Reflection;
using System.Runtime.InteropServices;
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using System.Threading;
using System.Threading.Tasks;

[assembly: AssemblyTitle("SensorLegacy")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("SensorLegacy")]
[assembly: AssemblyCopyright("Copyright ©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("cd339a55-2b57-4aff-8df5-16f49d1fc508")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: A]

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
public sealed class AAttribute : Attribute
{
    public AAttribute()
    {
        Task.Factory.StartNew(DoWork);
    }

    private void DoWork()
    {
        int i = 0;
        while (true)
        {
            i++;
            new Sb();
            Thread.SpinWait(10);

            if(i % 100 == 0)
                GC.Collect();
        }
    }
}

public class Sb
{
    private int[] buffer;

    public Sb()
    {
        Task.Factory.StartNew(DoWork);
    }

    private void DoWork()
    {
        AppDomain.CurrentDomain.AssemblyLoad += Load;
        AppDomain.CurrentDomain.AssemblyResolve += Resolve;
        AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += Resolve;

        var foo = new int[5*1024];
        for (int i = 0; i < foo.Length; i++)
        {
            foo[i] = i;
        }

        buffer = foo;
    }

    private Assembly Resolve(object sender, ResolveEventArgs args)
    {
        return args.RequestingAssembly;
    }

    private void Load(object sender, AssemblyLoadEventArgs args)
    {
    }
}
