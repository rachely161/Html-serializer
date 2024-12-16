using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htmlSerializer
{
    public class Attribute
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public Attribute(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }

        public override string ToString()
        {
            return "name: "+Name+" value: "+Value;
        }

    }
}
