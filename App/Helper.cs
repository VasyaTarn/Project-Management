using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App
{
    public class Helper
    {
        public String EscapeHtml(string input, Dictionary<string, string>? map = null)
        {
            if (input == null) throw new ArgumentNullException("HTML should not be null");
            if (input.Length == 0)
            {
                throw new ArgumentException("HTML must not be empty");
            }
            if (!IsHtmlValid(input))
            {
                throw new ArgumentException("This should be a correct html file");
            }
            Dictionary<string, string> htmlSpecSymbols = map ?? new Dictionary<string, string> { { "&", "&amp;" }, { "<", "&lt;" }, { ">", "&gt;" } };
            var escapedHtml = new StringBuilder();
            foreach (var c in input)
                escapedHtml.Append(htmlSpecSymbols.TryGetValue(c.ToString(), out var replacement) ? replacement : c.ToString());

            return escapedHtml.ToString();
        }

        public bool ContainsAttributes(String html)
        {
            string pattern = @"\w+\s+[^=]*(\w+=[^>]+)+>";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(html);
        }

        private bool IsHtmlValid(string input)
        {
            int openTagCount = 0;
            foreach (char c in input)
            {
                if (c == '<')
                {
                    openTagCount++;
                }
                else if (c == '>')
                {
                    if (openTagCount == 0)
                    {
                        return false;
                    }
                    openTagCount--;
                }
            }

            return openTagCount == 0;
        }

        public String Ellipsis(string input, int len)
        {
            if (input == null) throw new ArgumentNullException("Null detected of parameter: " + nameof(input));
            if (len < 3)
            {
                throw new ArgumentException("Argument 'len' could not be less than 3");
            }
            if (input.Length < len) {
                throw new ArgumentException("Argument 'len' could not be greater than input length");
            }
            return input.Substring(0, len - 3) + "...";
        }

        public String Finalize(String input)
        {
            return input.Substring(input.Length - 1) == "." ? input : input + ".";
        }

        public string CombineUrl(params String[] patrs)
        {
            try
            {
                string result = ""; 
                int i = 0;
                bool wasNull = false; 
                while (i < patrs.Length)
                {
                    if (patrs[i] == null)
                    {
                        i++;
                        wasNull = true; 
                        continue; 
                    }
                    if (wasNull) 
                    {
                        throw new ArgumentException("Not null argument after null one"); 
                                                                                         
                    }
                    result += patrs[i].Replace("/", "") == ".." ? "" : "/" + patrs[i].Replace("/", ""); 
                                                                                                        
                    i++;
                }
                if (result.Length == 0)
                {
                    throw new ArgumentException("Arguments are null!"); 
                }
                return result;
            }
            catch (NullReferenceException)
            {
                throw new ArgumentException("Arguments are null!"); 
                                                                    
            }
        }
    }
}
