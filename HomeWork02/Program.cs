namespace HomeWork02
{
    public class Program
    {
        static void Main(string[] args)
        {
            TcpServer server = new();
            server. StartAsync(8080);
            Console.WriteLine("\nСервис запущен на порту 8080...");

            // Для тестирования функционала сервера запускаем 10 клиентов
            TestClient.Start(10);

            // После того, как отработала тестовая нагрузка сервис не умирает, а ждет новых подключений
            // Можно проверить функционал через telnet 127.0.0.1 8080

            Console.WriteLine("\nСервис ожидает новые подключения");
            Console.WriteLine("\nНажмите любую клавишу для его завершения...");
            Console.ReadLine();
            server.StopServer();
            Console.WriteLine("\nСервис остановлен...");
        }
    }
}