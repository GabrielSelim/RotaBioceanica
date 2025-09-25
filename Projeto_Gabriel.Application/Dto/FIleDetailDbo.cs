using System.Text.Json.Serialization;

namespace Projeto_Gabriel.Application.Dto
{
    public class FIleDetailDbo
    {
        public string DocumentName { get; set; }

        public string DocType { get; set; }

        [JsonIgnore]
        public string DocUrl { get; set; }

        public string SizeInMb { get; set; }
    }
}
