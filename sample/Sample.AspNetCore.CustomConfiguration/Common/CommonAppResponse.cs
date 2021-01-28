
using System.Collections.Generic;
using System.Linq;

namespace Sample.AspNetCore.CustomConfiguration.Handlers
{
    public class CommonAppResponse
    {
        public object Response { get; set; }
        public IReadOnlyCollection<ValidationError> ValidationErrors { get; set; }
    }

    public static class RespondWith
    {
        public static CommonAppResponse Ok(object response)
        {
            return new CommonAppResponse
            {
                Response = response,
                ValidationErrors = Enumerable.Empty<ValidationError>().ToArray()
            };
        }

        public static CommonAppResponse NotFound()
        {
            return new CommonAppResponse
            {
                Response = null,
                ValidationErrors = Enumerable.Empty<ValidationError>().ToArray()
            };
        }

        public static CommonAppResponse ValidationErrors(params ValidationError[] errors)
        {
            return new CommonAppResponse
            {
                Response = null,
                ValidationErrors = errors
            };
        }
    }
}
