using System;
using TLP.Lexer_;
using System.Collections.Generic;

namespace TLP.Parser_
{
    public class Parser
    {
        public static ASTNode Parse(string expression)
        {
            Lexer lexer = new Lexer(new SourceScanner(expression));
            return Parse_program(lexer);
        }

        // PROGRAM
        public static ASTNode Parse_program(Lexer lexer)
        {
            Token token = (Parse_prog_name(lexer)).Token;

            ASTNode LeftRoundBracketASTNode = null;
            ASTNode RightRoundBracketASTNode = null;
            ASTNode LeftCurlyBracketASTNode = null;
            ASTNode ProgramBodyASTNode = null;
            ASTNode RightCurlyBracketASTNode = null;

            Token peekToken = lexer.Peek();         

            if (peekToken.Сlassifier == Token.TokenСlassifier.LeftRoundBracket)
                LeftRoundBracketASTNode = Parse_Signseparator(lexer);

            if (peekToken == null)
                throw new Exception($"Invalid Expression. LeftRoundBracket Expected at position {lexer.Position}");

            peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.RightRoundBracket)
                RightRoundBracketASTNode = Parse_Signseparator(lexer);

            if (peekToken == null)
                throw new Exception($"Invalid Expression. RightRoundBracket Expected at position {lexer.Position}");

            peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.LeftCurlyBracket)
                LeftCurlyBracketASTNode = Parse_Signseparator(lexer);

            if (peekToken == null)
                throw new Exception($"Invalid Expression. LeftCurlyBracket Expected at position {lexer.Position}");

            peekToken = lexer.Peek();
            Token peekToken2 = lexer.PeekN(2);

            while (peekToken2.Type != Token.TokenType.EOE)
            {
                ProgramBodyASTNode = Parse_progbody(lexer);

                peekToken = lexer.Peek();
                peekToken2 = lexer.PeekN(2);
            }

            if (peekToken.Сlassifier == Token.TokenСlassifier.RightCurlyBracket)
                RightCurlyBracketASTNode = Parse_Signseparator(lexer);

            if (peekToken == null)
                throw new Exception($"Invalid Expression. RightCurlyBracket Expected at position {lexer.Position}");

            List<ASTNode> astnodelist = new List<ASTNode>() { LeftRoundBracketASTNode, RightRoundBracketASTNode, LeftCurlyBracketASTNode, ProgramBodyASTNode, RightCurlyBracketASTNode };

            ASTNode node = CreateSyntaxFactor(token, astnodelist);

            return node;
        }
        public static ASTNode Parse_prog_name(Lexer lexer)
        {
            return Parse_Id(lexer);
        }
        public static ASTNode Parse_progbody(Lexer lexer)
        {
            var peekToken = lexer.Peek();

            if (peekToken.Type == Token.TokenType.Keyword &&
                (peekToken.Сlassifier == Token.TokenСlassifier.Int ||
                peekToken.Сlassifier == Token.TokenСlassifier.Double ||
                peekToken.Сlassifier == Token.TokenСlassifier.Bool ||
                peekToken.Сlassifier == Token.TokenСlassifier.String))
                return Parse_define_var_list(lexer);
            else
                return Parse_statement_list(lexer);
        }

        // DEFINE VAR
        private static ASTNode Parse_define_var_list(Lexer lexer)
        {
            Token token = new Token(Token.TokenType.None, Token.TokenСlassifier.None, -1, "define_var_list");

            List<ASTNode> DefineVarAstNodeList = new List<ASTNode>();

            var peekToken = lexer.Peek();

            while (peekToken.Сlassifier != Token.TokenСlassifier.RightCurlyBracket)
            {
                ASTNode DefineVarAstNode = Parse_define_var(lexer);

                if (DefineVarAstNode == null)
                    throw new Exception($"Invalid Expression. DEFINEVAR Expected at position {lexer.Position}");

                DefineVarAstNodeList.Add(DefineVarAstNode);

                peekToken = lexer.Peek();
            }

            ASTNode node = CreateSyntaxFactor(token, DefineVarAstNodeList);

            return node;
        }
        private static ASTNode Parse_define_var(Lexer lexer)
        {
            Token token = new Token(Token.TokenType.None, Token.TokenСlassifier.None, -1, "define_var");
            //List<ASTNode> astnodelist = new List<ASTNode>();

            ASTNode TypeASTNode = Parse_type(lexer);

            if (TypeASTNode == null)
                throw new Exception($"Invalid Expression. VAR TYPE Expected at position {lexer.Position}");

            List<ASTNode> astnodelist = new List<ASTNode>() { TypeASTNode };

            ASTNode IdListASTNode = null;
            ASTNode SemicolonASTNode = null;

            var peekToken = lexer.Peek();

            if (peekToken.Сlassifier != Token.TokenСlassifier.RightCurlyBracket)
            {
                IdListASTNode = Parse_id_list(lexer);

                if (IdListASTNode == null)
                    throw new Exception($"Invalid Expression. ID LIST Expected at position {lexer.Position}");

                astnodelist.Add(IdListASTNode);
            }
           
            peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.Semicolon)
            {
                SemicolonASTNode = Parse_Signseparator(lexer);
                astnodelist.Add(SemicolonASTNode);
            }

            var node = CreateSyntaxFactor(token, astnodelist);

            return node;
        }
        private static ASTNode Parse_id_list(Lexer lexer)
        {
            List<ASTNode> astnodelist = new List<ASTNode>();

            Token token = new Token(Token.TokenType.None, Token.TokenСlassifier.None, -1, "");

            var node = Parse_Id(lexer);
            astnodelist.Add(node);

            var peekToken = lexer.Peek();

            while (peekToken.Сlassifier == Token.TokenСlassifier.Comma)
            {
                var operator_ = lexer.ReadNext();
                astnodelist.Add(new SignseparatorASTNode(operator_));

                var nextASTNode = Parse_Id(lexer);
                astnodelist.Add(nextASTNode);
                peekToken = lexer.Peek();
            }

            if (peekToken.Сlassifier == Token.TokenСlassifier.Comma)
            {
                var nextASTNode = Parse_Signseparator(lexer);
                astnodelist.Add(nextASTNode);
            }

            node = CreateSyntaxFactor(token, astnodelist);

            return node;
        }
        private static ASTNode Parse_type(Lexer lexer)
        {
            var peekToken = lexer.Peek();

            if (peekToken.Type == Token.TokenType.Keyword &&
                peekToken.Сlassifier == Token.TokenСlassifier.Int)
                return Parse_Keyword_Int(lexer);

            if (peekToken.Type == Token.TokenType.Keyword &&
                peekToken.Сlassifier == Token.TokenСlassifier.Double)
                return Parse_Keyword_Double(lexer);

            if (peekToken.Type == Token.TokenType.Keyword &&
                peekToken.Сlassifier == Token.TokenСlassifier.String)
                return Parse_Keyword_String(lexer);

            if (peekToken.Type == Token.TokenType.Keyword &&
                peekToken.Сlassifier == Token.TokenСlassifier.Bool)
                return Parse_Keyword_Bool(lexer);

            if (peekToken.Type == Token.TokenType.EOE)
                return Parse_EOE(lexer);

            return Parse_statement_list(lexer);
        }

        // STATEMENT
        private static ASTNode Parse_statement_list(Lexer lexer)
        {
            List<ASTNode> astnodelist = new List<ASTNode>();
            Token token = new Token(Token.TokenType.None, Token.TokenСlassifier.None, -1, "");
            var node = Parse_statement(lexer);
            astnodelist.Add(node);

            var peekToken = lexer.Peek();

            while (peekToken.Сlassifier != Token.TokenСlassifier.RightCurlyBracket)
            {
                var nextASTNode = Parse_statement(lexer);

                if (nextASTNode == null)
                    throw new Exception($"Invalid Expression. STATEMENT Expected at position {lexer.Position}");

                astnodelist.Add(nextASTNode);

                peekToken = lexer.Peek();
            }

            node = CreateSyntaxFactor(token, astnodelist);

            return node;
        }
        public static ASTNode Parse_statement(Lexer lexer)
        {
            var peekToken = lexer.Peek();

            if (peekToken.Type == Token.TokenType.Keyword &&
                (peekToken.Value == "if"))
                return Parse_if(lexer);
            if (peekToken.Type == Token.TokenType.Keyword &&
                (peekToken.Value == "consol"))
                return Parse_consol(lexer);
            if (peekToken.Type == Token.TokenType.Keyword &&
                (peekToken.Value == "while"))
                return Parse_while(lexer);
            if (peekToken.Type == Token.TokenType.Keyword &&
                (peekToken.Value == "return"))
                return Parse_return(lexer);
            if (peekToken.Type == Token.TokenType.Identity)
                return Parse_assign_exp(lexer);
            if (peekToken.Type == Token.TokenType.EOE)
                return Parse_EOE(lexer);

            throw new Exception($"Invalid Expression.  STATEMENT Expected at position {lexer.Position}");
        }

        // KEYWORDS - RETURN/WHILE/CONSOL/IF
        public static ASTNode Parse_return(Lexer lexer)
        {
            List<ASTNode> astnodelist = new List<ASTNode>();
            var token = Parse_Keyword(lexer);

            var peekToken = lexer.Peek();

            while (peekToken.Сlassifier != Token.TokenСlassifier.Semicolon)
            {
                var nextASTNode = Parse_exp(lexer);
                astnodelist.Add(nextASTNode);
                peekToken = lexer.Peek();
            }

            if (peekToken.Сlassifier == Token.TokenСlassifier.Semicolon)
            {
                var nextASTNode = Parse_Signseparator(lexer);
                astnodelist.Add(nextASTNode);
            }

            var node = CreateSyntaxFactor(token, astnodelist);
            return node;
        }
        public static ASTNode Parse_while(Lexer lexer)
        {
            List<ASTNode> astnodelist = new List<ASTNode>();
            ASTNode nextASTNode;
            var token = Parse_Keyword(lexer);

            var peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.LeftRoundBracket)
            {
                nextASTNode = Parse_Signseparator(lexer);
                astnodelist.Add(nextASTNode);

                while (peekToken.Сlassifier != Token.TokenСlassifier.RightRoundBracket)
                {
                    nextASTNode = Parse_condition(lexer);

                    if (nextASTNode == null)
                        throw new Exception($"Invalid Expression. CONDITION Expected at position {lexer.Position}");

                    astnodelist.Add(nextASTNode);

                    peekToken = lexer.Peek();
                }
            }

            if (peekToken.Сlassifier == Token.TokenСlassifier.RightRoundBracket)
            {
                nextASTNode = Parse_Signseparator(lexer);
                astnodelist.Add(nextASTNode);
            }

            peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.LeftCurlyBracket)
            {
                nextASTNode = Parse_Signseparator(lexer);
                astnodelist.Add(nextASTNode);

                while (peekToken.Сlassifier != Token.TokenСlassifier.RightCurlyBracket)
                {
                    nextASTNode = Parse_statement_list(lexer);

                    if (nextASTNode == null)
                        throw new Exception($"Invalid Expression. STATEMENT_LIST Expected at position {lexer.Position}");

                    astnodelist.Add(nextASTNode);

                    peekToken = lexer.Peek();
                }
            }

            if (peekToken.Сlassifier == Token.TokenСlassifier.RightCurlyBracket)
            {
                nextASTNode = Parse_Signseparator(lexer);
                astnodelist.Add(nextASTNode);
            }

            var node = CreateSyntaxFactor(token, astnodelist);
            return node;
        }
        public static ASTNode Parse_consol(Lexer lexer)
        {
            var token = Parse_Keyword_Consol(lexer);

            ASTNode LeftRoundBracketASTNode = null;
            ASTNode TextASTNode = null;
            ASTNode CommaASTNode = null;
            ASTNode ValueASTNode = null;
            ASTNode RightRoundBracketASTNode = null;
            ASTNode SemicolonASTNode = null;

            var peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.LeftRoundBracket)
            {
                LeftRoundBracketASTNode = Parse_Signseparator(lexer);
            }

            peekToken = lexer.Peek();

            while (peekToken.Сlassifier != Token.TokenСlassifier.Comma)
            {
                TextASTNode = Parse_exprsch(lexer);
                peekToken = lexer.Peek();
            }

            if (peekToken.Сlassifier == Token.TokenСlassifier.Comma)
            {
                CommaASTNode = Parse_Signseparator(lexer);
            }

            peekToken = lexer.Peek();

            while (peekToken.Сlassifier != Token.TokenСlassifier.RightRoundBracket)
            {
                ValueASTNode = Parse_exp(lexer);
                peekToken = lexer.Peek();
            }
           
            if (peekToken.Сlassifier == Token.TokenСlassifier.RightRoundBracket)
            {
                RightRoundBracketASTNode = Parse_Signseparator(lexer);
            }

            peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.Semicolon)
            {
                SemicolonASTNode = Parse_Signseparator(lexer);
            }

            List<ASTNode> astnodelist = new List<ASTNode>() { LeftRoundBracketASTNode, TextASTNode, CommaASTNode, ValueASTNode, RightRoundBracketASTNode, SemicolonASTNode};

            var node = CreateSyntaxFactor(token, astnodelist);

            return node;
        }
        public static ASTNode Parse_if(Lexer lexer)
        {
            var token = Parse_Keyword_If(lexer);

            ASTNode LeftRoundBracketASTNode = null;
            ASTNode ConditionASTNode = null;
            ASTNode RightRoundBracketASTNode = null;
            ASTNode LeftCurlyBracketASTNode = null;
            ASTNode StatementListASTNode = null;
            ASTNode RightCurlyBracketASTNode = null;
            ASTNode ElseASTNode = null;

            var peekToken = lexer.Peek();
            
            if (peekToken.Сlassifier == Token.TokenСlassifier.LeftRoundBracket)
            {
                LeftRoundBracketASTNode = Parse_Signseparator(lexer);

                while (peekToken.Сlassifier != Token.TokenСlassifier.RightRoundBracket)
                {
                    ConditionASTNode = Parse_condition(lexer);

                    if (ConditionASTNode == null)
                        throw new Exception($"Invalid Expression. CONDITION Expected at position {lexer.Position}");

                    peekToken = lexer.Peek();
                }
            }

            if (peekToken.Сlassifier == Token.TokenСlassifier.RightRoundBracket)
            {
                RightRoundBracketASTNode = Parse_Signseparator(lexer);
            }

            peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.LeftCurlyBracket)
            {
                LeftCurlyBracketASTNode = Parse_Signseparator(lexer);

                while (peekToken.Сlassifier != Token.TokenСlassifier.RightCurlyBracket)
                {
                    StatementListASTNode = Parse_statement_list(lexer);

                    if (StatementListASTNode == null)
                        throw new Exception($"Invalid Expression. STATEMENT_LIST Expected at position {lexer.Position}");

                    peekToken = lexer.Peek();
                }
            }

            if (peekToken.Сlassifier == Token.TokenСlassifier.RightCurlyBracket)
            {
                RightCurlyBracketASTNode = Parse_Signseparator(lexer);
            }
            //
            peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.Else)
            {
                ElseASTNode = Parse_else(lexer);
            }

            List<ASTNode> astnodelist = new List<ASTNode>() { LeftRoundBracketASTNode, ConditionASTNode, RightRoundBracketASTNode, LeftCurlyBracketASTNode, StatementListASTNode, RightCurlyBracketASTNode, ElseASTNode};

            var IfASTNode = CreateSyntaxFactor(token, astnodelist);

            return IfASTNode;
        }
        public static ASTNode Parse_else(Lexer lexer)
        {
            var token = Parse_Keyword_Else(lexer);

            ASTNode LeftCurlyBracketASTNode = null;
            ASTNode StatementListASTNode = null;
            ASTNode RightCurlyBracketASTNode = null;

            var peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.LeftCurlyBracket)
            {
                LeftCurlyBracketASTNode = Parse_Signseparator(lexer);

                while (peekToken.Сlassifier != Token.TokenСlassifier.RightCurlyBracket)
                {
                    StatementListASTNode = Parse_statement_list(lexer);

                    if (StatementListASTNode == null)
                        throw new Exception($"Invalid Expression. STATEMENT_LIST Expected at position {lexer.Position}");

                    peekToken = lexer.Peek();
                }
            }

            if (peekToken.Сlassifier == Token.TokenСlassifier.RightCurlyBracket)
            {
                RightCurlyBracketASTNode = Parse_Signseparator(lexer);
            }

            List<ASTNode> astnodelist = new List<ASTNode>() { LeftCurlyBracketASTNode, StatementListASTNode, RightCurlyBracketASTNode };

            var ElseASTNode = CreateSyntaxFactor(token, astnodelist);

            return ElseASTNode;
        }

        // ASSIGN
        public static ASTNode Parse_assign_exp(Lexer lexer)
        {
            List<ASTNode> astnodelist = new List<ASTNode>();
            var node = Parse_Id(lexer);
            astnodelist.Add(node);

            var peekToken = lexer.Peek();

            if (peekToken.Сlassifier == Token.TokenСlassifier.Equal)
            {
                var operator_ = lexer.ReadNext();

                var nextpeekToken = lexer.Peek();

                while (nextpeekToken.Сlassifier != Token.TokenСlassifier.Semicolon)
                {
                    var nextASTNode = Parse_exp(lexer);
                    astnodelist.Add(nextASTNode);
                    nextpeekToken = lexer.Peek();
                }

                if (nextpeekToken.Сlassifier == Token.TokenСlassifier.Semicolon)
                {
                    var nextASTNode = Parse_Signseparator(lexer);
                    astnodelist.Add(nextASTNode);
                }

                node = CreateSyntaxFactor(operator_, astnodelist);
            }

            return node;
        }

        // EXP
        public static ASTNode Parse_exp(Lexer lexer)
        {
            var peekToken = lexer.Peek();
            var peekToken2 = lexer.PeekN(2);
            var peekToken3 = lexer.PeekN(3);

            if (
                (
                (peekToken.Type == Token.TokenType.Int ||
                peekToken.Type == Token.TokenType.Double ||
                peekToken.Type == Token.TokenType.Identity ||
                peekToken.Сlassifier == Token.TokenСlassifier.LeftRoundBracket) &&
                (peekToken2.Сlassifier != Token.TokenСlassifier.LogicalAnd &&
                peekToken2.Сlassifier != Token.TokenСlassifier.LogicalEqual &&
                peekToken2.Сlassifier != Token.TokenСlassifier.LogicalNotEqual &&
                peekToken2.Сlassifier != Token.TokenСlassifier.GreaterThan &&
                peekToken2.Сlassifier != Token.TokenСlassifier.LessThan &&
                peekToken2.Сlassifier != Token.TokenСlassifier.GreaterOrEqualThan &&
                peekToken2.Сlassifier != Token.TokenСlassifier.LessOrEqualThan)
                )
                ||
                (
                (peekToken.Type == Token.TokenType.Int ||
                peekToken.Type == Token.TokenType.Double ||
                peekToken.Type == Token.TokenType.Identity) &&
                (peekToken2.Сlassifier == Token.TokenСlassifier.Addition ||
                peekToken2.Сlassifier == Token.TokenСlassifier.Subtraction ||
                peekToken2.Сlassifier == Token.TokenСlassifier.Multiplication ||
                peekToken2.Сlassifier == Token.TokenСlassifier.Division)
                )
                )
                return Parse_exprcalc(lexer);

            if (
                (peekToken.Type == Token.TokenType.String ||
                peekToken.Type == Token.TokenType.Identity)
                &&
                (peekToken2.Сlassifier != Token.TokenСlassifier.LogicalAnd &&
                peekToken2.Сlassifier != Token.TokenСlassifier.LogicalEqual &&
                peekToken2.Сlassifier != Token.TokenСlassifier.LogicalNotEqual &&
                peekToken2.Сlassifier != Token.TokenСlassifier.GreaterThan &&
                peekToken2.Сlassifier != Token.TokenСlassifier.LessThan &&
                peekToken2.Сlassifier != Token.TokenСlassifier.GreaterOrEqualThan &&
                peekToken2.Сlassifier != Token.TokenСlassifier.LessOrEqualThan)
                ||
                ((peekToken.Type == Token.TokenType.String ||
                peekToken.Type == Token.TokenType.Identity) &&
                (peekToken2.Сlassifier == Token.TokenСlassifier.Addition)))
                return Parse_exprstring(lexer);

            if ((
                peekToken.Сlassifier == Token.TokenСlassifier.True ||
                peekToken.Сlassifier == Token.TokenСlassifier.False)
                ||
                (
                (peekToken.Сlassifier == Token.TokenСlassifier.True ||
                peekToken.Сlassifier == Token.TokenСlassifier.False ||
                peekToken.Сlassifier == Token.TokenСlassifier.Int ||
                peekToken.Сlassifier == Token.TokenСlassifier.Double ||
                peekToken.Сlassifier == Token.TokenСlassifier.String ||
                peekToken.Сlassifier == Token.TokenСlassifier.Identity)
                &&
                (peekToken2.Сlassifier == Token.TokenСlassifier.LogicalAnd) ||
                peekToken2.Сlassifier == Token.TokenСlassifier.LogicalEqual ||
                peekToken2.Сlassifier == Token.TokenСlassifier.LogicalNotEqual ||
                peekToken2.Сlassifier == Token.TokenСlassifier.GreaterThan ||
                peekToken2.Сlassifier == Token.TokenСlassifier.LessThan ||
                peekToken2.Сlassifier == Token.TokenСlassifier.GreaterOrEqualThan ||
                peekToken2.Сlassifier == Token.TokenСlassifier.LessOrEqualThan)
                )
                return Parse_condition(lexer);

            if (peekToken.Сlassifier == Token.TokenСlassifier.ExclamationMark)
                return Parse_not(lexer);

            if (peekToken.Type == Token.TokenType.EOE)
                return Parse_EOE(lexer);

            throw new Exception($"Invalid Expression.  EXP Expected at position {lexer.Position}");
        }

        // CONDITION
        public static ASTNode Parse_condition(Lexer lexer)
        {           
            var leftASTNode = Parse_condition_exp1(lexer);

            var peekToken = lexer.Peek();

            while (peekToken.Сlassifier == Token.TokenСlassifier.LogicalOR)
            {

                var operator_ = lexer.ReadNext();

                var rightASTNode = Parse_condition_exp1(lexer);

                if (rightASTNode == null)
                    throw new Exception($"Invalid Expression. CONDITION_EXP1 Expected at position {lexer.Position}");

                List<ASTNode> astnodelist = new List<ASTNode>() { leftASTNode, rightASTNode };

                leftASTNode = CreateSyntaxFactor(operator_, astnodelist);

                peekToken = lexer.Peek();
            }

            return leftASTNode;
        }
        public static ASTNode Parse_condition_exp1(Lexer lexer)
        {
            var leftASTNode = Parse_condition_exp2(lexer);

            var peekToken = lexer.Peek();

            while (peekToken.Сlassifier == Token.TokenСlassifier.LogicalAnd)
            {

                var operator_ = lexer.ReadNext();

                var rightASTNode = Parse_condition_exp2(lexer);

                if (rightASTNode == null)
                    throw new Exception($"Invalid Expression. CONDITION_EXP2 Expected at position {lexer.Position}");

                List<ASTNode> astnodelist = new List<ASTNode>() { leftASTNode, rightASTNode };

                leftASTNode = CreateSyntaxFactor(operator_, astnodelist);

                peekToken = lexer.Peek();
            }

            return leftASTNode;
        }
        public static ASTNode Parse_condition_exp2(Lexer lexer)
        {
            var leftASTNode = Parse_condition_exp3(lexer);

            var peekToken = lexer.Peek();

            while (peekToken.Сlassifier == Token.TokenСlassifier.LogicalEqual ||
                peekToken.Сlassifier == Token.TokenСlassifier.LogicalNotEqual)
            {
                var operator_ = lexer.ReadNext();

                var rightASTNode = Parse_condition_exp3(lexer);

                if (rightASTNode == null)
                    throw new Exception($"Invalid Expression. CONDITION_EXP3 Expected at position {lexer.Position}");

                List<ASTNode> astnodelist = new List<ASTNode>() { leftASTNode, rightASTNode };

                leftASTNode = CreateSyntaxFactor(operator_, astnodelist);

                peekToken = lexer.Peek();
            }

            return leftASTNode;
        }
        public static ASTNode Parse_condition_exp3(Lexer lexer)
        {
            var leftASTNode = Parse_boolstate(lexer);

            var peekToken = lexer.Peek();

            while (peekToken.Сlassifier == Token.TokenСlassifier.GreaterThan ||
                    peekToken.Сlassifier == Token.TokenСlassifier.LessThan ||
                    peekToken.Сlassifier == Token.TokenСlassifier.GreaterOrEqualThan ||
                    peekToken.Сlassifier == Token.TokenСlassifier.LessOrEqualThan)
            {
                var operator_ = lexer.ReadNext();

                var rightASTNode = Parse_boolstate(lexer);

                if (rightASTNode == null)
                    throw new Exception($"Invalid Expression. BOOLSTATE Expected at position {lexer.Position}");

                List<ASTNode> astnodelist = new List<ASTNode>() { leftASTNode, rightASTNode };

                leftASTNode = CreateSyntaxFactor(operator_, astnodelist);

                peekToken = lexer.Peek();
            }

            return leftASTNode;
        }
        public static ASTNode Parse_boolstate(Lexer lexer)
        {
            var peekToken = lexer.Peek();
            var peekToken2 = lexer.PeekN(2);

            if ((peekToken.Type == Token.TokenType.Keyword && 
                (peekToken.Value == "true" ||
                peekToken.Value == "false")) || 
                ((peekToken.Сlassifier == Token.TokenСlassifier.ExclamationMark) && 
                (peekToken2.Type == Token.TokenType.Keyword &&
                (peekToken2.Value == "true" ||
                peekToken2.Value == "false" || 
                peekToken2.Type == Token.TokenType.Identity))))
                return Parse_Bool(lexer);
            if (peekToken.Type == Token.TokenType.Identity)
                return Parse_Id(lexer);
            if (peekToken.Type == Token.TokenType.Int)
                return Parse_Int(lexer);
            if (peekToken.Type == Token.TokenType.Double)
                return Parse_Double(lexer);
            if (peekToken.Type == Token.TokenType.EOE)
                return Parse_EOE(lexer);

            return Parse_exp(lexer);
        }
        public static ASTNode Parse_not(Lexer lexer)
        {
            List<ASTNode> astnodelist = new List<ASTNode>();

            var peekToken = lexer.Peek();
            var operator_ = lexer.ReadNext();

            var nextASTNode = Parse_condition(lexer);

            astnodelist.Add(nextASTNode);
             
            var node = CreateSyntaxFactor(operator_, astnodelist);

            return node;
        }

        // STRING
        public static ASTNode Parse_exprstring(Lexer lexer)
        {           
            var leftASTNode = Parse_exprsch(lexer);
            var peekToken = lexer.Peek();           

            while (peekToken.Сlassifier == Token.TokenСlassifier.Addition)
            {
                var operator_ = lexer.ReadNext();
                operator_.Сlassifier = Token.TokenСlassifier.Concatenation;

                var rightASTNode = Parse_exprsch(lexer);

                if (rightASTNode == null)
                    throw new Exception($"Invalid Expression. EXPRSTRING Expected at position {lexer.Position}");

                List<ASTNode> astnodelist = new List<ASTNode>() { leftASTNode, rightASTNode };

                leftASTNode = CreateSyntaxFactor(operator_, astnodelist);

                peekToken = lexer.Peek();
            }

            return leftASTNode;
        }
        public static ASTNode Parse_exprsch(Lexer lexer)
        {
            var peekToken = lexer.Peek();

            if (peekToken.Type == Token.TokenType.String)
                return Parse_String(lexer);
            if (peekToken.Type == Token.TokenType.Identity)
                return Parse_Id(lexer);

            throw new Exception($"Invalid Expression.  EXPRSCH Expected at position {lexer.Position}");
        }

        // CALC
        public static ASTNode Parse_exprcalc(Lexer lexer)
        {
            var leftASTNode = Parse_expr1(lexer);

            var peekToken = lexer.Peek();

            while (peekToken.Сlassifier == Token.TokenСlassifier.Addition || 
                peekToken.Сlassifier == Token.TokenСlassifier.Subtraction)
            {
                var operator_ = lexer.ReadNext();   

                var rightASTNode = Parse_expr1(lexer);

                if (rightASTNode == null)
                    throw new Exception($"Invalid Expression. EXPR1 Expected at position {lexer.Position}");

                List<ASTNode> astnodelist = new List<ASTNode>() { leftASTNode, rightASTNode };

                leftASTNode = CreateSyntaxFactor(operator_, astnodelist);

                peekToken = lexer.Peek();
            }

            return leftASTNode;
        }
        public static ASTNode Parse_exprcalc_inbrackets(Lexer lexer)
        {                      
            ASTNode CalcASTNode = null;

            Token peekToken = lexer.Peek();

            Token operator_ = lexer.ReadNext();
            operator_.Сlassifier = Token.TokenСlassifier.Brackets;

            while (peekToken.Сlassifier != Token.TokenСlassifier.RightRoundBracket)
            {
                CalcASTNode = Parse_exprcalc(lexer);

                if (CalcASTNode == null)
                    throw new Exception($"Invalid Expression. CalcExpr Expected at position {lexer.Position}");

                peekToken = lexer.Peek();
            }

            peekToken = lexer.Peek();

            if (peekToken.Сlassifier != Token.TokenСlassifier.RightRoundBracket)
                throw new Exception($"Invalid Expression. RightRoundBracket Expected at position {lexer.Position}");
            else
                lexer.ReadNext();

            List<ASTNode> astnodelist = new List<ASTNode>() { CalcASTNode };
            ASTNode RBracketsASTNode = CreateSyntaxFactor(operator_, astnodelist);

            return RBracketsASTNode;
        }

        public static ASTNode Parse_expr1(Lexer lexer)
        {
            var leftASTNode = Parse_expr2(lexer);

            var peekToken = lexer.Peek();
            // while
            while (peekToken.Сlassifier == Token.TokenСlassifier.Multiplication || 
                peekToken.Сlassifier == Token.TokenСlassifier.Division)
            {
                var operator_ = lexer.ReadNext();

                var rightASTNode = Parse_expr2(lexer);

                if (rightASTNode == null)
                    throw new Exception($"Invalid Expression. EXPR2 Expected at position {lexer.Position}");

                List<ASTNode> astnodelist = new List<ASTNode>() { leftASTNode, rightASTNode };

                leftASTNode = CreateSyntaxFactor(operator_, astnodelist);

                peekToken = lexer.Peek();
            }

            return leftASTNode;
        }
        public static ASTNode Parse_expr2(Lexer lexer)
        {
            var peekToken = lexer.Peek();
            var peekToken2 = lexer.PeekN(2);

            if (peekToken.Type == Token.TokenType.Int)
                return Parse_Int(lexer);
            if (peekToken.Type == Token.TokenType.Double)
                return Parse_Double(lexer);
            if (((peekToken.Type == Token.TokenType.Identity) && (peekToken2.Сlassifier == Token.TokenСlassifier.Increment)) ||
                ((peekToken.Type == Token.TokenType.Identity) && (peekToken2.Сlassifier == Token.TokenСlassifier.Decrement)))
                return Parse_ExpIncDec(lexer);
            if ((peekToken.Type == Token.TokenType.Identity) &&
                 (peekToken2.Сlassifier == Token.TokenСlassifier.Addition ||
                  peekToken2.Сlassifier == Token.TokenСlassifier.Subtraction ||
                  peekToken2.Сlassifier == Token.TokenСlassifier.Multiplication ||
                  peekToken2.Сlassifier == Token.TokenСlassifier.Division ||
                  peekToken2.Сlassifier != Token.TokenСlassifier.Equal))
                return Parse_Id(lexer);

            //if (peekToken2.Сlassifier == Token.TokenСlassifier.Addition ||
            //    peekToken2.Сlassifier == Token.TokenСlassifier.Subtraction)
            //    return Parse_exprcalc(lexer);
            if (peekToken.Сlassifier == Token.TokenСlassifier.LeftRoundBracket)
            {
                return Parse_exprcalc_inbrackets(lexer);            
            }

            throw new Exception($"Invalid Expression.  EXPR2 Expected at position {lexer.Position}");
        }

        // INT - DOUBLE - BOOL - STRING - EXPINCDEC - ID
        public static ASTNode Parse_Int(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Int)
                throw new Exception($"Invalid Expression.  Int Expected at position {lexer.Position}");

            lexer.Accept();  

            return new IntASTNode(token);
        }
        public static ASTNode Parse_Double(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Double)
                throw new Exception($"Invalid Expression.  Double Expected at position {lexer.Position}");

            lexer.Accept(); 

            return new DoubleASTNode(token);
        }
        public static ASTNode Parse_Bool(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Keyword &&
                (token.Value == "true" || token.Value == "false"))
                throw new Exception($"Invalid Expression.  Bool Expected at position {lexer.Position}");

            lexer.Accept();

            return new BoolASTNode(token);
        }
        public static ASTNode Parse_String(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.String)
                throw new Exception($"Invalid Expression.  String Expected at position {lexer.Position}");

            lexer.Accept();

            return new StringASTNode(token);
        }
        public static ASTNode Parse_ExpIncDec(Lexer lexer)
        {
            List<ASTNode> astnodelist = new List<ASTNode>();
            var node = Parse_Id(lexer);
            astnodelist.Add(node);

            var peekToken = lexer.Peek();

            while (peekToken.Сlassifier == Token.TokenСlassifier.Increment ||
                peekToken.Сlassifier == Token.TokenСlassifier.Decrement)
            {
                var operator_ = lexer.ReadNext();

                if (operator_ == null)
                    throw new Exception($"Invalid Expression. INCDEC Expected at position {lexer.Position}");

                node = CreateSyntaxFactor(operator_, astnodelist);

                peekToken = lexer.Peek();
            }

            return node;
        }
        public static ASTNode Parse_Id(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Identity)
                throw new Exception($"Invalid Expression.  Identity Expected at position {lexer.Position}");

            lexer.Accept();  

            return new IdASTNode(token);
        }

        // KEYWORDSNODES
        public static ASTNode Parse_Keyword_Int(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Keyword &&
                token.Сlassifier != Token.TokenСlassifier.Int)
                throw new Exception($"Invalid Expression.  INTKeyword Expected at position {lexer.Position}");

            lexer.Accept();  //consume the token

            return new IntKeywordASTNode(token);
        }
        public static ASTNode Parse_Keyword_Double(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Keyword &&
                token.Сlassifier != Token.TokenСlassifier.Double)
                throw new Exception($"Invalid Expression.  DoubleKeyword Expected at position {lexer.Position}");

            lexer.Accept();  //consume the token

            return new DoubleKeywordASTNode(token);
        }
        public static ASTNode Parse_Keyword_Bool(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Keyword &&
                token.Сlassifier != Token.TokenСlassifier.Bool)
                throw new Exception($"Invalid Expression.  BoolKeyword Expected at position {lexer.Position}");

            lexer.Accept();

            return new BoolKeywordASTNode(token);
        }
        public static ASTNode Parse_Keyword_String(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Keyword &&
                token.Сlassifier != Token.TokenСlassifier.String)
                throw new Exception($"Invalid Expression.  StringKeyword Expected at position {lexer.Position}");

            lexer.Accept();

            return new StringKeywordASTNode(token);
        }

        // KEYWORDS
        public static Token Parse_Keyword_If(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Keyword &&
                token.Сlassifier != Token.TokenСlassifier.If)
                throw new Exception($"Invalid Expression.  IfKeyword Expected at position {lexer.Position}");

            lexer.Accept();

            return token;
        }
        public static Token Parse_Keyword_Else(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Keyword &&
                token.Сlassifier != Token.TokenСlassifier.Else)
                throw new Exception($"Invalid Expression.  ElseKeyword Expected at position {lexer.Position}");

            lexer.Accept();

            return token;
        }
        public static Token Parse_Keyword_Consol(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Keyword &&
                token.Сlassifier != Token.TokenСlassifier.Consol)
                throw new Exception($"Invalid Expression.  ConsolKeyword Expected at position {lexer.Position}");

            lexer.Accept();

            return token;
        }
        public static Token Parse_Keyword(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.Keyword)
                throw new Exception($"Invalid Expression.  Keyword Expected at position {lexer.Position}");

            lexer.Accept();  

            return token;
        }

        // SIGNSEPARATORS
        public static ASTNode Parse_Signseparator(Lexer lexer)
        {
            var token = lexer.Peek();

            if (token.Type != Token.TokenType.Signseparator)
                throw new Exception($"Invalid Expression.  Signseparator Expected at position {lexer.Position}");

            lexer.Accept();

            return new SignseparatorASTNode(token);
        }

        // EOE
        public static ASTNode Parse_EOE(Lexer lexer)
        {
            var token = lexer.Peek();
            if (token.Type != Token.TokenType.EOE)
                throw new Exception($"Invalid Expression.  EOE Expected at position {lexer.Position}");

            lexer.Accept();

            return new EOEASTNode(token);
        }

        private static SyntaxFactorASTNode CreateSyntaxFactor(Token token, List<ASTNode> astnodelist)
        {
            switch (token.Сlassifier)
            {
                case Token.TokenСlassifier.Return: return new KeywordReturnASTNode(token, astnodelist);                
                case Token.TokenСlassifier.While: return new WhileOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.If: return new IfOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Else: return new ElseOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Consol: return new ConsolOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.LessThan: return new LessThanOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.GreaterThan: return new GreaterThanOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.LessOrEqualThan: return new LessOrEqualThanOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.GreaterOrEqualThan: return new GreaterOrEqualThanOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.LogicalAnd: return new LogicalAndOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.LogicalNotEqual: return new LogicalNotOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.LogicalOR: return new LogicalOROperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.LogicalEqual: return new LogicalEqualOperatorASTNode(token, astnodelist);               
                case Token.TokenСlassifier.Equal: return new EqualOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Increment: return new IncrOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Decrement: return new DecOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Addition: return new AdditionOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Subtraction: return new SubtractionOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Multiplication: return new MultiplicationOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Division: return new DivisionOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Comma: return new CommaOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.ExclamationMark: return new CommaOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.LeftRoundBracket: return new LeftRoundBracketOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.RightRoundBracket: return new RightRoundBracketOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.LeftCurlyBracket: return new LeftCurlyBracketOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.RightCurlyBracket: return new RightCurlyBracketOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Brackets: return new BracketsOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Identity: return new IdOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.Concatenation: return new ConcatenationOperatorASTNode(token, astnodelist);
                case Token.TokenСlassifier.None: return new SyntaxExprOperatorASTNode(token, astnodelist);
                default:
                    throw new ArgumentOutOfRangeException(nameof(token));
            }
        }
    }
}
