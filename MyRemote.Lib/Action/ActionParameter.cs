using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRemote.Lib.Action
{
    public class ActionParameter
    {
        public string Name { get; set; }

        public string Value { get; set; }


        public int AsInt32()
        {
            return int.Parse(Value);
        }

        public long AsInt64()
        {
            return long.Parse(Value);
        }


        public static implicit operator string(ActionParameter prm)
        {
            return prm.Value;
        }

    }
}
