using System;
using System.Collections.Generic;
using System.IO;
using SQLite;
using System.Text;

using testJob.Model;

namespace testJob
{
    static class Program
    {

        private static void Main(string[] args)
        {
            var fileReader = new StreamReader("data.txt");
            string[] rowses = fileReader.ReadLine()?.ToLower().Split('\t');
            /* Строки с данными считаются с 1.
             * 0-я строка, строка со столбцами
             */
            int linesCounter = 0;
            while ( !fileReader.EndOfStream )
            {
                linesCounter++;
                try
                {
                    string[] order = FileReader.ReadOrder(fileReader);
                    commandString.Insert(commandString.Length,
                                         $"INSERT INTO [order] ({rowses[0]},{rowses[1]},{rowses[2]},{rowses[3]}) VALUES ('{order[0]}','{order[1]}','{order[2]}','{order[3]}');\n");
                }
                catch ( Exception exception )
                {
                    Console.WriteLine("Ошибка в строке {0}." + exception.Message, linesCounter);
                }
            }
            fileReader.Close();
            using (var db = new SQLiteConnection("db.sqlite",SQLiteOpenFlags.Create, true))
            {
                
            }

        }
    }
}
