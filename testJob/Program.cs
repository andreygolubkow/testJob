using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;

using testJob.Model;

namespace testJob
{
    static class Program
    {

        private static void Main(string[] args)
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
            var sqlite = new SqliteManager("db.sqlite");
            string longQuery = $"INSERT INTO [order] (id,dt,product_id,amount) VALUES ";
            while ( !fileReader.EndOfStream )
            {
                linesCounter++;
                try
                {
                    Order order = FileReader.ReadOrder(fileReader, rowses);
                   // if ( linesCounter == 1 )
                   // {
                    //    longQuery = longQuery + $"('{order.Id}','{order.Dt}','{order.ProductId}','{order.Amount}')";
                   // }
                    //else
                    //{
                        longQuery = longQuery = longQuery + $",('{order.Id}','{order.Dt}','{order.ProductId}','{order.Amount}')";
                   // }
                }
                catch ( Exception exception )
                {
                    Console.WriteLine("Ошибка в строке {0}."+exception.Message,linesCounter);
                }
                
            }
            //sqlite.SqlQuery(longQuery+";");
            fileReader.Close();

        }
    }
}
