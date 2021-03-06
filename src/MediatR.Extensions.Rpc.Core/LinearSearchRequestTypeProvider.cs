using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.Rpc
{
    internal class LinearSearchRequestTypeProvider
    {
        private readonly IEnumerable<Type> requestTypes;
        private readonly Func<Type, string> resolveRequestName;

        internal LinearSearchRequestTypeProvider(IEnumerable<Type> requestTypes, Func<Type, string> resolveRequestName)
        {
            this.requestTypes = requestTypes;
            this.resolveRequestName = resolveRequestName;
        }

        internal bool TryGetByName(string name, out Type requestType)
        {
            requestType = this.requestTypes
                .FirstOrDefault(d =>
                {
                    var compareName = this.resolveRequestName(d);
                    return compareName.Equals(name, StringComparison.OrdinalIgnoreCase);
                });

            return requestType != default;
        }
    }
}