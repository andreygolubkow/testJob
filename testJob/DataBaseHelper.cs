#region

using System.Collections.Generic;

using SQLite;

using testJob.Model;
using testJob.QueryHelpers;

#endregion

namespace testJob
{
    /// <summary>
    ///     Класс для работы с базой данных.
    /// </summary>
    public static class DataBaseHelper
    {
        /// <summary>
        ///     Подготовка данных о месяцах.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        private static void PrepareMonths(SQLiteConnection connection)
        {
            connection.CreateTable<Month>();
            if ( connection.Query<Month>("SELECT * FROM [Month]").Count == 0 )
            {
                connection.InsertAll(Month.GetMonthList());
            }
        }

        /// <summary>
        ///     Подготовка данных о продуктах.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        private static void PrepareProducts(SQLiteConnection connection)
        {
            connection.CreateTable<Product>();
            if ( connection.Query<Product>("SELECT * FROM [product]").Count != 0 )
            {
                return;
            }
            connection.InsertAll(Product.GetDemoProducts());
        }

        /// <summary>
        ///     Подготовка таблиц в базе данных.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        public static void PrepareTable(SQLiteConnection connection)
        {
            connection.CreateTable<Order>();
            PrepareProducts(connection);
            PrepareMonths(connection);
        }

        /// <summary>
        ///     Получение продуктов за текущий месяц.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        /// <returns>Список продуктов с данными.</returns>
        public static List<ProductItem> GetProductsInCurrentMonth(SQLiteConnection connection)
        {
            const string query =
                    "SELECT [Product].name as `Name`,SUM([Order].amount) as `Sum`,COUNT(*) as `Count` FROM [Order],[Product] WHERE dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month')) AND [Order].product_id = [Product].id GROUP BY[Product].name; ";
            return connection.Query<ProductItem>(query);
        }

        /// <summary>
        ///     Получение продуктов только в текущем месяце.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        /// <returns>Список продуктов.</returns>
        public static List<Product> GetProductsOnlyInCurrentMonth(SQLiteConnection connection)
        {
            const string query =
                    "SELECT [Product].name as `name` FROM [Order],[Product] WHERE (dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month'))) AND (product_id NOT IN (SELECT product_id FROM [Order] WHERE (dt >= strftime('%s',date('now','start of month','-1 month')) AND dt < strftime('%s',date('now','start of month'))))) AND [Product].id = [Order].product_id GROUP BY [Product].name;";
            return connection.Query<Product>(query);
        }

        /// <summary>
        ///     Получение продуктов которые были либо только в текущем месяце, либо только в прошлом.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        /// <returns>Список продуктов.</returns>
        public static List<Product> GetProductsOnlyInCurMonthPrevMonth(SQLiteConnection connection)
        {
            const string query =
                    "SELECT [Product].name as `name` FROM [Order],[Product] WHERE (dt >= strftime('%s',date('now','start of month')) AND dt < strftime('%s',date('now','start of month','+1 month'))) AND (product_id NOT IN (SELECT product_id FROM [Order] WHERE (dt >= strftime('%s',date('now','start of month','-1 month')) AND dt < strftime('%s',date('now','start of month'))))) AND [Product].id = [Order].product_id GROUP BY [Product].name UNION SELECT[Product].name as `Продукт` FROM[Order],[Product] WHERE(dt >= strftime('%s', date('now','start of month','-1 month')) AND dt<strftime('%s', date('now','start of month'))) AND(product_id NOT IN (SELECT product_id FROM[Order] WHERE (dt >= strftime('%s', date('now','start of month')) AND dt<strftime('%s', date('now','start of month','+1 month'))))) AND[Product].id = [Order].product_id GROUP BY[Product].name;";
            return connection.Query<Product>(query);
        }

        /// <summary>
        ///     Получение данных о продуктах по периодам.
        /// </summary>
        /// <param name="connection">Открытое подключение к базе данных.</param>
        /// <returns>Список данных по периодам.</returns>
        public static List<MonthlyData> GetPeriodsData(SQLiteConnection connection)
        {
            const string query =
                    "SELECT M.name||Y.year as `Period`,[Product].name as `Product`,(SELECT maxAmount FROM (SELECT [Order].product_id,SUM([Order].amount) as `maxAmount` FROM [Order] WHERE strftime('%m',date([Order].dt, 'unixepoch')) = [M].id AND strftime('%Y',date([Order].dt, 'unixepoch')) = CAST([Y].year as string)  GROUP BY [Order].product_id ORDER BY maxAmount DESC LIMIT 1)) as `Sum`,  ROUND(((SELECT maxAmount FROM (SELECT [Order].product_id,SUM([Order].amount) as `maxAmount` FROM [Order] WHERE strftime('%m',date([Order].dt, 'unixepoch')) = [M].id AND strftime('%Y',date([Order].dt, 'unixepoch')) = CAST([Y].year as string)  GROUP BY [Order].product_id ORDER BY maxAmount DESC LIMIT 1)))/   (SELECT SUM([Order].amount) as `sum` FROM [Order] WHERE strftime('%m',date([Order].dt, 'unixepoch')) = [M].id AND strftime('%Y',date([Order].dt, 'unixepoch')) = CAST([Y].year as string) )  *100,0) as `Part` FROM  (SELECT strftime(' %Y', date([Order].dt, 'unixepoch')) as year FROM [Order] GROUP BY year) as [Y], [Month] as [M], [Product] WHERE  NOT(`Product` is NULL) AND [Product].id =  (SELECT product_id FROM (SELECT [Order].product_id,SUM([Order].amount) as `maxAmount` FROM [Order] WHERE strftime('%m',date([Order].dt, 'unixepoch')) = [M].id AND strftime('%Y',date([Order].dt, 'unixepoch')) = CAST([Y].year as string)  GROUP BY [Order].product_id ORDER BY maxAmount DESC LIMIT 1));";
            return connection.Query<MonthlyData>(query);
        }
    }
}
