using HomeWork01;
using System.Buffers;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace HomeWork02
{
    /// <summary>
    /// класса TCP-сервера
    /// </summary>
    public class TcpServer : IDisposable
    {
        private const int SOCKET_BUFFER_SIZE = 1024;

        private bool _listening = false;
        private Socket _listenerSocket;
        private int _client;

        /// <summary>
        /// Конструктор
        /// </summary>
        public TcpServer()
        {

        }

        /// <summary>
        /// Деструктор
        /// </summary>
        ~TcpServer() 
        {
            Dispose(false);
        }

        /// <summary>
        /// Удаление ресурсов, занимаемых объектом
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Удаление ресурсов, занимаемых объектом
        /// </summary>
        private void Dispose(bool isDisposing)
        {
            if (!isDisposing)
                return;

             StopServer();
        }


        /// <summary>
        /// метод будет инициализировать Socket, связывать его с локальным IP-адресом и портом (например, 127.0.0.1:8080) и переводить в режим прослушивания (Listen).
        /// </summary>
        /// <param name="localPort">локальный порт, на котором сервис принимает входящие соединения</param>
        /// <param name="maxConnections">максимальная длина очереди подключения (по умолчанию 100)</param>
        /// <returns></returns>
        public void StartAsync(ushort localPort, ushort maxConnections = 100)
        {
            if (localPort < 1024 & localPort > 65535) throw new ArgumentNullException(nameof(localPort), "Значение localPort должно быть в диапазоне [1024;65535]");

            IPEndPoint endPoint = new(IPAddress.Any, localPort);
            try
            {
                _listenerSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _listenerSocket.Bind(endPoint);
                _listenerSocket.Listen(maxConnections);
                Console.WriteLine($"Запущен приём новых подключений на {_listenerSocket.LocalEndPoint}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"В процессе запуска сервера возникла ошибка: {ex.Message}");
                return;
            }

            _listening = true;
            Task.Run(() => StartListening());
        }

        /// <summary>
        /// Приём клиентских подключений
        /// </summary>
        /// <returns></returns>
        private async Task StartListening()
        {
            while (_listening)
            {
                try
                {
                    Socket clientSocket = await _listenerSocket.AcceptAsync();
                    Console.WriteLine($"Установлено новое клиентское подключение: {clientSocket.RemoteEndPoint}");

                    // Обработка клиентских подключений в отдельном потоке
                    _ = Task.Run(() => ProcessClientAsync(clientSocket));
                }
                catch (SocketException se)
                {
                    Console.WriteLine($"В процессе обработки клиентского подключения возникло исключение: {se.Message}");
                    break;
                }
                catch (ObjectDisposedException)
                { break; }
            }
        }

        /// <summary>
        /// Обработка информации, полученной из клиентского подключения 
        /// </summary>
        /// <param name="socket"></param>
        public async Task ProcessClientAsync(Socket socket)
        {
            int clientNum = Interlocked.Increment(ref _client);
            byte[] buffer = ArrayPool<byte>.Shared.Rent(SOCKET_BUFFER_SIZE);

            while (true)
            {
                int receivedBytes;
                try
                {
                    receivedBytes = await socket.ReceiveAsync(buffer, SocketFlags.None);
                }
                catch (SocketException se)
                {
                    Console.WriteLine($"В процессе получения данных от клиентского подключения возникло исключение: {se.Message}");
                    break;
                }
                catch (OperationCanceledException)
                { break; }
                catch (ObjectDisposedException)
                { break; }

                // Если вызов ReceiveAsync возвращает 0, это означает, что клиент закрыл соединение. 
                if (receivedBytes == 0)
                {
                    Console.WriteLine($"Подключение {socket.RemoteEndPoint} закрыто");
                    socket.Close();
                    socket.Dispose();
                    break;
                }

                char[] array = Encoding.UTF8.GetChars(buffer[..receivedBytes]);
                ParseResult result = CommandParser.Parse(array);
                if (!result.Command.IsEmpty)
                    Console.WriteLine($"Client {clientNum} - Command: {result.Command}, " +
                        $"Key: {(!result.Key.IsEmpty ? result.Key : "-")}, " +
                        $"Value: {(!result.Value.IsEmpty ? result.Value : "-")}");
            }

            // Возвращаем буффер в пул
            ArrayPool<byte>.Shared.Return(buffer, clearArray: false);
        }

        public void StopServer()
        {
            _listening = false;
            _listenerSocket?.Close();
        }
    }
}