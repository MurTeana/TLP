using System.Collections.Generic;

namespace TLP.Lexer_
{
    public sealed class SourceScanner
    {
        readonly string _buffer;

        readonly Stack<int> _positionStack = new Stack<int>();
        
        // Текущая позиция
        public int Position { get; private set; }

        // Индикатор конца файла
        public bool EndOfSource => Position >= _buffer.Length;

        // Создает экземпляр сканера источника, используя указанный источник
        public SourceScanner(string source) => _buffer = source;

        // Считывание следующего символа из исходного буфера
        public char? Read() => EndOfSource ? (char?)null : _buffer[Position++];

        // Просмотр следующего символа в исходном буфере
        public char? Peek()
        {
            Push();
            var nextChar = Read();
            Pop();
            return nextChar;
        }

        // Сохранение текущей позиции
        public void Push() => _positionStack.Push(Position);

        // Возврат к предыдущей позиции
        public void Pop() => Position = _positionStack.Pop();

    }
}
