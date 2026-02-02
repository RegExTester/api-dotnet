using System.ComponentModel.DataAnnotations;

namespace RegExTester.Api.DotNet.Models
{
    public class Input
    {
        public RegExTesterOptions Options { get; set; }

        [StringLength(512)]
        public string Pattern { get; set; }

        [StringLength(1024)]
        public string Text { get; set; }

        [StringLength(1024)]
        public string Replace { get; set; }
    }
}
