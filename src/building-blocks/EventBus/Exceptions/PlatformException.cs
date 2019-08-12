using System;

namespace EventBus.Exceptions
{
    public class PlatformException: Exception
    {
        public PlatformException(int code, string message) : base(message)
        {
            Code = code;
        }
        
        public int Code { get; set; }
    }
}
