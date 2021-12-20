using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class RawRule
    {
        public string RuleName { get; set; }
        public bool IsChecked { get; set; }
        public List<string> AgrList { get; set; }
    }
}
