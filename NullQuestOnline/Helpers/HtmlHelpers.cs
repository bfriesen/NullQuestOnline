using System;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace NullQuestOnline.Helpers
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString KeyOptions(this HtmlHelper html, object htmlAttributes = null, params KeyedOption[] options)
        {
            var ul = new TagBuilder("div");
            var sb = new StringBuilder();
            foreach (var keyedOption in options)
            {
                var li = new TagBuilder("div");
                li.InnerHtml = string.Format("[{0}] <a class=\"key-option\" data-key=\"{0}\" href=\"{1}\">{2}</a>", keyedOption.Key, keyedOption.Url, HttpUtility.HtmlEncode(keyedOption.Text));
                sb.Append(li.ToString());
            }
            ul.InnerHtml = sb.ToString();

            if (htmlAttributes != null)
            {
                var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
                ul.MergeAttributes(attributes);
            }
            return new MvcHtmlString(ul.ToString());
        }

        public class KeyedOption
        {
            public char Key { get; set; }
            public string Text { get; set; }
            public string Url { get; set; }

            public KeyedOption(char key, string text, string url)
            {
                Url = url;
                Text = text;
                Key = key;
            }

            public static KeyedOption Create(char key, string text, string url)
            {
                return new KeyedOption(key, text, url);
            }
        }

        public static IHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            var enumType = Nullable.GetUnderlyingType(metadata.ModelType) ?? metadata.ModelType;

            var enumValues = Enum.GetValues(enumType).Cast<object>();

            var items = from enumValue in enumValues
                        select new SelectListItem
                        {
                            Text = enumValue.ToString(),
                            Value = ((int)enumValue).ToString(),
                            Selected = enumValue.Equals(metadata.Model)
                        };

            return html.DropDownListFor(expression, items, string.Empty, null);
        }

        public static MvcHtmlString ColorPercentage(this HtmlHelper helper, double percentage, string zeroColor, string fullColor)
        {
            var zero = ColorTranslator.FromHtml(zeroColor);
            var full = ColorTranslator.FromHtml(fullColor);
            return new MvcHtmlString(
                ColorTranslator.ToHtml(
                    Color.FromArgb(
                        Step(percentage, zero.A, full.A),
                        Step(percentage, zero.R, full.R),
                        Step(percentage, zero.G, full.G),
                        Step(percentage, zero.B, full.B)
                )));
        }

        private static int Step(double percentage, int bottom, int top)
        {
            return (int) (bottom + (percentage*(top - bottom)));
        }
    }
}