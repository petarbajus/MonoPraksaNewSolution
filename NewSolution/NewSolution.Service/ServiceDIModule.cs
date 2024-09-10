using Autofac;
using NewSolution.Service;
using NewSolution.Service.Common;

public class ServiceDIModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register the services
        builder.RegisterType<ClubService>()
               .As<IClubService>()
               .InstancePerDependency(); // Equivalent to transient lifetime

        builder.RegisterType<FootballerService>()
               .As<IFootballerService>()
               .InstancePerDependency(); // Equivalent to transient lifetime
    }
}
