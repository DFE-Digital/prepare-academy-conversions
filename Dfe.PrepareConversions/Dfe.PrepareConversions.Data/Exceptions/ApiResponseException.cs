using System;
using System.Runtime.Serialization;

namespace Dfe.PrepareConversions.Data.Exceptions;

[Serializable]
public class ApiResponseException : Exception
{
   public ApiResponseException(string message) : base(message)
   {
   }

   protected ApiResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
   {
   }
}
