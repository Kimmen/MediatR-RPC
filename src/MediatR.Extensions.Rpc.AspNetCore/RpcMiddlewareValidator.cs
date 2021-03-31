namespace MediatR.Rpc.AspNetCore
{
    internal static class RpcMiddlewareValidator
    {
        internal static void ValidateCaller(IRpcRequestRunner rpcRunner)
        {
            AssertHelper.ValidateIsNotNull(rpcRunner, nameof(rpcRunner));
        }

        internal static void ValidateOptions(RpcEndpointOptions options)
        {
            AssertHelper.ValidateIsNotNull(options.Path, nameof(options.Path));
            AssertHelper.ValidateIsNotNull(options.SerializeResponse, nameof(options.SerializeResponse));
            AssertHelper.ValidateIsNotNull(options.DeserializeRequest, nameof(options.DeserializeRequest));

            AssertHelper.ValidateIsNotEmpty(options.Path, nameof(options.Path));
        }
    }
}