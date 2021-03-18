using Mediatr.Rpc;

namespace MediatR.Rpc.AspNetCore
{
    /// <summary>
    /// Holds helpers validation methods for <see cref="RpcMiddleware"/>.
    /// </summary>
    internal static class RpcMiddlewareValidator
    {
        /// <summary>
        /// Validates <see cref="RpcCaller"/> object.
        /// </summary>
        public static void ValidateCaller(RpcCaller rpcCaller)
        {
            AssertHelper.ValidateIsNotNull(rpcCaller, nameof(rpcCaller));
        }

        /// <summary>
        /// Validates <see cref="RpcEndpointOptions"/> object.
        /// </summary>
        public static void ValidateOptions(RpcEndpointOptions options)
        {
            AssertHelper.ValidateIsNotNull(options.Path, nameof(options.Path));
            AssertHelper.ValidateIsNotNull(options.SerializeResponse, nameof(options.SerializeResponse));
            AssertHelper.ValidateIsNotNull(options.DeserializeRequest, nameof(options.DeserializeRequest));
            AssertHelper.ValidateIsNotNull(options.HandlResponse, nameof(options.HandlResponse));

            AssertHelper.ValidateIsNotEmpty(options.Path, nameof(options.Path));
        }
    }
}