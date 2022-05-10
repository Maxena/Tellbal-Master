using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class PolicyException : AppException
    {
        public PolicyException()
            : base(ApiResultStatusCode.PolicyError, System.Net.HttpStatusCode.UnavailableForLegalReasons)
        {
        }

        public PolicyException(string message)
            : base(ApiResultStatusCode.PolicyError, message, System.Net.HttpStatusCode.UnavailableForLegalReasons)
        {
        }

        public PolicyException(object additionalData)
            : base(ApiResultStatusCode.PolicyError, null, System.Net.HttpStatusCode.UnavailableForLegalReasons, additionalData)
        {
        }

        public PolicyException(string message, object additionalData)
            : base(ApiResultStatusCode.PolicyError, message, System.Net.HttpStatusCode.UnavailableForLegalReasons, additionalData)
        {
        }

        public PolicyException(string message, Exception exception)
            : base(ApiResultStatusCode.PolicyError, message, exception, System.Net.HttpStatusCode.UnavailableForLegalReasons)
        {
        }

        public PolicyException(string message, Exception exception, object additionalData)
            : base(ApiResultStatusCode.PolicyError, message, System.Net.HttpStatusCode.UnavailableForLegalReasons, exception, additionalData)
        {
        }
    }
}
