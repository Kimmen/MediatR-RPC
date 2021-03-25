namespace MediatR.Rpc.Azure.Functions
{
    internal static class RpcHttpFunctionValidator
    {
        public static void ValidateCaller(IRpcRequestRunner runner)
        {
            AssertHelper.ValidateIsNotNull(runner, nameof(runner));
        }

        public static void ValidateOptions(RpcHttpFunctionOptions options)
        {
            AssertHelper.ValidateIsNotNull(options.DeserializeRequest, nameof(options.DeserializeRequest));
            AssertHelper.ValidateIsNotNull(options.HandleResponse, nameof(options.HandleResponse));
        }
    }
}