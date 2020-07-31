using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RegExTester.Api.DotNet.Models;

namespace RegExTester.Api.DotNet.Services
{
    public interface IRegExProcessor
    {
        dynamic Matches(string pattern, string text, string replace, RegExTesterOptions options);
    }

    public class RegExProcessor : IRegExProcessor
    {
        public dynamic Matches(string pattern, string text, string replace, RegExTesterOptions options)
        {
            string error = null;
            string replaceResult = null;
            var matchesResult = new List<dynamic>();

            try
            {
                var showCaptures = options.HasFlag(RegExTesterOptions.ShowCaptures);
                var regexOptions = showCaptures ? (RegexOptions)(options - RegExTesterOptions.ShowCaptures) : (RegexOptions)options;
                var regex = new Regex(pattern, regexOptions);
                var matches = regex.Matches(text);

                foreach (Match match in matches)
                {
                    var matchItem = new {
                        name = match.Name,
                        index = match.Index,
                        length = match.Length,
                        value = match.Value,
                        //groups_count = match.Groups.Count,
                        groups = new List<dynamic>(),
                        //captures_count = showCaptures ? (int?)match.Captures.Count : null,
                        captures = showCaptures ? GetCaptures(match.Captures) : null
                    };

                    foreach (Group group in match.Groups)
                    {
                        var groupItem = new {
                            name = group.Name,
                            index = group.Index,
                            length = group.Length,
                            value = group.Value,
                            //captures_count = showCaptures ? (int?)group.Captures.Count : null,
                            captures = showCaptures ? GetCaptures(group.Captures) : null
                        };

                        matchItem.groups.Add(groupItem);
                    }

                    matchesResult.Add(matchItem);
                }

                if (replace != null)
                {
                    replaceResult = regex.Replace(text, replace);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return new {
                error = error,
                replace = replaceResult,
                matches = matchesResult
            };
        }

        private List<dynamic> GetCaptures(CaptureCollection captures)
        {
            var result = new List<dynamic>();
            foreach (Capture capture in captures)
            {
                result.Add(new {
                    index = capture.Index,
                    length = capture.Length,
                    value = capture.Value
                });
            }
            return result;
        }
    }
}
