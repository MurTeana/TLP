using System;
using System.Collections.Generic;
using TLP.Parser_;

namespace TLP.Interpretator
{
    public class ExpressionEngine
    {
        public List<Identity_> idList = new List <Identity_>();

        public string EvaluateNode(ASTNode astRoot)
        {
            if (astRoot.ToString() == "TLP.Parser_.AdditionOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.SubtractionOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.MultiplicationOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.DivisionOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.IntASTNode" ||
                astRoot.ToString() == "TLP.Parser_.DoubleASTNode" ||                
                astRoot.ToString() == "TLP.Parser_.StringASTNode" ||
                astRoot.ToString() == "TLP.Parser_.IdASTNode" ||
                astRoot.ToString() == "TLP.Parser_.ConcatenationOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.BoolASTNode" ||
                astRoot.ToString() == "TLP.Parser_.IncrOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.DecOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.EqualOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.LogicalAndOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.LogicalOROperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.LessThanOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.GreaterThanOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.LessOrEqualThanOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.GreaterOrEqualThanOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.LogicalNotOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.WhileOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.ConsolOperatorASTNode" ||
                astRoot.ToString() == "TLP.Parser_.IfOperatorASTNode")
            {
                return Convert.ToString(EvaluateNode(astRoot as dynamic));
            }
            else
            {
                return "Неподходящий формат";
            }
        }
     
        // EvaluateNodesLogic

        // EvaluateEqualExpr
        protected string EvaluateNode(EqualOperatorASTNode node)
        {
            
            return Convert.ToString(EvaluateNode(node.ASTNodeList[1] as dynamic));
        }

        // EvaluateCalcExpr
        protected double EvaluateNode(AdditionOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic)) + Convert.ToDouble(EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected double EvaluateNode(SubtractionOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic)) - Convert.ToDouble(EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected double EvaluateNode(MultiplicationOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic)) * Convert.ToDouble(EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected double EvaluateNode(DivisionOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic)) / Convert.ToDouble(EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected double EvaluateNode(BracketsOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic));
        protected int EvaluateNode(IntASTNode node) => node.Value;
        protected double EvaluateNode(DoubleASTNode node) => node.Value;
        protected string EvaluateNode(IdASTNode node) => ParseASTNode.getIdValue(node.Value);

        // EvaluateStringExpr       
        protected string EvaluateNode(ConcatenationOperatorASTNode node) => EvaluateNode(node.ASTNodeList[0] as dynamic) + EvaluateNode(node.ASTNodeList[1] as dynamic);
        protected string EvaluateNode(StringASTNode node) => node.Value;

        // EvaluateConditionExpr
        protected bool EvaluateNode(BoolASTNode node) => Convert.ToBoolean(node.Value.ToString());
        protected bool EvaluateNode(LogicalAndOperatorASTNode node) => (EvaluateNode(node.ASTNodeList[0] as dynamic)) && (EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected bool EvaluateNode(LogicalOROperatorASTNode node) => (EvaluateNode(node.ASTNodeList[0] as dynamic)) || (EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected bool EvaluateNode(LogicalEqualOperatorASTNode node) => Convert.ToString(EvaluateNode(node.ASTNodeList[0] as dynamic)) == Convert.ToString(EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected bool EvaluateNode(LessThanOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic)) < Convert.ToDouble(EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected bool EvaluateNode(GreaterThanOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic)) > Convert.ToDouble(EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected bool EvaluateNode(LessOrEqualThanOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic)) <= Convert.ToDouble(EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected bool EvaluateNode(GreaterOrEqualThanOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic)) >= Convert.ToDouble(EvaluateNode(node.ASTNodeList[1] as dynamic));
        protected bool EvaluateNode(LogicalNotOperatorASTNode node) => !(EvaluateNode(node.ASTNodeList[0] as dynamic));

        // EvaluateIncDec
        protected double EvaluateNode(IncrOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic)) + 1;
        protected double EvaluateNode(DecOperatorASTNode node) => Convert.ToDouble(EvaluateNode(node.ASTNodeList[0] as dynamic)) - 1;

        // EvaluateWhile
        protected string EvaluateNode(WhileOperatorASTNode node)
        {
            bool condition = (EvaluateNode((node as SyntaxFactorASTNode).ASTNodeList[1] as dynamic));

            while (Convert.ToBoolean(EvaluateNode((node as SyntaxFactorASTNode).ASTNodeList[1] as dynamic)))
            {
                int i = 0;

                foreach (ASTNode nodeX in ((node as SyntaxFactorASTNode).ASTNodeList[4] as SyntaxFactorASTNode).ASTNodeList)
                {
                    string result = EvaluateNode(((node as SyntaxFactorASTNode).ASTNodeList[4] as SyntaxFactorASTNode).ASTNodeList[i] as dynamic);

                    string idname = (((node as SyntaxFactorASTNode).ASTNodeList[4] as SyntaxFactorASTNode).ASTNodeList[i] as SyntaxFactorASTNode).ASTNodeList[0].Token.Value;

                    int num = ParseASTNode.isInIdentityInListNum(idname);

                    if (num > -1)
                    {
                        ParseASTNode.Identity_List[num].Value = result;
                    }

                    i++;
                }
            }

            return condition.ToString();
        }

        // EvaluateIfElse
        protected string EvaluateNode(IfOperatorASTNode node)
        {
            bool condition = (EvaluateNode((node as SyntaxFactorASTNode).ASTNodeList[1] as dynamic));

            if (condition)
            {
                int i = 0;

                foreach (ASTNode nodeX in ((node as SyntaxFactorASTNode).ASTNodeList[4] as SyntaxFactorASTNode).ASTNodeList)
                {
                    string result = EvaluateNode(((node as SyntaxFactorASTNode).ASTNodeList[4] as SyntaxFactorASTNode).ASTNodeList[i] as dynamic);

                    string idname = (((node as SyntaxFactorASTNode).ASTNodeList[4] as SyntaxFactorASTNode).ASTNodeList[i] as SyntaxFactorASTNode).ASTNodeList[0].Token.Value;

                    int num = ParseASTNode.isInIdentityInListNum(idname);

                    if (num > -1)
                    {
                        ParseASTNode.Identity_List[num].Value = result;
                    }

                    i++;
                }
            }
            else
            {
                int i = 0;

                foreach (ASTNode nodeX in (((node as SyntaxFactorASTNode).ASTNodeList[6] as SyntaxFactorASTNode).ASTNodeList[1]as SyntaxFactorASTNode).ASTNodeList)
                {
                    string result = EvaluateNode((((node as SyntaxFactorASTNode).ASTNodeList[6] as SyntaxFactorASTNode).ASTNodeList[1] as SyntaxFactorASTNode).ASTNodeList[i] as dynamic);

                    string idname = ((((node as SyntaxFactorASTNode).ASTNodeList[6] as SyntaxFactorASTNode).ASTNodeList[1] as SyntaxFactorASTNode).ASTNodeList[0] as SyntaxFactorASTNode).ASTNodeList[0].Token.Value;

                    int num = ParseASTNode.isInIdentityInListNum(idname);

                    if (num > -1)
                    {
                        ParseASTNode.Identity_List[num].Value = result;
                    }

                    i++;
                }
            }

            return condition.ToString();
        }

        // EvaluateConsol
        protected string EvaluateNode(ConsolOperatorASTNode node)
        {
            string printToConsol = "";

            int i = 0;
            
            foreach (ASTNode nodeX in (node as SyntaxFactorASTNode).ASTNodeList)
            {
                if (nodeX.ToString() == "TLP.Parser_.StringASTNode" || nodeX.ToString() == "TLP.Parser_.IdASTNode")
                {
                    string result = EvaluateNode((node as SyntaxFactorASTNode).ASTNodeList[i]);

                    printToConsol = printToConsol + result;
                }

                i++;
            }

            ParseASTNode.PrintToConsol_List.Add(printToConsol);            

            return printToConsol;
        }
    }
}
