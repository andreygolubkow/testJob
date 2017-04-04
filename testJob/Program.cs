#region

using System;
using System.Collections.Generic;
using System.Globalization;

using SQLite;

using testJob.Model;
using testJob.QueryHelpers;

#endregion

namespace testJob
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var db = new SQLiteConnection("db.sqlite", true);
            DataBaseHelper.PrepareTable(db);
            if ( args.Length > 0 )
            {
                string fileName = args[0];
                try
                {
                    FileReader.CheckFile(fileName);
                    var readExceptions = new List<Exception>();
                    IEnumerable<Order> ordersList = FileReader.ParseFile(fileName, readExceptions);
                    foreach (Exception ex in readExceptions)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    db.InsertAll(ordersList);
                }
                catch ( Exception exception )
                {
                    Console.WriteLine(exception.Message);
                    return;
                }
            }
            WriteFirstQuery(db);
            WriteSecondAQuery(db);
            WriteSecondBQuery(db);
            WriteThirdQuery(db);
            db.Dispose();
#if DEBUG
            Console.ReadKey();
#endif
        }

        /// <summary>
        ///     Выполнение запроса 1.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        private static void WriteFirstQuery(SQLiteConnection connection)
        {
            Console.WriteLine("Запрос 1. Вывести количество и сумму заказов по каждому продукту за текущей месяц.");
            List<ProductItem> productItems = DataBaseHelper.GetProductsInCurrentMonth(connection);
            Console.WriteLine("Продукт\tСумма\tКоличество");
            foreach (ProductItem item in productItems)
            {
                Console.WriteLine(item.Name + '\t' + Convert.ToString(item.Sum, CultureInfo.InvariantCulture) + '\t'
                                  + Convert.ToString(item.Count));
            }
        }

        /// <summary>
        ///     Выполнение запроса 2.а.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        private static void WriteSecondAQuery(SQLiteConnection connection)
        {
            Console.WriteLine(
                              "Запрос 2a. Вывести все продукты, которые были заказаны в текущем месяце, но которых не было в прошлом.");
            List<Product> productsList = DataBaseHelper.GetProductsOnlyInCurrentMonth(connection);
            Console.WriteLine("Продукт");
            foreach (Product product in productsList)
            {
                Console.WriteLine(product.name);
            }
        }

        /// <summary>
        ///     Выполнение запроса 2.b.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        private static void WriteSecondBQuery(SQLiteConnection connection)
        {
            Console.WriteLine(
                              "Запрос 2б. Вывести все продукты, которые были заказаны в текущем месяце, но которых не было в прошлом, и которые были в прошлом, но не было в текущем.");
            List<Product> productsList = DataBaseHelper.GetProductsOnlyInCurMonthPrevMonth(connection);
            Console.WriteLine("Продукт");
            foreach (Product product in productsList)
            {
                Console.WriteLine(product.name);
            }
        }

        /// <summary>
        ///     Выполнение запроса 3.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        private static void WriteThirdQuery(SQLiteConnection connection)
        {
            Console.WriteLine(
                              "Запрос 3. Помесячно вывести продукт, по которому была максимальная сумма заказов за этот период, сумму по этому продукту и его долю от общего объема за этот период.");
            List<MonthlyData> monthlyDataList = DataBaseHelper.GetPeriodsData(connection);
            Console.WriteLine("Период\tПродукт\tСумма\tДоля");
            foreach (MonthlyData dataItem in monthlyDataList)
            {
                Console.WriteLine(dataItem.Period + '\t' + dataItem.Product + '\t' + Convert.ToString(dataItem.Sum) + '\t'
                                  + Convert.ToString(dataItem.Part));
            }
        }
    }
}
