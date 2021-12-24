using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contract;

namespace AddCounterRule
{
    class AddCounterArgs:StringArgs
    {
        public int? Start { get; set; }
        public int? Counter { get; set; }
        public int? Step { get; set; }
        public int? Digit { get; set; }

    }
}
