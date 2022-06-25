using System.ComponentModel.DataAnnotations;

namespace NeoForum.Models
{
    public class NeoFile
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
    }

    public class NeoFileViewModel
    {
        public List<NeoFile> Files { get; set; }
            = new List<NeoFile>();
    }
}
