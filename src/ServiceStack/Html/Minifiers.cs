﻿using System.Text.RegularExpressions;

namespace ServiceStack.Html
{
    public static class Minifiers
    {
        public static ICompressor JavaScript = new JSMinifier();
        public static ICompressor Css = new CssMinifier();

        public static ICompressor HtmlAdvanced = new HtmlCompressor
        {
            JavaScriptCompressor = JavaScript,
            CompressJavaScript = true,
            CssCompressor = Css,
            CompressCss = true,
        };

        public static ICompressor Html = new HtmlCompressor();
    }

    public class BasicHtmlMinifier
    {
        static Regex BetweenScriptTagsRegEx = new Regex(@"<script[^>]*>[\w|\t|\r|\W]*?</script>", RegexOptions.Compiled);
        static Regex BetweenTagsRegex = new Regex(@"(?<=[^])\t{2,}|(?<=[>])\s{2,}(?=[<])|(?<=[>])\s{2,11}(?=[<])|(?=[\n])\s{2,}|(?=[\r])\s{2,}", RegexOptions.Compiled);
        static Regex MatchBodyRegEx = new Regex(@"</body>", RegexOptions.Compiled);

        // https://github.com/kangax/html-minifier
        // C# port: http://stackoverflow.com/questions/16875114/regex-minify-pre-tag-content
        public static string MinifyHtml(string html)
        {
            if (html == null)
                return html;

            var mymatch = BetweenScriptTagsRegEx.Matches(html);
            html = BetweenScriptTagsRegEx.Replace(html, string.Empty);
            html = BetweenTagsRegex.Replace(html, string.Empty);

            var str = string.Empty;
            foreach (Match match in mymatch)
            {
                str += match.ToString();
            }

            html = MatchBodyRegEx.Replace(html, str + "</body>");
            return html;
        }        
    }
}