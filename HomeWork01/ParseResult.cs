namespace HomeWork01
{
    /// <summary>
    /// Структура, содержащая результаты парсинга команды
    /// </summary>
    public readonly ref struct ParseResult
    {
        /// <summary>
        /// Комада
        /// </summary>
        public ReadOnlySpan<char> Command { get; init; }
        
        /// <summary>
        /// Ключ
        /// </summary>
        public ReadOnlySpan<char> Key { get; init; }
        
        /// <summary>
        /// Значение
        /// </summary>
        public ReadOnlySpan<char> Value { get; init; }

        public ParseResult(ReadOnlySpan<char> command, ReadOnlySpan<char> key, ReadOnlySpan<char> value)
        { 
            Command = command;
            Key = key; 
            Value = value;
        }
    }
}