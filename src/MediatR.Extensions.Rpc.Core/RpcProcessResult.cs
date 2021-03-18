namespace MediatR.Rpc
{
    /// <summary>
    /// Result of a processed request.
    /// </summary>
    public interface IRpcResult
    {
    }

    /// <summary>
    /// Result of successfully processed request.
    /// </summary>
    public struct SuccessfullyProcessedRequestResult : IRpcResult
    {
        /// <summary>
        /// The response of request process.
        /// </summary>
        public object? Response { get; internal set; }
    }

    /// <summary>
    /// Result of a not found request.
    /// </summary>
    public struct NotFoundRequestResult : IRpcResult
    {
        /// <summary>
        /// Name of the request that was not found.
        /// </summary>
        public string RequestName { get; set; }
    }
}