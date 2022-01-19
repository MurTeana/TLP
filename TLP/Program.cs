using System;
using TLP.Helper;
using TLP.Interpretator;
using TLP.Lexer_;
using TLP.Parser_;

namespace TLP
{
    class Program
    {     
        static void Main(string[] args)
        {
            // Чтение из файла
            ReadFile read = new ReadFile();
            string expression = read.readtext();

            // Лексер
            Lexer lexer = new Lexer(new SourceScanner(expression));
            Token token = null;

            do
            {
                token = lexer.ReadNext();
                Console.WriteLine("Текущая лексема: {0} - Тип лексемы: {1} -  Классификатор лексемы: {2} - Позиция: {3}", token.Value, token.Type, token.Сlassifier, token.Position);

            } while (token.Type != Token.TokenType.EOE);

            // Парсер (построение дерева)
            Parser parser = new Parser();
            var ast = Parser.Parse(expression) as SyntaxFactorASTNode;
            Console.WriteLine("\n");

            // Вывод на печать узлов дерева и вычисление значений идентификаторов
            ParseASTNode.printASTNodeInfList(ast);

            Console.WriteLine("");
        }
    }
}
