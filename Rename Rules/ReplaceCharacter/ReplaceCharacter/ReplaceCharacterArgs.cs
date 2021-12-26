using Contract;
using System.ComponentModel;

namespace ReplaceCharacter
{
    public class ReplaceCharacterArgs : StringArgs
    {
        public ReplaceCharacterArgs()
        {
            From = new BindingList<string>();
        }
        public string To { get; set; }
        public BindingList<string> From { get; set; }
    }
}
