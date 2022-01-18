using System.Collections.Generic;
using TLP.Lexer_;

namespace TLP.Parser_
{
    public abstract class ASTNode
    {
        public Token Token { get; protected set; }
        public ASTNode(Token token) => Token = token;
    }

    // Базовый класс для всех операторов
    public abstract class OperatorASTNode : ASTNode
    {
        public OperatorASTNode(Token token) : base(token) { }
    }

    // LIST ASTNode
    public abstract class SyntaxFactorASTNode : OperatorASTNode
    {
        public List<ASTNode> ASTNodeList { get; protected set; }

        public SyntaxFactorASTNode(Token token, List<ASTNode> astnodelist) : base(token)
        {
            ASTNodeList = astnodelist;
        }
    }

    //****************************************************************************************
    // УЗЛЫ

    // Int узел дерева
    public class IntASTNode : ASTNode
    {
        public int Value => int.Parse(Token.Value);

        public IntASTNode(Token token) : base(token) { }
    }

    // Double узел дерева
    public class DoubleASTNode : ASTNode
    {
        public double Value => double.Parse(Token.Value);

        public DoubleASTNode(Token token) : base(token) { }
    }

    // Id узел дерева
    public class IdASTNode : ASTNode
    {
        public string Value => Token.Value;

        public IdASTNode(Token token) : base(token) { }
    }

    // String узел дерева
    public class StringASTNode : ASTNode
    {
        public string Value => Token.Value;

        public StringASTNode(Token token) : base(token) { }
    }

    // Bool узел дерева
    public class BoolASTNode : ASTNode
    {
        public bool Value => bool.Parse(Token.Value);

        public BoolASTNode(Token token) : base(token) { }
    }

    // EOE узел дерева
    public class EOEASTNode : ASTNode
    {
        public char Value => char.Parse(Token.Value);

        public EOEASTNode(Token token) : base(token) { }
    }

    // Signseparator узел дерева
    public class SignseparatorASTNode : ASTNode
    {
        public char Value => char.Parse(Token.Value);

        public SignseparatorASTNode(Token token) : base(token) { }
    }

    // Int Keyword узел дерева
    public class IntKeywordASTNode : ASTNode
    {
        public string Value => Token.Value;

        public IntKeywordASTNode(Token token) : base(token) { }
    }

    // Double Keyword узел дерева
    public class DoubleKeywordASTNode : ASTNode
    {
        public string Value => Token.Value;

        public DoubleKeywordASTNode(Token token) : base(token) { }
    }

    // String Keyword узел дерева
    public class StringKeywordASTNode : ASTNode
    {
        public string Value => Token.Value;

        public StringKeywordASTNode(Token token) : base(token) { }
    }
    
    // Keyword узел дерева
    public class KeywordASTNode_ : ASTNode
    {
        public string Value => Token.Value;

        public KeywordASTNode_(Token token) : base(token) { }
    }

    // Keyword_If узел дерева
    public class KeywordIfASTNode : ASTNode
    {
        public string Value => Token.Value;

        public KeywordIfASTNode(Token token) : base(token) { }
    }

    // Keyword_Else узел дерева
    public class KeywordElseASTNode : ASTNode
    {
        public string Value => Token.Value;

        public KeywordElseASTNode(Token token) : base(token) { }
    }

    // Bool Keyword узел дерева
    public class BoolKeywordASTNode : ASTNode
    {
        public string Value => Token.Value;

        public BoolKeywordASTNode(Token token) : base(token) { }
    }

    //****************************************************************************************
    // КЛЮЧЕВЫЕ СЛОВА

    // Keyword узел дерева
    public class KeywordASTNode : SyntaxFactorASTNode
    {
        public KeywordASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Keyword_While узел дерева
    public class WhileOperatorASTNode : SyntaxFactorASTNode
    {
        public WhileOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Keyword_If узел дерева
    public class IfOperatorASTNode : SyntaxFactorASTNode
    {
        public IfOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Keyword_Else узел дерева
    public class ElseOperatorASTNode : SyntaxFactorASTNode
    {
        public ElseOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Keyword_Consol узел дерева
    public class ConsolOperatorASTNode : SyntaxFactorASTNode
    {
        public ConsolOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Keyword_Return узел дерева
    public class KeywordReturnASTNode : SyntaxFactorASTNode
    {
        public KeywordReturnASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // ОПЕРАЦИИ

    // Приравнивание
    public class EqualOperatorASTNode : SyntaxFactorASTNode
    {
        public EqualOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Инкремент
    public class IncrOperatorASTNode : SyntaxFactorASTNode
    {
        public IncrOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Декремент
    public class DecOperatorASTNode : SyntaxFactorASTNode
    {
        public DecOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Сложение
    public class AdditionOperatorASTNode : SyntaxFactorASTNode
    {
        public AdditionOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Вычитание
    public class SubtractionOperatorASTNode : SyntaxFactorASTNode
    {
        public SubtractionOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Умножение
    public class MultiplicationOperatorASTNode : SyntaxFactorASTNode
    {
        public MultiplicationOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Деление
    public class DivisionOperatorASTNode : SyntaxFactorASTNode
    {
        public DivisionOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Скобки
    public class BracketsOperatorASTNode : SyntaxFactorASTNode
    {
        public BracketsOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Конкатенация
    public class ConcatenationOperatorASTNode : SyntaxFactorASTNode
    {
        public ConcatenationOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // <
    public class LessThanOperatorASTNode : SyntaxFactorASTNode
    {
        public LessThanOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // >
    public class GreaterThanOperatorASTNode : SyntaxFactorASTNode
    {
        public GreaterThanOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // <=
    public class LessOrEqualThanOperatorASTNode : SyntaxFactorASTNode
    {
        public LessOrEqualThanOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // >=
    public class GreaterOrEqualThanOperatorASTNode : SyntaxFactorASTNode
    {
        public GreaterOrEqualThanOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // &&
    public class LogicalAndOperatorASTNode : SyntaxFactorASTNode
    {
        public LogicalAndOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }    
    
    // !
    public class LogicalNotOperatorASTNode : SyntaxFactorASTNode
    {
        public LogicalNotOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // ||
    public class LogicalOROperatorASTNode : SyntaxFactorASTNode
    {
        public LogicalOROperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // ==
    public class LogicalEqualOperatorASTNode : SyntaxFactorASTNode
    {
        public LogicalEqualOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

    // Разделители
    // ,
    public class CommaOperatorASTNode : SyntaxFactorASTNode
    {
        public CommaOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }
    // (
    public class LeftRoundBracketOperatorASTNode : SyntaxFactorASTNode
    {
        public LeftRoundBracketOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }
    // )
    public class RightRoundBracketOperatorASTNode : SyntaxFactorASTNode
    {
        public RightRoundBracketOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }
    // {
    public class LeftCurlyBracketOperatorASTNode : SyntaxFactorASTNode
    {
        public LeftCurlyBracketOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }
    // }
    public class RightCurlyBracketOperatorASTNode : SyntaxFactorASTNode
    {
        public RightCurlyBracketOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }
    // id
    public class IdOperatorASTNode : SyntaxFactorASTNode
    {
        public IdOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }
    // SyntaxExprOperatorASTNode
    public class SyntaxExprOperatorASTNode : SyntaxFactorASTNode
    {
        public SyntaxExprOperatorASTNode(Token token, List<ASTNode> astnode) : base(token, astnode) { }
    }

}
