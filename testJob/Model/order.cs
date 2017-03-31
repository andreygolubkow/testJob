using System;

namespace testJob.Model
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public struct Order
    {
        public int Id;
        public int ProductId;
        public double Amount;
        public DateTime Dt;
    }
}