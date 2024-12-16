// See https://aka.ms/new-console-template for more information

using htmlSerializer;
using System.Text.RegularExpressions;
using Attribute = htmlSerializer.Attribute;

var html=await Load("https://hebrewbooks.org/beis");

//var cleanHtml = new Regex("\\s").Replace(html,"");
var cleanHtml = Regex.Replace(html, @"(?<=<[^>]*?>)\s+|\s+(?=<)|[\r\n]+", "");

//<text>
var htmlLines= new Regex("<(.*?)>").Split(cleanHtml).Where(s=>s.Length>0).ToList();


var root = Parse(htmlLines);
var selector = Selector.FromQueryString("div .formPopup");
IEnumerable<HtmlElement> matches = root.FindElementsBySelector(selector);
foreach (var match in matches)
{
    Console.WriteLine(match.ToString());
    Console.WriteLine();
    Console.WriteLine();
}


Console.ReadLine();

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

HtmlElement Parse(List<string> inputLines)
{
    HtmlElement root = new HtmlElement { Name = "root" };
    HtmlElement currentElement = root;

    foreach (var line in inputLines)
    {
        string[] parts = line.Split(new[] { ' ' }, 2);
        string tag = parts[0];

        if (tag == "html/")
        {
            break; // סיום ה-html
        }

        if (tag.StartsWith("/"))
        {
            // תגית סוגרת
            if(currentElement!=null)
                currentElement = currentElement.Parent;
            continue;
        }

        if (HtmlHelper.Instance.HtmlTags.Contains(tag))
        {
            HtmlElement newElement = new HtmlElement { Name = tag, Parent = currentElement };

            if (parts.Length > 1)
            {
                string attributesString = parts[1];

                // בדוק אם יש attribute
                if (attributesString.Contains("="))
                {
                    ParseAttributes(attributesString, newElement);
                }
                else
                {
                    // כאן אתה יכול להוסיף את ה-innerHTML
                    newElement.InnerHtml = attributesString; // אם זה innerHTML
                }
            }

            if (tag.EndsWith("/") || HtmlHelper.Instance.HtmlVoidTags.Contains(tag))
            {
                if(currentElement!= null)
                // תגית סוגרת את עצמה
                    currentElement.Children.Add(newElement);
            }
            else
            {
                // הוסף את האלמנט החדש לרשימת הילדים של האלמנט הנוכחי
                if(currentElement!= null)   
                    currentElement.Children.Add(newElement);
                currentElement = newElement; // עדכון האלמנט הנוכחי
            }
        }
        else
        {
            if(currentElement!=null)
            {
                string str = parts[0];
                if (parts.Length > 1)
                    str = str + parts[1];

                // כאן אתה יכול להוסיף את ה-innerHTML
                currentElement.InnerHtml = str; // אם זה innerHTML
            }

        }

        
    }

    return root;
}

void ParseAttributes(string attributes, HtmlElement element)
{
    if (element == null)
        return;
    var matches = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(attributes);

    foreach (Match match in matches)
    {
        string key = match.Groups[1].Value;
        string value = match.Groups[2].Value.Trim('"', '\'');

        if (key == "class")
        {
            element.Classes.AddRange(value.Split(' '));
        }
        else if (key == "id")
        {
            element.Id = value;
        }
        else
        {
            element.Attributes.Add(new Attribute(key, value));
        }
    }

}
