using RazorEngine;
using RazorEngine.Templating;
using System;
using System.IO;
using System.Linq;

namespace LodCore.Infrastructure.Mailing
{
    public static class RenderEmailTemplateHelper
    {
        public static string RenderPartialToString(object model)
        {
            var modelType = model.GetType().ToString().Split('.').Last();

            var viewPath = $"~/Email_Templates/{modelType}.cshtml";

            string viewAbsolutePath = MapPath(viewPath);

            var viewSource = File.ReadAllText(viewAbsolutePath);

            var renderedText = Engine.Razor.RunCompile(viewSource, modelType, null, model);

            return renderedText;
        }

        private static string MapPath(string filePath)
        {
            return string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory, filePath.Replace("~", string.Empty).TrimStart('/'));
        }
    }
}
