using System.IO;

namespace TLP.Helper
{
    public class ReadFile
    {
        // Функция чтения из файла
        public string readtext()
        {
            FileStream file = new FileStream("..\\..\\..\\Program1.txt", FileMode.Open);
            StreamReader reader = new StreamReader(file);
            string text = reader.ReadToEnd();
            reader.Close();

            return text;
        }
    }
}
