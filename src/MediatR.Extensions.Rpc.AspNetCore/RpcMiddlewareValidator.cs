namespace MediatR.Rpc.AspNetCore
{
    /// <summary>
    /// Holds helpers validation methods for <see cref="RpcMiddleware"/>.
    /// </summary>
    internal static class RpcMiddlewareValidator
    {
        /// <summary>
        /// Validates <see cref="IRpcRequestRunner"/> object.
        /// </summary>
        public static void ValidateCaller(IRpcRequestRunner rpcRunner)
        {
            AssertHelper.ValidateIsNotNull(rpcRunner, nameof(rpcRunner));
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