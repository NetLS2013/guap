using Guap.Helpers;
using Newtonsoft.Json;

namespace Guap.Models
{
    public class ResultModel
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        
        [JsonProperty("result")]
        public bool Result { get; set; }

        public string Message
        {
            get
            {
                StatusResult.Messages.TryGetValue((StatusMessage) Code, out string message);

                return message ?? "Internal application error";
            }
        }
    }
}