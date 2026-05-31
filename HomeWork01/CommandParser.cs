namespace HomeWork01
{
    /// <summary>
    /// Статический класс, способный разбирать входящие команды без выделения дополнительной памяти 
    /// в управляемой куче (zero-allocation)
    /// </summary>
    public static class CommandParser
    {
        /// <summary>
        /// разделитель лексем в строке
        /// </summary>
        private const char COMMA = ' ';

        /// <summary>
        /// метод отвечает за разбор команды
        /// </summary>
        /// <param name="line">входная строка</param>
        /// <returns></returns>
        public static ParseResult Parse(ReadOnlySpan<char> line)
        {
            // Проверяем на пустую строку, возвращая пустую структуру
            if (line.IsEmpty)
               return new ParseResult([], [], []);

            // Находим индекс первого разделителя
            // Если разделителя не найдено, во входной строке присутствует только одна команда
            int firstComma = line.IndexOf(COMMA);
            if (firstComma == -1)
                return new ParseResult(line, [], []);

            // Если строка состоит из разделителя, возвращаем пустую структуру 
            if (firstComma == 0 & line.Length == 1)
                return new ParseResult([], [], []);

            // Получаем команду из строки (замена line.Slice(0, firstComma) на line[..firstComma])
            ReadOnlySpan<char> command = line[..firstComma];

            // Двигаемся вперед по строке, пропуская лишние пробелы между аргументами
            foreach (char c in line[firstComma..])
                if (c == COMMA) firstComma++; else break;

            // Получаем оставшийся блок текста из строки
            ReadOnlySpan<char> textBlock = line[(firstComma)..];

            // Находим индекс второго разделителя, если его нет, значение отсутствует
            int secComma = textBlock.IndexOf(COMMA);
            if (secComma == -1)
                return new ParseResult(command, textBlock, []);

            // Получаем ключ из строки
            ReadOnlySpan<char> key = textBlock[..secComma];

            // Двигаемся вперед по строке, пропуская лишние пробелы между аргументами
            foreach (char c in textBlock[secComma..])
                if (c == COMMA) secComma++; else break;

            // Получаем значение из оставшегося блока
            ReadOnlySpan<char> value = textBlock[(secComma)..];

            return new ParseResult(command, key, value);
        }
    }
}