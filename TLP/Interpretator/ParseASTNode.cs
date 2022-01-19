using System;
using System.Collections.Generic;
using TLP.Parser_;

namespace TLP.Interpretator
{
    public class ParseASTNode
    {
        public static List<NodeInf> NodeInf_List = new List<NodeInf>();
        public static List<Identity_> Identity_List = new List<Identity_>();
        public static List<string> PrintToConsol_List = new List<string>();       
        public static bool flag = true;
        public static int level = 0;
        public static List<string> PathLevels = new List<string>();

        public static void parseAST(SyntaxFactorASTNode astNode)
        {
            level++;
            PathLevels.Add(astNode.ToString());

            int i = 0;

            foreach (ASTNode node in astNode.ASTNodeList)
            {
                var nodeprint = (astNode as SyntaxFactorASTNode).ASTNodeList[i];

                string path = "";

                for (int l = 0; l < PathLevels.Count; l++)
                    path = path + "\n" + (l+1).ToString() + PathLevels[l];

                // Информация об узле
                NodeInf nodeInf = new NodeInf();

                nodeInf.NodeName = node.ToString();
                nodeInf.Token = node.Token;
                nodeInf.Level = level;
                nodeInf.LongPath = path + "\n" + (level + 1).ToString() + node.ToString();

                NodeInf_List.Add(nodeInf);

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
            //parseAST(astNode);
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
        public static void printASTNodeInfList(SyntaxFactorASTNode astNode)
        {
            parseAST(astNode);

            Console.WriteLine("Lv 0 Node = {0} Value = {1}", astNode.ToString(), astNode.Token.Value);

            for (int z = 0; z < NodeInf_List.Count; z++)
            {
                string lvpoint_ = levelpoint(NodeInf_List[z].Level);

                Console.WriteLine("{0}Lv {1} Node = {2} Value = {3}", lvpoint_, NodeInf_List[z].Level, NodeInf_List[z].NodeName, NodeInf_List[z].Token.Value);
                //Console.WriteLine("Path {0}", NodeInf_List[z].LongPath);
            }

            Console.WriteLine("\n");

            printIdentity(astNode);
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
        public static string levelpoint(int lv)
        {
            string levelpoint = "";

            for (int l = 0; l < lv; l++)
                levelpoint = levelpoint + "    ";

            return levelpoint;
        }
    }
}
