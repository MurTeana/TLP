using System;
using System.Collections.Generic;
using System.Text;

namespace TLP.Lexer_
{
    public class Lexer
    {
        static readonly Dictionary<string, Func<int, string, Token>> SignseparatorMap = new Dictionary<string, Func<int, string, Token>> {
            { "+", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.Addition, p,v.ToString()) },
            { "-", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.Subtraction, p,v.ToString()) },
            { "*", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.Multiplication, p,v.ToString()) },
            { "/", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.Division, p,v.ToString()) },
            { "=", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.Equal, p,v.ToString()) },
            { "(", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.LeftRoundBracket, p,v.ToString()) },
            { ")", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.RightRoundBracket, p,v.ToString()) },
            { ";", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.Semicolon, p,v.ToString()) },
            { ",", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.Comma, p,v.ToString()) },
            { ".", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.Dot, p,v.ToString()) },
            { "{", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.LeftCurlyBracket, p,v.ToString()) },
            { "}", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.RightCurlyBracket, p,v.ToString()) },
            { "!", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.ExclamationMark, p,v.ToString()) },
            { ">", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.GreaterThan, p,v.ToString()) },
            { "<", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.LessThan, p,v.ToString()) },
            { "&&", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.LogicalAnd, p,v.ToString()) },
            { "||", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.LogicalOR, p,v.ToString()) },
            { "++", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.Increment, p,v.ToString()) },
            { "--", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.Decrement, p,v.ToString()) },
            { ">=", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.GreaterOrEqualThan, p,v.ToString()) },
            { "<=", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.LessOrEqualThan, p,v.ToString()) },
            { "!=", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.LogicalNotEqual, p,v.ToString()) },
            { "==", (p,v) => new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.LogicalEqual, p,v.ToString()) }
        };
        static readonly Dictionary<string, Func<int, string, Token>> KeywordMap = new Dictionary<string, Func<int, string, Token>> {
            { "int", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.Int, p,v.ToString()) },
            { "double", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.Double, p,v.ToString()) },
            { "bool", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.Bool, p,v.ToString()) },
            { "string", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.String, p,v.ToString()) },
            { "return", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.Return, p,v.ToString()) },
            { "if", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.If, p,v.ToString()) },
            { "else", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.Else, p,v.ToString()) },
            { "consol", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.Consol, p,v.ToString()) },
            { "while", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.While, p,v.ToString()) },
            { "true", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.True, p,v.ToString()) },
            { "false", (p,v) => new Token(Token.TokenType.Keyword, Token.TokenСlassifier.False, p,v.ToString())}
        };        
        static readonly Dictionary<string, Func<int, string, Token>> BoolMap = new Dictionary<string, Func<int, string, Token>> {
            { "bool", (p,v) => new Token(Token.TokenType.Bool, Token.TokenСlassifier.Bool, p,v.ToString()) },
            { "true", (p,v) => new Token(Token.TokenType.Bool, Token.TokenСlassifier.True, p,v.ToString()) },
            { "false", (p,v) => new Token(Token.TokenType.Bool, Token.TokenСlassifier.False, p,v.ToString()) }
        };

        string[] keywords =
        {
            "int",
            "double",
            "string",
            "bool",
            "consol",
            "else",
            "false",
            "if",
            "return",
            "true",
            "while"
        };
        
        string[] signseparators =
        {
            "+",
            "-",
            "*",
            "/",
            "=",
            "(",
            ")",
            ";",
            ",",
            ".",
            "{",
            "}",
            "!",
            ">",
            "<",
            "&&",
            "||",
            "--",
            "++",
            ">=",
            "<=",
            "!=",
            "=="
        };

        List<string> identity = new List<string>();

        

        readonly SourceScanner _scanner;
        public int Position => _scanner.Position;
        public Lexer(SourceScanner scanner) => _scanner = scanner;

        // Функция лексического анализатора
        public Token ReadNext()
        {
            if (_scanner.EndOfSource)
                return new Token(Token.TokenType.EOE, Token.TokenСlassifier.None, _scanner.Position, null);

            ConsumeWhiteSpace();

            Token token;

            if (TryTokenizeIntORDouble(out token))
                return token;

            if (TryTokenizeKeywordORIdentity(out token))                 
                return token;              

            if (TryTokenizeString(out token))
                return token;

            if (TryTokenizeSignseparator(out token))
                return token;

            // Ошибка
            throw new Exception($"Error parsing expression at {_scanner.Position} value of {_scanner.Peek()}");
        }

        // Функции распознавания токена
        private bool TryTokenizeIntORDouble(out Token token)
        {
            token = null;
            var sb = new StringBuilder();

            if (isDigit_(_scanner.Peek()) || isPoint_(_scanner.Peek()))
            { 
                if (isDigit_(_scanner.Peek()))
                {
                    var position = _scanner.Position;               

                    while (isDigit_(_scanner.Peek()))
                    {
                        sb.Append(_scanner.Read().Value);
                    }

                    token = new Token(Token.TokenType.Int, Token.TokenСlassifier.Int, position, sb.ToString());

                    if (isComma_(_scanner.Peek()))
                    {
                        sb.Append(_scanner.Read().Value);

                        while (isDigit_(_scanner.Peek()))
                        {
                            sb.Append(_scanner.Read().Value);
                        }

                        token = new Token(Token.TokenType.Double, Token.TokenСlassifier.Double, position, sb.ToString());
                    }
                }
            }

            return token != null;
        }
        private bool TryTokenizeKeywordORIdentity(out Token token)
        {
            token = null;
            var sb = new StringBuilder();

            if (isLetter_(_scanner.Peek()) || isLowbar_(_scanner.Peek()) || isDigit_(_scanner.Peek()))
            {
                var position = _scanner.Position;              

                while (isLetter_(_scanner.Peek()) || isLowbar_(_scanner.Peek()) || isDigit_(_scanner.Peek()))
                {
                    sb.Append(_scanner.Read().Value);
                }

                token = new Token(Token.TokenType.Identity, Token.TokenСlassifier.Identity, position, sb.ToString());

                for (int i = 0; i < keywords.Length; i++)
                {
                    if (sb.ToString() == keywords[i])
                        //token = new Token(Token.TokenType.Keyword, Token.TokenСlassifier.None, position, sb.ToString());
                        token = KeywordMap[sb.ToString()](position, sb.ToString());
                }
            }

            addToIdentityList(token);

            return token != null;
        }
        private bool TryTokenizeString(out Token token)
        {
            token = null;

            if (isQuatationMark_(_scanner.Peek()))
            {
                _scanner.Read();
                var position = _scanner.Position;
                var sb = new StringBuilder();
                
                while (true)
                {
                    sb.Append(_scanner.Read().Value);
                    if (isQuatationMark_(_scanner.Peek()))
                    {
                        _scanner.Read();
                        token = new Token(Token.TokenType.String, Token.TokenСlassifier.String, position, sb.ToString());
                        return token != null;
                    }
                }                 
            }

            return token != null;
        }
        private bool TryTokenizeSignseparator(out Token token)
        {
            token = null;

            if (isSign_(_scanner.Peek()))
            {
                var position = _scanner.Position;
                var sb = new StringBuilder();

                while (isSign_(_scanner.Peek()))
                {
                    sb.Append(_scanner.Read().Value);

                    if (isSignseparator_(sb.ToString()))
                    {
                        char? val = _scanner.Peek();
                        if (isSign_(val))
                        {
                            sb.Append(val.Value);

                            if (isSignseparator_(sb.ToString()))
                            {
                                _scanner.Read();
                                token = SignseparatorMap[sb.ToString()](position, sb.ToString());
                                //token = new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.None, position, sb.ToString());
                                return token != null;
                            }
                            else
                            {
                                sb.Remove(sb.Length - 1,1);
                            }
                        }

                        //token = new Token(Token.TokenType.Signseparator, Token.TokenСlassifier.None, position, sb.ToString());
                        token = SignseparatorMap[sb.ToString()](position, sb.ToString());

                        return token != null;
                    }                   
                }
            }

            return token != null;
        }
        private void ConsumeWhiteSpace()
        {
            while (isWhiteSpace_(_scanner.Peek()))
                _scanner.Read();   //eat the whitespace
        }

        // Функции проверки символа
        private static bool isDigit_(char? currsymb)
        {
            bool state = ((currsymb.HasValue) && Char.IsDigit(currsymb.Value));
            return state;
        }
        private static bool isPoint_(char? currsymb)
        {
            bool state = ((currsymb.HasValue) && ((currsymb.Value) == '.'));
            return state;
        }
        private static bool isComma_(char? currsymb)
        {
            bool state = ((currsymb.HasValue) && ((currsymb.Value) == ','));
            return state;
        }
        private static bool isLetter_(char? currsymb)
        {
            bool state = (currsymb.HasValue) && (Char.IsLetter(currsymb.Value));
            return state;
        }
        private static bool isWhiteSpace_(char? currsymb)
        {
            bool state = ((currsymb.HasValue) && Char.IsWhiteSpace(currsymb.Value));
            return state;
        }      
        private static bool isLowbar_(char? currsymb)
        {
            bool state = ((currsymb.HasValue) && ((currsymb.Value) == '_'));
            return state;
        }
        private bool isQuatationMark_(char? currsymb)
        {
            bool state = ((currsymb.HasValue) && ((currsymb.Value) == '"'));
            return state;
        }
        private bool isSign_(char? currsymb)
        {
            bool state = false;

            string[] signs =
        {
            "+",
            "-",
            "*",
            "/",
            "=",
            "(",
            ")",
            ";",
            ",",
            ".",
            "{",
            "}",
            "!",
            ">",
            "<",
            "&",
            "|",
        };

            for (int i = 0; i < signs.Length; i++)
            {
                if (((currsymb.HasValue) && ((currsymb.Value) == Convert.ToChar(signs[i]))))
                {
                    state = true;
                    return state;
                }             
            }

            return state;
        }
        private bool isSignseparator_(string currToken)
        {
            bool state = false;

            for (int i = 0; i < signseparators.Length; i++)
            {
                if (((currToken.Length > 0) && (currToken == (signseparators[i]))))
                {
                    state = true;
                    return state;
                }
            }

            return state;
        }

        // Добавление в список и Вывод на печать списка идентификаторов
        private void addToIdentityList(Token token)
        {
            if ((token != null) && (token.Type == Token.TokenType.Identity))
            {
                bool state = false;

                for (int i = 0; i < identity.Count; i++)
                {
                    if (token.Value == identity[i])
                        state = true;
                }

                if (!state)
                    identity.Add(token.Value);                    
            }
        }
        public void printidentity()
        {
            Console.WriteLine("\n");

            for (int i = 0; i < identity.Count; i++)
                Console.WriteLine("Идентификатор {0} равен {1}", i, identity[i]);
        }

        public void Accept() => ReadNext();

        // Посмотреть следующий токен
        public Token Peek()
        {
            _scanner.Push();
            var token = ReadNext();
            _scanner.Pop();
            return token;
        }

        // Посмотреть токен с позицией n

        public Token PeekN(int n)
        {
            Token token = null;

            for (int i = 0; i < n; i++)
            {
                _scanner.Push();
                token = ReadNext();

            }

            for (int i = 0; i < n; i++)
            {
                _scanner.Pop();
            }

            return token;
        }

    }
}
