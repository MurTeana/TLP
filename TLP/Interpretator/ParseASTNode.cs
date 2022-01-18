using System;
using System.Collections.Generic;
using TLP.Parser_;

namespace TLP.Interpretator
{
    public class ParseASTNode
    {
        public static List<Identity_> Identity_List = new List<Identity_>();
        public static List<string> PrintToConsol_List = new List<string>();
        public static List<ASTNode> Astnode_List = new List<ASTNode>();
        public static int level = 0;
        public static List<string> PathLevels = new List<string>();
        public static bool flag = true;

        public static void parseAST(SyntaxFactorASTNode astNode)
        {
            level++;
            PathLevels.Add(astNode.ToString());

            int i = 0;

            foreach (ASTNode node in astNode.ASTNodeList)
            {
                var nodeprint = (astNode as SyntaxFactorASTNode).ASTNodeList[i];

                Astnode_List.Add(nodeprint);

                string levelpoint = "";

                for (int l = 0; l < level; l++)
                    levelpoint = levelpoint + "    ";

                string path = "";

                for (int l = 0; l < PathLevels.Count; l++)
                    path = path + "\n" + (l+1).ToString() + PathLevels[l];

                Console.WriteLine("{0}Lv {1} Node = {2} Value = {3}", levelpoint, level, node.ToString(), node.Token.Value);
                //Console.WriteLine("Path = {1}", levelpoint, path);

                // Evaluate
                evaluateASTNode(node);

                if ((nodeprint as SyntaxFactorASTNode) != null)
                {
                    parseAST(nodeprint as SyntaxFactorASTNode);
                    level--;
                    PathLevels.RemoveAt(PathLevels.Count - 1);
                }                      

                i++;
            }
        }

        // Evaluate
        public static void evaluateASTNode(ASTNode node)
        {                          
            if (node.ToString() == "TLP.Parser_.WhileOperatorASTNode")
            {
                var expressionEngine = new ExpressionEngine();

                string result = (expressionEngine.EvaluateNode(node)).ToString();

                flag = false;
            }

            if (node.ToString() == "TLP.Parser_.IfOperatorASTNode")
            {
                var expressionEngine = new ExpressionEngine();

                string result = (expressionEngine.EvaluateNode(node)).ToString();

                flag = false;
            }

            if (node.ToString() == "TLP.Parser_.EqualOperatorASTNode" && flag == true)
            {
                Identity_ identity_ = new Identity_();

                var expressionEngine = new ExpressionEngine();

                string idname = (node as SyntaxFactorASTNode).ASTNodeList[0].Token.Value;
                string result = (expressionEngine.EvaluateNode(node)).ToString();

                int num = isInIdentityInListNum(idname);

                if (num == -1)
                {
                    identity_.IdentityName = idname;
                    identity_.Value = result;

                    Identity_List.Add(identity_);
                }
                else
                {
                    Identity_List[num].Value = result;
                }
            }
        }

        // Проверка наличия идентификатора в списке
        public static int isInIdentityInListNum(string idname)
        {
            int num = -1;

            for (int i = 0; i < Identity_List.Count; i++)
            {
                if (idname == Identity_List[i].IdentityName)
                    num = i;
            }

            return num;
        }

        // Вывод на экран всех идентификаторов
        public static void printIdentity(SyntaxFactorASTNode astNode)
        {
            parseAST(astNode);
            Console.WriteLine("\n");

            for (int z = 0; z < Identity_List.Count; z++)
                Console.WriteLine("Идентификатор {0} = {1}", Identity_List[z].IdentityName, Identity_List[z].Value);
            
            Console.WriteLine("\n");

            printToConsol();
        }

        // Вывод на экран всех данных оператора consol
        public static void printToConsol()
        {
            for (int z = 0; z < PrintToConsol_List.Count; z++)
                Console.WriteLine("ConsolResult = {0}", PrintToConsol_List[z]);

            Console.WriteLine("\n");
        }

        // Вывод на экран всех узлов дерева
        public static void printListASTNode(SyntaxFactorASTNode astNode)
        {
            parseAST(astNode);

            Console.WriteLine("{0} = {1}", astNode.ToString(), astNode.Token.Value);

            for (int z = 0; z < Astnode_List.Count; z++)
                Console.WriteLine("{0} = {1}", Astnode_List[z].ToString(), Astnode_List[z].Token.Value);
            
            Console.WriteLine("\n");
        }

        // Получение значения идентификатора
        public static string getIdValue(string idname)
        {
            string idValue = "";

            for (int i = 0; i < Identity_List.Count; i++)
            {
                if (idname == Identity_List[i].IdentityName)
                    idValue = Identity_List[i].Value;
            }

            return idValue;
        }
    }
}
