using System;
using System.Collections.Generic;

namespace Mediatr.Rpc
{
    public class RpcOptions
    {
#nullable disable
        public Func<Type, string> MatchingConvention { get; set; }
        public IEnumerable<Type> Requests { get; set; }
#nullable restore
    }
}