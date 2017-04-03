using System;

namespace testJob.Model
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public class Order
    {
        public int id { get; set; }
        public int product_id { get; set; }
        public double amount { get; set; }
        public int dt { get; set; }//UnixTimestamp
    }
}