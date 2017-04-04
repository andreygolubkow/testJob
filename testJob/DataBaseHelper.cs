using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

using testJob.Model;
using testJob.QueryHelpers;

namespace testJob
{
    public static class DataBaseHelper
    {

        private static void PrepareMonths(SQLiteConnection connection)
        {
            connection.CreateTable<Month>();
            if (connection.Query<Month>("SELECT * FROM [Month]").Count == 0)
            {
                connection.InsertAll(Month.GetMonthList());
            }
        }
        private static void PrepareProducts(SQLiteConnection connection)
        {
            connection.CreateTable<Product>();
            if ( connection.Query<Product>("SELECT * FROM [product]").Count != 0 )
            {
                return;
            }
            connection.InsertAll(Product.GetDemoProducts());
        }
        
        public static void PrepareTable(SQLiteConnection connection)
        {
            connection.CreateTable<Order>();
            PrepareProducts(connection);
            PrepareMonths(connection);
        }

        public static List<ProductItem> GetProductsInCurrentMonth(SQLiteConnection connection)
        {
            const string query = "SELECT [Product].name as `Name`,SUM([Order].amount) as `Sum`,COUNT(*) as `Count` FROM [Order],[Product] WHERE dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month')) AND [Order].product_id = [Product].id GROUP BY[Product].name; ";
            return connection.Query<ProductItem>(query);
        }

        public static List<Product> GetProductsOnlyInCurrentMonth(SQLiteConnection connection)
        {
            const string query = "SELECT [Product].name as `name` FROM [Order],[Product] WHERE (dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month'))) AND (product_id NOT IN (SELECT product_id FROM [Order] WHERE (dt >= strftime('%s',date('now','start of month','-1 month')) AND dt < strftime('%s',date('now','start of month'))))) AND [Product].id = [Order].product_id GROUP BY [Product].name;";
            return connection.Query<Product>(query);
        }

        public static List<Product> GetProductsOnlyInCurMonthPrevMonth(SQLiteConnection connection)
        {
            const string query = "SELECT [Product].name as `name` FROM [Order],[Product] WHERE (dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month'))) AND (product_id NOT IN (SELECT product_id FROM [Order] WHERE (dt >= strftime('%s',date('now','start of month','-1 month')) AND dt < strftime('%s',date('now','start of month'))))) AND [Product].id = [Order].product_id GROUP BY [Product].name UNION SELECT[Product].name as `Продукт` FROM[Order],[Product] WHERE(dt >= strftime('%s', date('now','start of month','-1 month')) AND dt<strftime('%s', date('now','start of month'))) AND(product_id NOT IN (SELECT product_id FROM[Order] WHERE (dt >= strftime('%s', date('now','start of month')) AND dt<strftime('%s', date('now','start of month','+1 month'))))) AND[Product].id = [Order].product_id GROUP BY[Product].name;";
            return connection.Query<Product>(query);
        }

        public static List<MonthlyData> GetPeriodsData(SQLiteConnection connection)
        {
            const string query = "SELECT [M].name||strftime(' %Y', date([Order].dt, 'unixepoch')) as `Period`,[Product].name as `Product`, [Order].amount as `Sum`, ROUND([Order].amount/(SELECT SUM([Order].amount) FROM [Order],[Month] WHERE Month.id = strftime('%m', date([Order].dt, 'unixepoch')) AND [Month].id = [M].id GROUP BY strftime('%m', date([Order].dt, 'unixepoch')))*100,0) as `Part` FROM [Order],[Product],[Month] as [M] WHERE [Product].id = [Order].product_id AND [M].id = strftime('%m', date([Order].dt, 'unixepoch')) GROUP BY[M].name HAVING(Max([Order].amount)) ORDER BY [M].id;";
            return connection.Query<MonthlyData>(query);
        }
    }
}
