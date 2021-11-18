namespace PetStore.User.Api.Infrastructure;

public class ApiRegistry : ServiceRegistry
{
    public ApiRegistry()
    {
        Scan(x =>
        {
            x.AssemblyContainingType<Program>();
                
            x.WithDefaultConventions();

            x.LookForRegistries();
                
            x.ExcludeType<ApiRegistry>();
        });

        For<IConnectionFactory>().Use<ConnectionFactory>().Singleton();
    }
}