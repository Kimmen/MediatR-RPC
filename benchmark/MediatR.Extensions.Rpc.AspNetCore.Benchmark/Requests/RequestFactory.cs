using System;
using System.Collections.Generic;
using System.Linq;

namespace MediatR.Rpc.AspNetCore.Benchmark.Requests
{
    public class RequestFactory
    {
        private Dictionary<Type, object> requests;

        public RequestFactory()
        {
            this.requests = RequestTypeScanner
                .FindRequestTypes(GetType().Assembly.GetTypes())
                .ToDictionary(t => t, t => Activator.CreateInstance(t));
        }

        public IEnumerable<Type> TakeRequestTypes(int count)
        {
            return this.requests.Keys.OrderBy(x => x.Name).Take(count);
        }

        public int Count()
        {
            return this.requests.Count;
        }
    }
}
