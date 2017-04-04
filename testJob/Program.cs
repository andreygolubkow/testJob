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
            if ( args.Length > 0 )
            {
                string fileName = args[0];
                FileReader.CheckFile(fileName);
                var readExceptions = new List<Exception>();
                IEnumerable<Order> ordersList = FileReader.ParseFile(fileName, readExceptions);
                /* Строки с данными считаются с 1.
                 * 0-я строка, строка со столбцами
                 */
                foreach (Exception ex in readExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
                db.InsertAll(ordersList);
            }
            //Положили данные в лист, начинаем работать с 
                
            

            const string query1 = "SELECT [Product].name as `Name`,SUM([Order].amount) as `Sum`,COUNT(*) as `Count` FROM [Order],[Product] WHERE dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month')) AND [Order].product_id = [Product].id GROUP BY[Product].name; ";
            const string query21 = "SELECT [Product].name as `name` FROM [Order],[Product] WHERE (dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month'))) AND (product_id NOT IN (SELECT product_id FROM [Order] WHERE (dt >= strftime('%s',date('now','start of month','-1 month')) AND dt < strftime('%s',date('now','start of month'))))) AND [Product].id = [Order].product_id GROUP BY [Product].name;";
            const string query22 = "SELECT [Product].name as `name` FROM [Order],[Product] WHERE (dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month'))) AND (product_id NOT IN (SELECT product_id FROM [Order] WHERE (dt >= strftime('%s',date('now','start of month','-1 month')) AND dt < strftime('%s',date('now','start of month'))))) AND [Product].id = [Order].product_id GROUP BY [Product].name UNION SELECT[Product].name as `Продукт` FROM[Order],[Product] WHERE(dt >= strftime('%s', date('now','start of month','-1 month')) AND dt<strftime('%s', date('now','start of month'))) AND(product_id NOT IN (SELECT product_id FROM[Order] WHERE (dt >= strftime('%s', date('now','start of month')) AND dt<strftime('%s', date('now','start of month','+1 month'))))) AND[Product].id = [Order].product_id GROUP BY[Product].name;";
            const string query3 = "SELECT [M].name||strftime(' %Y', date([Order].dt, 'unixepoch')) as `Period`,[Product].name as `Product`, [Order].amount as `Sum`, ROUND([Order].amount/(SELECT SUM([Order].amount) FROM [Order],[Month] WHERE Month.id = strftime('%m', date([Order].dt, 'unixepoch')) AND [Month].id = [M].id GROUP BY strftime('%m', date([Order].dt, 'unixepoch')))*100,0) as `Part` FROM [Order],[Product],[Month] as [M] WHERE [Product].id = [Order].product_id AND [M].id = strftime('%m', date([Order].dt, 'unixepoch')) GROUP BY[M].name HAVING(Max([Order].amount)) ORDER BY [M].id;";
            Console.WriteLine("Запрос 1. Вывести количество и сумму заказов по каждому продукту за текущей месяц.");
            List<ProductItem> productItems = db.Query<ProductItem>(query1);
            Console.WriteLine("Продукт\tСумма\tКоличество");
            foreach (ProductItem item in productItems)
            {
                Console.WriteLine(item.Name+'\t'+Convert.ToString(item.Sum, CultureInfo.InvariantCulture)+'\t'+Convert.ToString(item.Count));
            }
            Console.WriteLine("Запрос 2a. Вывести все продукты, которые были заказаны в текущем месяце, но которых не было в прошлом.");
            List<Product> productsList = db.Query<Product>(query21);
            Console.WriteLine("Продукт");
            foreach (Product product in productsList)
            {
                Console.WriteLine(product.name);
            }
            Console.WriteLine("Запрос 2б. Вывести все продукты, которые были заказаны в текущем месяце, но которых не было в прошлом, и которые были в прошлом, но не было в текущем.");
            productsList = db.Query<Product>(query22);
            Console.WriteLine("Продукт");
            foreach (Product product in productsList)
            {
                Console.WriteLine(product.name);
            }
            Console.WriteLine("Запрос 3. Помесячно вывести продукт, по которому была максимальная сумма заказов за этот период, сумму по этому продукту и его долю от общего объема за этот период.");
            List<MonthlyData> monthlyDataList = db.Query<MonthlyData>(query3);
            Console.WriteLine("Период\tПродукт\tСумма\tДоля");
            foreach (MonthlyData dataItem in monthlyDataList)
            {
                Console.WriteLine(dataItem.Period+'\t'+dataItem.Product+'\t'+Convert.ToString(dataItem.Sum)+'\t'+Convert.ToString(dataItem.Part));
            }
#if DEBUG
            Console.ReadKey();   
#endif
            db.Dispose();
        }
    }
}
