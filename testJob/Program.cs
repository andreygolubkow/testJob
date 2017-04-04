using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using SQLite;

using testJob.Model;
using testJob.QueryHelpers;

namespace testJob
{
    static class Program
    {

        private static void Main(string[] args)
        {
            var ordersList = new List<Order>();
            if ( args.Length > 1 )
            {

                var fileInfo = new FileInfo("data.txt");
                if ( !fileInfo.Exists )
                {
                    Console.WriteLine("Ошибка в файле.");
                    return;
                }
                var fileReader = new StreamReader("data.txt");
                FileReader.Rows[] rowses = FileReader.ReadRowses(fileReader);
                /* Строки с данными считаются с 1.
                 * 0-я строка, строка со столбцами
                 */

                var linesCounter = 0;
                while ( !fileReader.EndOfStream )
                {
                    linesCounter++;
                    try
                    {
                        ordersList.Add(FileReader.ReadOrder(fileReader, rowses));
                    }
                    catch ( Exception exception )
                    {
                        Console.WriteLine("Ошибка в строке {0}." + exception.Message, linesCounter);
                    }

                }
                fileReader.Close();
            }
            //Положили данные в лист, начинаем работать с БД
            var db = new SQLiteConnection("db.sqlite", true);
            db.CreateTable<Order>();
            db.CreateTable<Product>();
            if ( db.Query<Product>("SELECT * FROM [product]").Count == 0 )
            {
                var products = new List<Product>
                               {
                                   new Product(1, "A"),
                                   new Product(2, "B"),
                                   new Product(3, "C"),
                                   new Product(4, "D"),
                                   new Product(5, "E"),
                                   new Product(6, "F"),
                                   new Product(7, "G")
                               };
                db.InsertAll(products);
            }
            db.CreateTable<Month>();
            if (db.Query<Month>("SELECT * FROM [Month]").Count == 0)
            {
                var months = new List<Month>
                               {
                                   new Month(1, "янв"),
                                   new Month(2, "фев"),
                                   new Month(3, "мар"),
                                   new Month(4, "апр"),
                                   new Month(5, "май"),
                                   new Month(6, "июн"),
                                   new Month(7, "июл"),
                                   new Month(8, "авг"),
                                   new Month(9, "сен"),
                                   new Month(10, "окт"),
                                   new Month(11, "ноя"),
                                   new Month(12, "дек")
                               };
                db.InsertAll(months);
            }
            db.InsertAll(ordersList);

            var query1 =
                    "SELECT [Product].name as `Name`,SUM([Order].amount) as `Sum`,COUNT(*) as `Count` FROM [Order],[Product] WHERE dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month')) AND [Order].product_id = [Product].id GROUP BY[Product].name; ";
            var query21 =
                    "SELECT [Product].name as `Продукт` FROM [Order],[Product] WHERE (dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month'))) AND (product_id NOT IN (SELECT product_id FROM [Order] WHERE (dt >= strftime('%s',date('now','start of month','-1 month')) AND dt < strftime('%s',date('now','start of month'))))) AND [Product].id = [Order].product_id GROUP BY [Product].name;";
            var query22 =
                    "SELECT [Product].name as `Продукт` FROM [Order],[Product] WHERE (dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month'))) AND (product_id NOT IN (SELECT product_id FROM [Order] WHERE (dt >= strftime('%s',date('now','start of month','-1 month')) AND dt < strftime('%s',date('now','start of month'))))) AND [Product].id = [Order].product_id GROUP BY [Product].name UNION SELECT[Product].name as `Продукт` FROM[Order],[Product] WHERE(dt >= strftime('%s', date('now','start of month','-1 month')) AND dt<strftime('%s', date('now','start of month'))) AND(product_id NOT IN (SELECT product_id FROM[Order] WHERE (dt >= strftime('%s', date('now','start of month')) AND dt<strftime('%s', date('now','start of month','+1 month'))))) AND[Product].id = [Order].product_id GROUP BY[Product].name;";
            var query3 = "SELECT [M].name||strftime(' %Y', date([Order].dt, 'unixepoch')) as `Период`,[Product].name as `Продукт`, [Order].amount as `Сумма`, ROUND([Order].amount/(SELECT SUM([Order].amount) FROM [Order],[Month] WHERE Month.id = strftime('%m', date([Order].dt, 'unixepoch')) AND [Month].id = [M].id GROUP BY strftime('%m', date([Order].dt, 'unixepoch')))*100,0) as `Доля` FROM [Order],[Product],[Month] as [M] WHERE [Product].id = [Order].product_id AND [M].id = strftime('%m', date([Order].dt, 'unixepoch')) GROUP BY[M].name HAVING(Max([Order].amount)) ORDER BY [M].id;";

            List<ProductItem> productItems = db.Query<ProductItem>(query1);
            Console.WriteLine("Продукт\tСумма\tКоличество");
            foreach (ProductItem item in productItems)
            {
                Console.WriteLine(item.Name+'\t'+Convert.ToString(item.Sum, CultureInfo.InvariantCulture)+'\t'+Convert.ToString(item.Count));
            }
#if DEBUG
            Console.ReadKey();   
#endif


            db.Dispose();
        }
    }
}
