namespace testJob.Model
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Идентификатор заказа.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int product_id { get; set; }

        /// <summary>
        /// Сумма заказа.
        /// </summary>
        public double amount { get; set; }

        /// <summary>
        /// Дата и время заказа. В формате unix timestamp.
        /// </summary>
        public int dt { get; set; }
    }
}