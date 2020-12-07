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

using HandyHangoutStudios.Common.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HandyHangoutStudios.Common.Tests
{
    public class HtmlExtensionMethodTesting
    {
        [Fact]
        public void TestParagraph()
        {
            Assert.Equal("\n\n", "<p></p>".FromHtmlToDiscordMarkdown());
        }

        [Fact]
        public void EmphasisBecomesItalicized()
        {
            Assert.Equal("*Test*", "<em>Test</em>".FromHtmlToDiscordMarkdown());
            Assert.Equal("*Test*", "<i>Test</i>".FromHtmlToDiscordMarkdown());
        }

        [Fact]
        public void StrongBecomesBold()
        {
            Assert.Equal("**Test**", "<strong>Test</strong>".FromHtmlToDiscordMarkdown());
            Assert.Equal("**Test**", "<b>Test</b>".FromHtmlToDiscordMarkdown());
        }

        [Fact]
        public void StrongEmphasisBecomesBoldItalicized()
        {
            Assert.Equal("***Test***", "<strong><em>Test</em></strong>".FromHtmlToDiscordMarkdown());
            Assert.Equal("***Test***", "<em><strong>Test</strong></em>".FromHtmlToDiscordMarkdown());
        }

        [Fact]
        public void PrefixPostfixPropagates()
        {
            List<(string, string)> testPairs = new List<(string, string)>
            {
                ("<p></p>", "\n\n"),
                ("<p><em>Test</em></p>", "*Test*\n\n"),
                ("<p><strong>Test</strong></p>", "**Test**\n\n"),
                ("<p><strong><em>Test</em></strong></p>", "***Test***\n\n"),
            };

            foreach ((string html, string expected) in testPairs)
            {
                Assert.Equal(expected, html.FromHtmlToDiscordMarkdown());
            }
        }

        [Fact]
        public void BlockQuoteBecomeBlockQuote()
        {
            Assert.Equal("> Test", "<blockquote>Test</blockquote>".FromHtmlToDiscordMarkdown());
        }

        [Fact]
        public void BreakDoesntPropagateThroughParagraphs()
        {
            Assert.Equal("\n\n\n", "<p><br></p>".FromHtmlToDiscordMarkdown());
        }

        [Fact]
        public void BreakPropagates()
        {

            List<(string, string)> testPairs = new List<(string, string)>
            {
                ("<p><em>Test<br>Test</em></p>", "*Test*\n*Test*\n\n"),
                ("<p><strong>Test<br>Test</strong></p>", "**Test**\n**Test**\n\n"),
                ("<p><strong><em>Test<br>Test</em></strong></p>", "***Test***\n***Test***\n\n"),
                ("<blockquote>Test<br>Test</blockquote>", "> Test\n> Test"),
                // As counter-intuitive as this is, this is the correct behavior because a block-quote cannot be nested inside of a paragraph.
                ("<p><blockquote>Test<br>Test</blockquote></p>", "\n\n> Test\n> Test"),
                ("<p><blockquote><em>Test</em><br><strong>Test</strong></blockquote></p>", "\n\n> *Test*\n> **Test**"),
            };

            foreach ((string html, string expected) in testPairs)
            {
                Assert.Equal(expected, html.FromHtmlToDiscordMarkdown());
            }
        }

        [Fact]
        public void RealDataWorks()
        {
            string html = "<p>Yullis Rockheart still had most of his power left, even after the skin-blast that had taken care of the chandelier mimic—though the little maneuver <em>had </em>cost him his reinforced stone skin. Being back at Azure Branch was terribly annoying, but he would make do. Besides, he wouldn’t need much to finish his work. Killing Marko Laskarelis would be easy, and then only the fungaloid would be left. And though Logan had proven to be formidable in some ways, he was no melee monster.</p>";
            string expected = "Yullis Rockheart still had most of his power left, even after the skin-blast that had taken care of the chandelier mimic—though the little maneuver *had* cost him his reinforced stone skin. Being back at Azure Branch was terribly annoying, but he would make do. Besides, he wouldn’t need much to finish his work. Killing Marko Laskarelis would be easy, and then only the fungaloid would be left. And though Logan had proven to be formidable in some ways, he was no melee monster.\n\n";

            Assert.Equal(expected, html.FromHtmlToDiscordMarkdown());
        }

        [Fact]
        public void LineThroughBecomesTilde()
        {
            Assert.Equal("~~Test~~\n\n", "<p><span style=\"text-decoration: line-through\">Test</span></p>".FromHtmlToDiscordMarkdown());
        }

        [Fact]
        public void UnderlineBecomesUnderline()
        {
            Assert.Equal("__Test__\n\n", "<p><span style=\"text-decoration: underline\">Test</span></p>".FromHtmlToDiscordMarkdown());
        }
    }
}
