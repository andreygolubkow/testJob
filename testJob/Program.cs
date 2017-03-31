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
                    var order = FileReader.ReadOrder(fileReader, rowses);
                }
                catch ( Exception exception )
                {
                    Console.WriteLine("Ошибка в строке {0}."+exception.Message,linesCounter);
                }
                
            }
            fileReader.Close();

        }
    }
}
