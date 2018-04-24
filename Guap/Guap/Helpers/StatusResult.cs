using System.Collections.Generic;

namespace Guap.Helpers
{
    public static class StatusResult
    {
        public static Dictionary<StatusMessage, string> Messages = new Dictionary<StatusMessage, string>
        {
            { StatusMessage.PhoneNumberIsNotValid, "The number you entered is invalid." },
            { StatusMessage.EmailIsNotValid, "The email you entered is invalid." }
        };
    }
    
    public enum StatusMessage
    {
        PhoneNumberIsNotValid = 1001,
        EmailIsNotValid = 1002
    }
}