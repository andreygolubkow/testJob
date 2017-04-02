using System;
using System.Globalization;
using System.IO;
using System.Linq;

using testJob.Model;

namespace testJob
{
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

        };
        /// <summary>
        /// Считывание строки со столбцами и преобразование их в enum.
        /// </summary>
        /// <param name="fileStream">Поток с файлом.</param>
        /// <returns>Массив столбцов.</returns>
        public static string[]  ReadRowses(StreamReader fileStreamReader)
        {
            return fileStreamReader.ReadLine()?.ToLower().Split('\t');
        }

        /// <summary>
        /// Считывание строки с заказом в указанном формате.
        /// </summary>
        /// <param name="fileStreamReader">Поток данных.</param>
        /// <param name="formatRowses">Массив содержащий значения перечисления(названия столбцов).</param>
        /// <returns>Объект Order.</returns>
        public static string[] ReadOrder(StreamReader fileStreamReader)
        {
            string textLine = fileStreamReader.ReadLine();
            string[] dataStrings = textLine.Split('\t');
            if ( dataStrings.Length < 4 )
            {
                throw new ArgumentException("Ошибка в строке с данными.");
            }
            return dataStrings;
        }

    }
}
