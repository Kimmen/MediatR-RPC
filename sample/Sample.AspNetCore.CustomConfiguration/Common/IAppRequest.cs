using MediatR;

namespace Sample.AspNetCore.CustomConfiguration.Handlers
{
    public interface IAppRequest : IRequest<CommonAppResponse>
    {

    }
}
