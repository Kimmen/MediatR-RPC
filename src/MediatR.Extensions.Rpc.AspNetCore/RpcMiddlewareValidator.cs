using Mediatr.Rpc;

namespace MediatR.Rpc.AspNetCore
{
    internal static class RpcMiddlewareValidator
    {
        public static void ValidateCaller(RpcCaller rpcCaller)
        {
            AssertHelper.ValidateIsNotNull(rpcCaller, nameof(rpcCaller));
        }

        public static void ValidateOptions(RpcEndpointOptions options)
        {
            AssertHelper.ValidateIsNotNull(options.Path, nameof(options.Path));
            AssertHelper.ValidateIsNotNull(options.SerializeResponse, nameof(options.SerializeResponse));
            AssertHelper.ValidateIsNotNull(options.DeserializeRequest, nameof(options.DeserializeRequest));
            AssertHelper.ValidateIsNotNull(options.HandlResponse, nameof(options.HandlResponse));
            AssertHelper.ValidateIsNotNull(options.UnmatchedRequest, nameof(options.UnmatchedRequest));

            AssertHelper.ValidateIsNotEmpty(options.Path, nameof(options.Path));
        }
    }
}