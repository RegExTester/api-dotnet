using System.ComponentModel.DataAnnotations;

namespace RegExTester.Api.DotNet.Models
{
    public class Input
    {
        public RegExTesterOptions Options { get; set; }

        [MaxLength(100*1024)] // 100KB
        public string Pattern { get; set; }

        [MaxLength(100 * 1024)] // 100KB
        public string Text { get; set; }

        [MaxLength(100 * 1024)] // 100KB
        public string Replace { get; set; }
    }
}
