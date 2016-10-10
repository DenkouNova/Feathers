using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Feathers
{
    // eventually maybe: implement Comparable, Equals and stuff

    public class FeatherItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public FeatherItem(string text, object value)
        {
            this.Text = text;
            this.Value = value;
        }

        public override string ToString() { return Text; }
    }
}
