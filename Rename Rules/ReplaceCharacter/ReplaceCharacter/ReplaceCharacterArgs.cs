using Contract;
using System.Collections.Generic;
namespace ReplaceCharacter
{
    public class ReplaceCharacterArgs : StringArgs
    {
        public ReplaceCharacterArgs()
        {
            From = new List<char>();
        }
        public char To { get; set; }
        public List<char> From { get; set; }
    }
}
