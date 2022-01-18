
namespace TLP.Lexer_
{
    public class Token
    {
        public enum TokenType
        {
            None,
            EOE,                    //  Конец файла
            Keyword,                //  Ключевое слово
            Identity,               //  Идентификатор
            Int,                    //  Int                    
            Double,                 //  Double
            String,                 //  String
            Bool,                   //  Bool
            Signseparator,          //  Значимый разделитель
            Notsignseparator        //  Незначимый разделитель
        }

        public enum TokenСlassifier
        {
            None,
            Addition,               //  Signseparator =>  +
            Subtraction,            //  Signseparator =>  -
            Multiplication,         //  Signseparator =>  *
            Division,               //  Signseparator =>  /
            Equal,                  //  Signseparator =>  =
            LeftRoundBracket,       //  Signseparator =>  (
            RightRoundBracket,      //  Signseparator =>  )
            Semicolon,              //  Signseparator =>  ;
            Comma,                  //  Signseparator =>  ,
            Dot,                    //  Signseparator =>  .
            LeftCurlyBracket,       //  Signseparator =>  {
            RightCurlyBracket,      //  Signseparator =>  }
            ExclamationMark,        //  Signseparator =>  !
            GreaterThan,            //  Signseparator =>  >
            LessThan,               //  Signseparator =>  <
            LogicalAnd,             //  Signseparator =>  &&
            LogicalOR,              //  Signseparator =>  ||
            Increment,              //  Signseparator =>  ++
            Decrement,              //  Signseparator =>  --
            GreaterOrEqualThan,     //  Signseparator =>  >=
            LessOrEqualThan,        //  Signseparator =>  <=
            LogicalNotEqual,        //  Signseparator =>  !=
            LogicalEqual,           //  Signseparator =>  ==
            Int,                    //  Int =>  int
            Double,                 //  Double =>  double
            String,                 //  String =>  string
            Bool,                   //  Bool =>  bool
            Consol,                 //  Keyword =>  consol
            Else,                   //  Keyword =>  else
            False,                  //  Bool =>  false
            If,                     //  Keyword =>  if
            Return,                 //  Keyword =>  return
            True,                   //  Bool =>  true
            While,                  //  Keyword =>  while
            Identity,               //  Identity
            Concatenation,          //  Concatenation
            Brackets                //  Signseparator "()"
        }

        public TokenType Type { get; private set; }
        public TokenСlassifier Сlassifier { get; set; }
        public int Position { get; private set; }
        public string Value { get; private set; }
        public Token(TokenType type, TokenСlassifier сlassifier,  int position, string value)
        {
            Type = type;
            Сlassifier = сlassifier;
            Position = position;
            Value = value;
        }
    }
}
