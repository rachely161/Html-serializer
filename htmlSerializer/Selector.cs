using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htmlSerializer
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector()
        {
            Classes = new List<string>();
        }


        public static Selector FromQueryString(string query)
        {
            // מפצל את השאילתה לפי רווחים
            var levels = query.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Selector root = null;
            Selector current = null;

            foreach (var level in levels)
            {
                var parts = level.Split(new[] { '#', '.' }, StringSplitOptions.RemoveEmptyEntries);
                var newSelector = new Selector();

                // עדכון מאפיינים
                foreach (var part in parts)
                {
                    if (level.StartsWith("#") && newSelector.Id == null)
                    {
                        newSelector.Id = part;
                    }
                    else if (level.StartsWith("."))
                    {
                        newSelector.Classes.Add(part);
                    }
                    else
                    {
                        // בדיקה אם זה שם תקין של תגית HTML
                        if (IsValidHtmlTag(part))
                        {
                            newSelector.TagName = part;
                        }
                    }
                }

                // אם זו השורש
                if (root == null)
                {
                    root = newSelector;
                }
                else
                {
                    current.Child = newSelector; // הוספת ילד
                    newSelector.Parent = current; // הגדרת ההורה
                }

                current = newSelector; // עדכון הסלקטור הנוכחי
            }

            return root; // החזרת השורש
        }

        private static bool IsValidHtmlTag(string tag)
        {
            return HtmlHelper.Instance.HtmlTags.Contains(tag);
        }

    }
}
