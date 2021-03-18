namespace MediatR.Rpc.Azure.Functions
{
    internal static class RpcHttpFunctionValidator
    {
        public static void ValidateCaller(RpcCaller caller)
        {
            AssertHelper.ValidateIsNotNull(caller, nameof(caller));
        }

        public static void ValidateOptions(RpcHttpFunctionOptions options)
        {
            AssertHelper.ValidateIsNotNull(options.DeserializeRequest, nameof(options.DeserializeRequest));
            AssertHelper.ValidateIsNotNull(options.HandleResponse, nameof(options.HandleResponse));
        }
    }
}