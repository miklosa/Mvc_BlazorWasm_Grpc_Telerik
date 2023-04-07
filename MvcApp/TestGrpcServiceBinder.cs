using ProtoBuf.Grpc.Configuration;
using System.Reflection;

internal sealed class TestGrpcServiceBinder : ServiceBinder
{
    private readonly IServiceCollection _services;

    public TestGrpcServiceBinder(IServiceCollection services)
    {
        _services = services;
    }

    public override IList<object> GetMetadata(MethodInfo method, Type contractType, Type serviceType)
    {
        var resolvedServiceType = serviceType;

        if (serviceType.IsInterface)
        {
            resolvedServiceType = _services.SingleOrDefault(x => x.ServiceType == serviceType)?.ImplementationType ?? serviceType;
        }

        return base.GetMetadata(method, contractType, resolvedServiceType);
    }
}