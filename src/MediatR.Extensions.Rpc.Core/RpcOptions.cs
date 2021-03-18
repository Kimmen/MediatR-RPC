using System;
using System.Collections.Generic;

namespace MediatR.Rpc
{
    /// <summary>
    /// Options for the RCP behavior.
    /// </summary>
    public class RpcOptions
    {
#nullable disable
        /// <summary>
        /// Convention for generating a corresponding name given type.
        /// </summary>
        public Func<Type, string> MatchingConvention { get; set; }
        /// <summary>
        /// The requests that can be matched using RPC.
        /// </summary>
        public IEnumerable<Type> Requests { get; set; }
#nullable restore
    }
}