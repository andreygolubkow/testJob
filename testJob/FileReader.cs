﻿using System;
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
        public static Rows[]  ReadRowses(StreamReader fileStreamReader)
        {
            var rowsArray = new Rows[4];
            string lowerTextLine = fileStreamReader.ReadLine().ToLower();
            for (int i = 0; i < 4; i++)
            {
                Enum.TryParse(lowerTextLine.Split('\t')[i], out rowsArray[i]);
            }
            return rowsArray;
        }

        /// <summary>
        /// Считывание строки с заказом в указанном формате.
        /// </summary>
        /// <param name="fileStreamReader">Поток данных.</param>
        /// <param name="formatRowses">Массив содержащий значения перечисления(названия столбцов).</param>
        /// <returns>Объект Order.</returns>
        public static Order ReadOrder(StreamReader fileStreamReader, Rows[] formatRowses)
        {
            string textLine = fileStreamReader.ReadLine();
            string[] dataStrings = textLine.Split('\t');
            if ( dataStrings.Length < formatRowses.Length )
            {
                throw new ArgumentException("Ошибка в строке с данными.");
            }
            var order = new Order();
            for (var i = 0; i < formatRowses.Length; i++)
            {
                switch ( formatRowses[i] )
                {
                    case (Rows.Id):
                        order.Id = Convert.ToInt32(dataStrings[i]);
                        break;
                    case (Rows.amount):
                        order.Amount = Convert.ToDouble(dataStrings[i],CultureInfo.InvariantCulture);
                        break;
                    case (Rows.dt):
                        order.Dt = Convert.ToDateTime(dataStrings[i]);
                        break;
                    case (Rows.product_id):
                        order.ProductId = Convert.ToInt32(dataStrings[i]);
                        break;
                }
            }
            return order;
        }

    }
}
