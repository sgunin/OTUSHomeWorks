namespace HomeWork01
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Реализация базового хранилища SimpleStore
            Console.WriteLine("Test SimpleStore");
            
            var store = new SimpleStore();
            store.Set("key1", [1, 2, 3, 4, 5]);
            store.Set("key2", [6, 7, 8, 9, 0]);

            byte[]? data = store.Get("key1");
            if (data != null)
                foreach (var item in data)
                    Console.WriteLine($"{item:X2} ");
            store.Delete("key1");

            // Парсер команд CommandParser
            Console.WriteLine("\nTest CommandParser");
            
            foreach (string line in File.ReadLines("sampleCommands.txt"))
            {
                ParseResult result = CommandParser.Parse(line);
                if (!result.Command.IsEmpty)
                    Console.WriteLine($"Command: {result.Command}, " +
                        $"Key: {(!result.Key.IsEmpty ? result.Key : "-")}, " +
                        $"Value: {(!result.Value.IsEmpty ? result.Value : "-")}");
            }

            Console.WriteLine("\nНажмите Enter для завершения...");
            Console.ReadLine();
        }
    }
}