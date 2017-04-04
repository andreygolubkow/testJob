using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using testJob.Model;

namespace testJob
{
    /// <summary>
    /// Класс для работы с файлом.
    /// </summary>
    public static class FileReader
    {
        /// <summary>
        /// Перечисление столбцов.
        /// </summary>
        public enum Rows
        {
            Id,
            dt,
            product_id,
            amount

        }

        /// <summary>
        /// Считывание строки со столбцами и преобразование их в enum.
        /// </summary>
        /// <param name="fileStreamReader">Поток с файлом.</param>
        /// <returns>Массив столбцов.</returns>
        public static Rows[]  ReadRowses(StreamReader fileStreamReader)
        {
            var rowsArray = new Rows[4];
            string lowerTextLine = fileStreamReader.ReadLine()?.ToLower();
            for (var i = 0; i < 4; i++)
            {
                Enum.TryParse(lowerTextLine?.Split('\t')[i], out rowsArray[i]);
            }
            return rowsArray;
        }

        /// <summary>
        /// Считываение строки данных о заказе.
        /// </summary>
        /// <param name="fileStreamReader">Поток с файлом.</param>
        /// <param name="count">Количество столбцов.</param>
        /// <returns>Массив строк считанных из файла.</returns>
        private static string[] ReadOrderString(StreamReader fileStreamReader, int count)
        {
            string textLine = fileStreamReader.ReadLine();
            string[] dataStrings = textLine.Split('\t');
            if (dataStrings.Length < count)
            {
                throw new ArgumentException("Ошибка в строке с данными.");
            }
            return dataStrings;
        }

        /// <summary>
        /// Приведение строки с данными о заказе в объекта заказа.
        /// </summary>
        /// <param name="orderStrings">Строка с данными.</param>
        /// <param name="formatRowses">Порядок столбцов.</param>
        /// <returns>Объект Order.</returns>
        private static Order StringsToOrder(string[] orderStrings, Rows[] formatRowses)
        {
            var order = new Order();
            for (var i = 0; i < formatRowses.Length; i++)
            {
                switch (formatRowses[i])
                {
                    case (Rows.Id):
                        order.id = Convert.ToInt32(orderStrings[i]);
                        break;
                    case (Rows.amount):
                        order.amount = Convert.ToDouble(orderStrings[i], CultureInfo.InvariantCulture);
                        break;
                    case (Rows.dt):
                        DateTime dataTime = Convert.ToDateTime(orderStrings[i]);
                        order.dt = (int)(dataTime - new DateTime(1970, 1, 1)).TotalSeconds;
                        break;
                    case (Rows.product_id):
                        order.product_id = Convert.ToInt32(orderStrings[i]);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("Неизвестный столбец.");
                }
            }
            return order;
        }

        /// <summary>
        /// Считывание строки с заказом в указанном формате.
        /// </summary>
        /// <param name="fileStreamReader">Поток данных.</param>
        /// <param name="formatRowses">Массив содержащий значения перечисления(названия столбцов).</param>
        /// <returns>Объект Order.</returns>
        private static Order ReadOrder(StreamReader fileStreamReader, Rows[] formatRowses)
        {
            string[] dataStrings = ReadOrderString(fileStreamReader,formatRowses.Length);
            return StringsToOrder(dataStrings, formatRowses);
        }

        /// <summary>
        /// Проверка файла на существование.
        /// </summary>
        /// <param name="fileName">Путь к файлу.</param>
        public static void CheckFile(string fileName)
        {
            var fileInfo = new FileInfo(fileName);
            if (!fileInfo.Exists)
            {
                throw new FileLoadException("Ошибка в файле.");
            }
        }

        /// <summary>
        /// Разбор данных в файле на список заказов.
        /// </summary>
        /// <param name="fileName">Файл с данными.</param>
        /// <param name="exceptions">Список для хранения ошибок.</param>
        /// <returns>Перечисление(список) заказов.</returns>
        public static IEnumerable<Order> ParseFile(string fileName,List<Exception> exceptions)
        {
            var fileReader = new StreamReader(fileName);
            Rows[] rowses = ReadRowses(fileReader);
            var linesCounter = 0;
            var ordersList = new List<Order>();
            while (!fileReader.EndOfStream)
            {
                linesCounter++;
                try
                {
                    ordersList.Add(ReadOrder(fileReader, rowses));
                }
                catch (Exception exception)
                {
                    exceptions.Add(new FormatException($"Ошибка в строке {linesCounter}." + exception.Message));
                    /* Строки с данными считаются с 1.
                    * 0-я строка, строка со столбцами */
                }
            }
            fileReader.Close();
            return ordersList;
        }
    }
}
