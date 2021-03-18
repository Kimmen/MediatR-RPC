using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.Rpc
{
    /// <summary>
    /// Finds request types given a name. Will do a linear search through all requests.
    /// </summary>
    internal class LinearSearchRequestTypeProvider
    {
        private readonly IEnumerable<Type> requestTypes;
        private readonly Func<Type, string> resolveRequestName;

        public LinearSearchRequestTypeProvider(IEnumerable<Type> requestTypes, Func<Type, string> resolveRequestName)
        {
            this.requestTypes = requestTypes;
            this.resolveRequestName = resolveRequestName;
        }

        /// <summary>
        /// Finds a request type that corresponds to a specified name. A return value indicates if found.
        /// </summary>
        /// <param name="name">Name to match a request type with.</param>
        /// <param name="requestType">The corresponding request type, if found.</param>
        /// <returns>True if a request was found for the corresponding name; otherwise false.</returns>
        public bool TryGetByName(string name, out Type requestType)
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