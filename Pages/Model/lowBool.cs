using System;

namespace ECSForm.Model
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