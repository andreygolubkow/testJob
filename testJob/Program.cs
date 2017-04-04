using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using SQLite;

using testJob.Model;
using testJob.QueryHelpers;

namespace testJob
{
    static class Program
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
            WriteSecondQuery(db);
            WriteThirdQuery(db);
            WriteFourthQuery(db);
            db.Dispose();
#if DEBUG
            Console.ReadKey();   
#endif
        }

        private static void WriteFirstQuery(SQLiteConnection connection)
        {
            Console.WriteLine("Запрос 1. Вывести количество и сумму заказов по каждому продукту за текущей месяц.");
            List<ProductItem> productItems = DataBaseHelper.GetProductsInCurrentMonth(connection);
            Console.WriteLine("Продукт\tСумма\tКоличество");
            foreach (ProductItem item in productItems)
            {
                Console.WriteLine(item.Name + '\t' + Convert.ToString(item.Sum, CultureInfo.InvariantCulture) + '\t' + Convert.ToString(item.Count));
            }
        }

        private static void WriteSecondQuery(SQLiteConnection connection)
        {
            Console.WriteLine("Запрос 2a. Вывести все продукты, которые были заказаны в текущем месяце, но которых не было в прошлом.");
            List<Product> productsList = DataBaseHelper.GetProductsOnlyInCurrentMonth(connection);
            Console.WriteLine("Продукт");
            foreach (Product product in productsList)
            {
                Console.WriteLine(product.name);
            }
        }

        private static void WriteThirdQuery(SQLiteConnection connection)
        {
            Console.WriteLine("Запрос 2б. Вывести все продукты, которые были заказаны в текущем месяце, но которых не было в прошлом, и которые были в прошлом, но не было в текущем.");
            List<Product> productsList = DataBaseHelper.GetProductsOnlyInCurMonthPrevMonth(connection);
            Console.WriteLine("Продукт");
            foreach (Product product in productsList)
            {
                Console.WriteLine(product.name);
            }
        }

        private static void WriteFourthQuery(SQLiteConnection connection)
        {
            Console.WriteLine("Запрос 3. Помесячно вывести продукт, по которому была максимальная сумма заказов за этот период, сумму по этому продукту и его долю от общего объема за этот период.");
            List<MonthlyData> monthlyDataList = DataBaseHelper.GetPeriodsData(connection);
            Console.WriteLine("Период\tПродукт\tСумма\tДоля");
            foreach (MonthlyData dataItem in monthlyDataList)
            {
                Console.WriteLine(dataItem.Period + '\t' + dataItem.Product + '\t' + Convert.ToString(dataItem.Sum) + '\t' + Convert.ToString(dataItem.Part));
            }
        }
    }
}
