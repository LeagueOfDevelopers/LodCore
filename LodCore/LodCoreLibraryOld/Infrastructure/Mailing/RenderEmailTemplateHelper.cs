using System;
using System.IO;
using System.Linq;
using RazorEngine;
using RazorEngine.Templating;

namespace LodCoreLibraryOld.Infrastructure.Mailing
{
    public static class RenderEmailTemplateHelper
    {
        public static string RenderPartialToString(object model)
        {
            var modelType = model.GetType().ToString().Split('.').Last();

            var viewPath = $"~/Email_Templates/{modelType}.cshtml";

            var viewAbsolutePath = MapPath(viewPath);

            var viewSource = File.ReadAllText(viewAbsolutePath);

            var renderedText = Engine.Razor.RunCompile(viewSource, modelType, null, model);

            return renderedText;
        }

        private static string MapPath(string filePath)
        {
            return string.Format("{0}{1}", AppDomain.CurrentDomain.BaseDirectory,
                filePath.Replace("~", string.Empty).TrimStart('/'));
        }
    }
}