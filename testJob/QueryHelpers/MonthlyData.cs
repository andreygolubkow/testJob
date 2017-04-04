namespace testJob.QueryHelpers
{
    /// <summary>
    /// Класс для отображения данных по периодам.
    /// </summary>
    public class MonthlyData
    {
        /// <summary>
        /// Период.
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// Название продукта.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Сумма заказа по продукту.
        /// </summary>
        public double Sum { get; set; }

        /// <summary>
        /// Доля продукта.
        /// </summary>
        public int Part { get; set; }
    }
}