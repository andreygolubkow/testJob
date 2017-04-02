using System;

using SQLite;

namespace testJob.Model
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public class Order
    {
        [NotNull]
        public int id { get; set; }
        [NotNull]
        public int product_id { get; set; }
        [NotNull]
        public double amount { get; set; }
        [NotNull]
        public DateTime dt { get; set; }
    }
}