using System;

namespace WebApplication2.Pages
{
    public class LowBool
    {
        public bool Value { get; set; }

        public override string ToString()
        {
            return Value.ToString().ToLower();
        }
    }
}