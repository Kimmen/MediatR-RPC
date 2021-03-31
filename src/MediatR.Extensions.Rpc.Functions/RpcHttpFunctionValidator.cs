namespace MediatR.Rpc.Azure.Functions
{
    internal static class RpcHttpFunctionValidator
    {
        internal static void ValidateRunner(IRpcRequestRunner runner)
        {
            AssertHelper.ValidateIsNotNull(runner, nameof(runner));
        }

        internal static void ValidateOptions(RpcHttpFunctionOptions options)
        {
            AssertHelper.ValidateIsNotNull(options.DeserializeRequest, nameof(options.DeserializeRequest));
            AssertHelper.ValidateIsNotNull(options.SerializeResponse, nameof(options.SerializeResponse));
        }
    }
}