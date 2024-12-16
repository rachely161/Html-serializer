using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace htmlSerializer
{
    public class HtmlHelper
    {
        private static readonly HtmlHelper _instance = new HtmlHelper();
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        private HtmlHelper()
        {
            var tagsJson = File.ReadAllText("HtmlTags.json");
            var voidTagsJson = File.ReadAllText("HtmlVoidTags.json");

            HtmlTags = JsonSerializer.Deserialize<string[]>(tagsJson);
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(voidTagsJson);
        }
        public static HtmlHelper Instance=> _instance;

    }
}
