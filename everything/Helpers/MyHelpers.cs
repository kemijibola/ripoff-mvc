using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace everything.Helpers
{
    public static class MyHelpers
    {
        // Render BootStrap menu item with active class
        public static MvcHtmlString MenuItem(this HtmlHelper htmlHelper,
                                             string text, string action,
                                             string controller,
                                             object routeValues = null,
                                             object htmlAttributes = null)
        {
            var li = new TagBuilder("li");
            var routeData = htmlHelper.ViewContext.RouteData;
            var currentAction = routeData.GetRequiredString("action");
            var currentController = routeData.GetRequiredString("controller");
            if (string.Equals(currentAction,
                              action,
                              StringComparison.OrdinalIgnoreCase) &&
                string.Equals(currentController,
                              controller,
                              StringComparison.OrdinalIgnoreCase))
            {
                li.AddCssClass("active");
            }
            if (routeValues != null)
            {
                li.InnerHtml = (htmlAttributes != null)
                    ? htmlHelper.ActionLink(text,
                                            action,
                                            controller,
                                            routeValues,
                                            htmlAttributes).ToHtmlString()
                    : htmlHelper.ActionLink(text,
                                            action,
                                            controller,
                                            routeValues).ToHtmlString();
            }
            else
            {
                li.InnerHtml = (htmlAttributes != null)
                    ? htmlHelper.ActionLink(text,
                                            action,
                                            controller,
                                            null,
                                            htmlAttributes).ToHtmlString()
                    : htmlHelper.ActionLink(text,
                                            action,
                                            controller).ToHtmlString();
            }
            return MvcHtmlString.Create(li.ToString());
        }


        // As the text the: "<span class='glyphicon glyphicon-plus'></span>" can be entered
        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
                                             string text, string title, string action,
                                             string controller,
                                             object routeValues = null,
                                             object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            TagBuilder  builder = new TagBuilder("a");
            builder.InnerHtml = text;
            builder.Attributes["title"] = title;
            builder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            builder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

            return MvcHtmlString.Create(builder.ToString());
        }

        //public static MvcHtmlString MyInput(this HtmlHelper htmlHelper, string name, string value, string icon)
        //{
        //    var attrs = new Dictionary<string, object>();
        //    attrs.Add("data_icon", icon);
        //    return htmlHelper.TextBox(name, name, value, attrs);
        //}
        public static MvcHtmlString Timeago(this HtmlHelper helper, DateTime dateTime)
        {
            var tag = new TagBuilder("abbr");
            tag.AddCssClass("timeago");
            tag.Attributes.Add("title", dateTime.ToString("s") + "Z");
            tag.SetInnerText(dateTime.ToString());

            return MvcHtmlString.Create(tag.ToString());
        }

            public static string ActivePage(this HtmlHelper helper, string controller, string action)
            {
                string classValue = "";

                string currentController = helper.ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString();
                string currentAction = helper.ViewContext.Controller.ValueProvider.GetValue("action").RawValue.ToString();

                if (currentController == controller && currentAction == action)
                {
                    classValue = "active";
                }

                return classValue;
            }


    }
}