using System.Net.Sockets;
using System.Text;

namespace HomeWork02
{
    public class TestClient
    {
        public static void Start(int count)
        {
            List<Task> clients = [];
            
            for (int i = 0; i < count; i++)
                clients.Add(Task.Run(() =>
                {
                    string fileName = "sampleCommands.txt";
                    var file = File.ReadLines(fileName);
                    using TcpClient client = new("127.0.0.1", 8080);
                    NetworkStream stream = client.GetStream();

                    // Рандоминизируем строки, которые клиенты прочтут из файла
                    // Общее количество строк в файле
                    int totalLines = file.Count();
                    // Случайное количество строк, которое прочитает каждый клиент из файла
                    int randLineCount = Random.Shared.Next(0, totalLines);
                    // Читаем произвольное количество строк
                    for (int i = 0; i < randLineCount+1; i++)
                    {
                        byte[] data = Encoding.ASCII.GetBytes(file.ElementAt(Random.Shared.Next(0, randLineCount)));
                        stream.Write(data, 0, data.Length);
                        // Искуственная задержка
                        Thread.Sleep(300*randLineCount);
                    }

                    client.Close();
                }));
            
            Task.WaitAll(clients);
        }
    }
}