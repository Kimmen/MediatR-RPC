using System;
using System.Collections.Generic;

namespace Mediatr.Rpc
{
    public class RpcOptions
    {
        public Func<Type, string> MatchingConvention { get; set; }
        public IEnumerable<Type> Requests { get; set; }
    }
}