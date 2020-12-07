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
using System.Threading.Tasks;

namespace HandyHangoutStudios.Common.ExtensionMethods
{
    public static class HtmlExtensionMethods
    {

        public static string FromHtmlNodeToDiscordMarkdown(this HtmlNode documentNode)
        {
            return HtmlConvert(documentNode, new StringBuilder(), new StringBuilder()).ToString();
        }

        public static string FromHtmlToDiscordMarkdown(this string html)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            return HtmlConvert(document.DocumentNode, new StringBuilder(), new StringBuilder()).ToString();
        }

        private static StringBuilder HtmlConvert(HtmlNode parent, StringBuilder pre, StringBuilder post)
        {
            StringBuilder converted = new StringBuilder();
            // Base Case
            if (parent.Name.Equals("#text") || !parent.ChildNodes.Any())
            {
                string trimmedEnd = parent.EndNode.InnerHtml.TrimEnd();
                string final = trimmedEnd.TrimStart();
                return converted.Append(' ', trimmedEnd.Length - final.Length).Append(pre).Append(final).Append(post).Append(' ', parent.EndNode.InnerHtml.Length - trimmedEnd.Length);
            }

            // Recursion
            foreach (var node in parent.ChildNodes)
            {
                bool paragraph = false;
                StringBuilder nextPre = new StringBuilder();
                nextPre.Append(pre);
                StringBuilder nextPost = new StringBuilder();

                switch (node.Name.ToLower())
                {
                    case "p":
                        paragraph = true;
                        break;
                    case "em":
                    case "i":
                        nextPre.Append(ITALIC);
                        nextPost.Append(ITALIC);
                        break;
                    case "strong":
                    case "b":
                        nextPre.Append(BOLD);
                        nextPost.Append(BOLD);
                        break;
                    case "blockquote":
                        nextPre.Append(BLOCKQUOTE);
                        break;
                    case "span":
                        AppendSpanAttributes(node, nextPre, nextPost);
                        break;
                }

                if (!node.Name.ToLower().Equals("br"))
                {
                    nextPost.Append(post);
                }
                else
                {
                    nextPre.Clear();
                    nextPost.Append('\n');
                }

                converted.Append(HtmlConvert(node, nextPre, nextPost)).Append(paragraph ? "\n\n" : "");
            }

            return converted;
        }

        private static void AppendSpanAttributes(HtmlNode node, StringBuilder nextPre, StringBuilder nextPost)
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

        private const char ITALIC = '*';
        private const string BOLD = "**";
        private const string UNDERLINE = "__";
        private const string STRIKETHROUGH = "~~";
        private const char CODEBLOCK = '`';
        private const string MULTILINE_CODEBLOCK = "```";
        private const string BLOCKQUOTE = "> ";
    }
}
