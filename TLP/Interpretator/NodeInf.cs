using TLP.Lexer_;

namespace TLP.Interpretator
{
    public class NodeInf
    {
        public string NodeName { get; set; }
        public Token Token { get; set; }
        public int Level { get; set; }
        public string LongPath { get; set; }
    }
}
