namespace HomeWork01
{
    /// <summary>
    /// Класс, отвечающий за хранение данных в памяти
    /// </summary>
    public class SimpleStore
    {
        private readonly Dictionary<string, byte[]> _dict = [];

        /// <summary>
        /// добавляет или обновляет значение по ключу
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, byte[] value)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key), "Ключ не может быть null или empty");
            if (value == null || value.Length == 0) throw new ArgumentNullException(nameof(value), "Значение не может быть null или иметь нулевую длину");

            if (!_dict.TryAdd(key, value))
                throw new ArgumentException("Элемент с этим ключем уже добавлен в коллекцию", nameof(key));
        }

        /// <summary>
        /// возвращает значение по ключу или null, если ключ не найден
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public byte[]? Get(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key), "Ключ не может быть null или empty");

            if (_dict.TryGetValue(key, out var value))
                return value;
            else return null;
        }

        /// <summary>
        /// удаляет ключ и значение
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key), "Ключ не может быть null или empty");

            _dict.Remove(key);
        }
    }
}
