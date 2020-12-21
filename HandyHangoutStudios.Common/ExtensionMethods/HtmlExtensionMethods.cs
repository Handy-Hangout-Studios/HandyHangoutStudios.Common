//    HandyHangoutStudios.Common, common classes for use by the Handy Hangout Dev Team
//    Copyright (C) 2020 John Marsden

//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.

//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.

//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.

using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HandyHangoutStudios.Common.ExtensionMethods
{
    public static class HtmlExtensionMethods
    {
        public static string FromHtmlNodeToDiscordMarkdown(this HtmlNode documentNode)
        {
            return HtmlConvert(documentNode).ToString();
        }

        public static string FromHtmlToDiscordMarkdown(this string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            return Regex.Replace(ListToString(HtmlConvert(document.DocumentNode)), @"\n\n[\s-[\r\n]]", "\n\n");
        }

        private static List<StringBuilder> HtmlConvert(HtmlNode parent)
        {
            // Base Case
            if (parent.Name.Equals("#text") || !parent.ChildNodes.Any())
            {
                return new List<StringBuilder> { new StringBuilder().Append(parent.InnerHtml.EscapeDiscordSpecialCharacters()) };
            }

            List<StringBuilder> ttr = new List<StringBuilder>();
            StringBuilder temp = new StringBuilder();

            // Recursion
            foreach (var node in parent.ChildNodes)
            {
                StringBuilder nextPre = new StringBuilder();
                StringBuilder nextPost = new StringBuilder();
                Flags flags = new Flags();

                string nodeName = node.Name.ToLower();

                if (nodeName.Equals("p"))
                {
                    flags.IsParagraph = true;
                }
                else if (nodeName.Equals("em") || nodeName.Equals("i"))
                {
                    nextPre.Append(ITALIC);
                    nextPost.Append(ITALIC);
                }
                else if (nodeName.Equals("strong") || nodeName.Equals("b"))
                {
                    nextPre.Append(BOLD);
                    nextPost.Append(BOLD);
                }
                else if (nodeName.Equals("blockquote"))
                {
                    flags.IsBlockquote = true;
                }
                else if (nodeName.Equals("span"))
                {
                    HtmlAttribute style = node.Attributes["style"];
                    foreach (string value in style.Value.ToLower().Split(';'))
                    {
                        string[] valueSplit = value.Split(':');
                        if (valueSplit[0].Trim().Equals("text-decoration"))
                        {
                            if (valueSplit[1].Trim().Equals("underline"))
                            {
                                nextPre.Append(UNDERLINE);
                                nextPost.Insert(0, UNDERLINE);
                            }
                            else if (valueSplit[1].Trim().Equals("line-through"))
                            {
                                nextPre.Append(STRIKETHROUGH);
                                nextPost.Insert(0, STRIKETHROUGH);
                            }
                        }
                        if (valueSplit[0].Trim().Equals("font-weight"))
                        {
                            if (valueSplit[1].Trim().Equals("bold"))
                            {
                                nextPre.Append(BOLD);
                                nextPost.Insert(0, BOLD);
                            }
                        }
                        if (valueSplit[0].Trim().Equals("font-style"))
                        {
                            if (valueSplit[1].Trim().Equals("italic"))
                            {
                                nextPre.Append(ITALIC);
                                nextPost.Insert(0, ITALIC);
                            }
                        }
                    }
                }
                else if (nodeName.Equals("br"))
                {
                    ttr.Add(temp);
                    temp = new StringBuilder();
                }

                StringBuilder[] htmlLines = HtmlConvert(node).ToArray();

                StringBuilder str = htmlLines[0];
                string trimmedEnd = str.ToString().TrimEnd();
                string final = trimmedEnd.TrimStart();
                temp.Append(flags.IsBlockquote ? "> " : "")
                    .Append(' ', trimmedEnd.Length - final.Length)
                    .Append(nextPre)
                    .Append(final)
                    .Append(nextPost)
                    .Append(' ', str.Length - trimmedEnd.Length);

                for (int i = 1; i < htmlLines.Length; i++)
                {
                    str = htmlLines[i];
                    trimmedEnd = str.ToString().TrimEnd();
                    final = trimmedEnd.TrimStart();
                    temp.Append('\n')
                        .Append(flags.IsBlockquote ? "> " : "")
                        .Append(' ', trimmedEnd.Length - final.Length)
                        .Append(nextPre)
                        .Append(final)
                        .Append(nextPost)
                        .Append(' ', str.Length - trimmedEnd.Length);
                }

                temp.Append(flags.IsParagraph ? "\n\n" : "");
            }

            ttr.Add(temp);
            return ttr;
        }

        private static ISet<char> escapableCharacters = new HashSet<char>
        {
            '!', '@', '#', '$', '%', '^', '&',
            '*', '(', ')', '_', '-', '+', '=',
            '~', '`', '{', '[', ']', '}', '|',
            ':', ';', '"', '\'', '<', '>', ',',
            '?', '.', '/', '—', '“', '”'
        };

        private static StringBuilder EscapeDiscordSpecialCharacters(this string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach(char c in text)
            {
                if (escapableCharacters.Contains(c))
                {
                    sb.Append('\\');
                }
                sb.Append(c);
            }
            return sb;
        }

        public static string ListToString(List<StringBuilder> builders)
        {
            StringBuilder str = new StringBuilder();

            foreach (StringBuilder builder in builders)
            {
                str.Append(builder);
            }

            return str.ToString();
        }

        private const char ITALIC = '*';
        private const string BOLD = "**";
        private const string UNDERLINE = "__";
        private const string STRIKETHROUGH = "~~";
        private const char CODEBLOCK = '`';
        private const string MULTILINE_CODEBLOCK = "```";
        private const string BLOCKQUOTE = "> ";

        private struct Flags
        {
            public bool IsBlockquote;
            public bool IsList;
            public bool IsParagraph;
            public bool IsItalicized;
            public bool IsBolded;
            public bool IsUnderlined;
        }
    }
}
