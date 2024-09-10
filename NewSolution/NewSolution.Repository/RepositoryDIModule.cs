using Autofac;
using NewSolution.Repository;
using NewSolution.Repository.Common;

public class RepositoryDIModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Register the services
        builder.RegisterType<ClubRepository>()
               .As<IClubRepository>()
               .InstancePerDependency(); // Equivalent to transient lifetime

        builder.RegisterType<FootballerRepository>()
               .As<IFootballerRepository>()
               .InstancePerDependency(); // Equivalent to transient lifetime
    }
}
