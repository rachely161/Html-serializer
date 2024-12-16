using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htmlSerializer
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }

        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement()
        {
            Children = new List<HtmlElement>();
            Classes = new List<string>();   
            Attributes = new List<Attribute>();
        }

        // פונקציית Descendants
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            queue.Enqueue(this);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                yield return current;

                foreach (var child in current.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        public IEnumerable<HtmlElement> Ancestors()
        {
            HtmlElement curr = this;
            while (curr != null)
            {
                yield return curr;
                curr = curr.Parent;
            }
        }

        public IEnumerable<HtmlElement> FindElementsBySelector(Selector selector)
        {
            HashSet<HtmlElement> result = new HashSet<HtmlElement>();
            FindElementsBySelectorRecursive(this, selector, result);
            return result;
        }


        private void FindElementsBySelectorRecursive(HtmlElement element, Selector selector, HashSet<HtmlElement> results)
        {
            // קבלת כל הצאצאים של האלמנט הנוכחי
            var descendants = element.Descendants();
            var matchDescendants=descendants.Where(descendant =>IsMatchToSelector(descendant, selector));
                foreach(HtmlElement match in matchDescendants)
                {
                    if (selector.Child == null)
                        results.Add(match);
                    else
                        FindElementsBySelectorRecursive(match,selector.Child,results);  
                }

        }

        private bool IsMatchToSelector(HtmlElement element, Selector selector)
        {
            if (selector.TagName != null&&element.Name!=selector.TagName)
               return false;
            if (selector.Id != null&&element.Name!=selector.Id)
               return false;
            if(selector.Classes.Count>0)
                foreach (var c in selector.Classes)
                {
                    if (!element.Classes.Contains(c))
                        return false;
                }
            return true;
        }
        public override string ToString()
        {
            string str;
            str = "id: " + Id;
            str += "\nname: " + Name;
            str += "\nattributes";
            Attributes.ForEach(a => str += "\n" + a.ToString);
            str += "\nclasses:";
            Classes.ForEach(c => str += "\n"+c.ToString());
            str += "\ninnerHtml: " + InnerHtml;
            str += "\n";
            str += "\nparent:" + Parent;

            return str;

        }
    }

    


}
