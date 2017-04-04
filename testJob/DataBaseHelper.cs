using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;

using testJob.Model;

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
    }
}
