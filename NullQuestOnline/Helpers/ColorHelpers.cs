using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NullQuestOnline.Helpers
{
    public static class ColorHelpers
    {
        private static Dictionary<char, string> colorCodes;
        static ColorHelpers()
        {
            colorCodes = new Dictionary<char, string>(7);
            colorCodes.Add('R', "#FF0000");
            colorCodes.Add('G', "#00FF00");
            colorCodes.Add('Y', "#FFFF00");
            colorCodes.Add('V', "#FF00FF");
            colorCodes.Add('C', "#00FFFF");
            colorCodes.Add('W', "#FFFFFF");
        }

        public static MvcHtmlString Colorize(this string str)
        {
            var sb = new StringBuilder();
            bool inTag = false;
            for (int i = 0; i < str.Length - 1; i++)
            {
                if (str[i] == '^')
                {
                    if (str[i + 1] == 'N')
                    {
                        if (inTag)
                        {
                            sb.Append("</span>");
                            inTag = false;
                        }
                        i++;
                    }
                    if (str[i + 1] == 'K')
                    {
                        if (inTag)
                        {
                            sb.Append("</span>");
                        }
                        sb.Append("<span style=\"color: #FF0000; background-color: #CCCCCC;\">");
                        inTag = true;
                        i++;
                    }
                    if (colorCodes.ContainsKey(str[i + 1]))
                    {
                        if (inTag)
                        {
                            sb.Append("</span>");
                        }
                        sb.AppendFormat("<span style=\"color: {0}; \">", colorCodes[str[i + 1]]);
                        inTag = true;
                        i++;
                    }
                }
                else
                {
                    sb.Append(str[i]);
                }
            }
            if (str.Length > 0)
            {
                sb.Append(str[str.Length - 1]);
            }
            return new MvcHtmlString(sb.ToString());
        }
    }
}