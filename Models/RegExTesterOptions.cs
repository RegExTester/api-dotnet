using System;
using System.Text.RegularExpressions;

namespace RegExTester.Api.DotNet.Models
{
    [Flags]
    public enum RegExTesterOptions
    {
        None = RegexOptions.None,                                           // 0b0000_0000_0000_0000 = 0
        IgnoreCase = RegexOptions.IgnoreCase,                               // 0b0000_0000_0000_0001 = 1
        Multiline = RegexOptions.Multiline,                                 // 0b0000_0000_0000_0010 = 2
        ExplicitCapture = RegexOptions.ExplicitCapture,                     // 0b0000_0000_0000_0100 = 4
        Compiled = RegexOptions.Compiled,                                   // 0b0000_0000_0000_1000 = 8
        Singleline = RegexOptions.Singleline,                               // 0b0000_0000_0001_0000 = 16
        IgnorePatternWhitespace = RegexOptions.IgnorePatternWhitespace,     // 0b0000_0000_0010_0000 = 32
        RightToLeft = RegexOptions.RightToLeft,                             // 0b0000_0000_0100_0000 = 64
        ECMAScript = RegexOptions.ECMAScript,                               // 0b0000_0001_0000_0000 = 256
        CultureInvariant = RegexOptions.CultureInvariant,                   // 0b0000_0010_0000_0000 = 512
        ShowCaptures = 1 << 15                                              // 0b1000_0000_0000_0000 = 32768
    }
}
