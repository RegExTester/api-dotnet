using System;
using System.Collections.Generic;
using System.Linq;

namespace RegExTester.Api.DotNet.Models
{
    public class Input
    {
        public RegExTesterOptions Options { get; set; }
        public string Pattern { get; set; }
        public string Text { get; set; }
        public string Replace { get; set; }
    }
}
