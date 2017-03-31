using System;
using System.Collections.Generic;
using System.IO;

using testJob.Model;

namespace testJob
{
    static class Program
    {
        private static void Main(string[] args)
        {
            var fileInfo = new FileInfo(args[1]);
            if ( !fileInfo.Exists )
            {
                Console.WriteLine("Ошибка в файле.");
                return;
            }
            var fileReader = new StreamReader(args[1]);
            var ordersList = new List<Order>();
            FileReader.Rows[] rowses = FileReader.ReadRowses(fileReader);
            while ( !fileReader.EndOfStream )
            {
                ordersList.Add(FileReader.ReadOrder(fileReader,rowses));
            }

        }
    }
}
