using System.Collections.Generic;

using SQLite;

namespace testJob.Model
{
    /// <summary>
    /// Класс месяца для базы данных.
    /// </summary>
    public class Month
    {
        /// <summary>
        /// Уникальный идентификтор месяца. Совпадает с номером месяца.
        /// </summary>
        [Unique]
        public int id { get; set; }

        /// <summary>
        /// Название месяца.
        /// </summary>
        public string name { get; set; }

        public Month(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Month()
        {
            id = 0;
            name = "";
        }

        /// <summary>
        /// Список месяцев. От января до декабря.
        /// </summary>
        /// <returns>List из 12 месяцев.</returns>
        public static List<Month> GetMonthList()
        {
            return new List<Month>
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
        }

    }
}