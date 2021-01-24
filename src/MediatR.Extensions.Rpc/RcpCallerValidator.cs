using System;
using System.Linq;

using MediatR;

namespace Mediatr.Rpc
{
    /// <summary>
    /// Holds helpers validation methods for <see cref="RpcCaller"/>.
    /// </summary>
    internal static class RcpCallerValidator
    {
        /// <summary>
        /// Validates <see cref="ISender"/> object.
        /// </summary>
        public static void ValidateSender(ISender sender)
        {
            AssertHelper.ValidateIsNotNull(sender, nameof(sender));
        }

        /// <summary>
        /// Validates <see cref="RpcOptions"/> object.
        /// </summary>
        public static void ValidateOptions(RpcOptions options)
        {
            AssertHelper.ValidateIsNotNull(options.Requests, nameof(options.Requests));
            AssertHelper.ValidateIsNotNull(options.MatchingConvention, nameof(options.MatchingConvention));

            var nonMediatrRequests = options.Requests
                .Where(t => false == RequestTypeScanner.IsMediatrRequest(t))
                .ToList();

            if (nonMediatrRequests.Any())
            {
                var nonMediatrRequestTypeNames = nonMediatrRequests
                    .Aggregate(new System.Text.StringBuilder(), (a, t) => a.AppendLine(t.FullName));
                throw new ArgumentException($"All request types needs to derive from {nameof(IRequest)}. Non-accepted types: {nonMediatrRequestTypeNames}", nameof(options.Requests));
            }
        }
    }
}