using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PlanMy.Library
{
    public static class HtmlTools
    {
        public static string Strip(string text)
        {
            return Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        public static string WrapContent(Blog post)
        {
            var sb = new StringBuilder();
            var content = post.HtmlDescription;

            // remove first img from post if there's one
            if (post.Image != null)
            {
                content = Regex.Replace(content, "^<img.*?", "");
                content = Regex.Replace(content, "^<p><img.*?</p>", "");
            }

            sb.Append("<html><head>");
            sb.Append("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0, user-scalable=no\">");
            sb.Append("<style>");
            sb.Append("body { max-width:700px; margin: 0 auto; font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; padding: 0 10px 10px 10px; box-sizing: border-box; } div[id^=\"attachment\"] { max-width: calc(100% + 30px) !important; } img { max-width: calc(100% + 30px) !important; height: auto; margin-left: calc(-15px); margin-right: calc(-15px); } img#featured { width: calc(100% + 30px); margin-left: calc(-15px); margin-right: calc(-15px); } h1 { font-weight: lighter; } #postmeta { font-style: italic; } iframe { width: calc(100% + 30px); margin-left: calc(-15px); margin-right: calc(-15px); } @media (max-width: 639px) { img { width: calc(100% + 25px); margin-left: calc(-15px); margin-right: calc(-15px); } }");
            sb.Append("</style>");
            sb.Append("</head><body>");

            sb.Append(FeaturedImage(post));
            sb.Append($"<h1>{post.Title}</h1>");
            
            sb.Append($"<p id=\"postmeta\">admin | {post.PostDate}</p>");
            sb.Append(content);
            sb.Append("</body></html>");

            return sb.ToString();
        }

        public static string FeaturedImage(Blog post)
        {
            if (post.Image == null)
                return string.Empty;

            
            var imgSrc = Statics.MediaLink + post.Image;

            var sb = new StringBuilder();
            sb.Append("<img class=\"alignnone size-full\" ");
            sb.Append($"src=\"{imgSrc}\" ");
            sb.Append(" />");

            return sb.ToString();
        }
    }
}
