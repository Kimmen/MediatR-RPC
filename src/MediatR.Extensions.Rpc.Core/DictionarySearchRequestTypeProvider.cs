using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.Rpc
{
    internal class DictionarySearchRequestTypeProvider
    {
        private readonly Dictionary<string, Type> mappedRequests;

        internal DictionarySearchRequestTypeProvider(IEnumerable<Type> requestTypes, Func<Type, string> resolveRequestName)
        {
            this.mappedRequests = requestTypes
                .ToDictionary(t => resolveRequestName(t).ToLowerInvariant());
        }

        internal bool TryGetByName(string name, out Type requestType)
        {
            return this.mappedRequests.TryGetValue(name.ToLowerInvariant(), out requestType);
        }
    }
}