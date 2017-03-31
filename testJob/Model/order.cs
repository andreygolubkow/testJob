using System;

namespace testJob.Model
{
    /// <summary>
    /// Заказ.
    /// </summary>
    public class Order
    {
        private int _id;
        private int _productId;
        private double _amount;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if ( value < 0 )
                {
                    throw new ArgumentOutOfRangeException("Ошибка при присвоении Id.");
                }
                _id = value;
            }
        }

        /// <summary>
        /// Время после 1970-01-01 без учета таймзоны
        /// </summary>
        public DateTime Dt { get; set; }

        public int ProductId
        {
            get
            {
                return _productId;
            }
            set
            {
                if ( value < 0 )
                {
                    throw new ArgumentOutOfRangeException("Id продукта должен быть больше 0.");
                }
                _productId = value;
            }
        }

        /// <summary>
        /// Сумма заказа.
        /// </summary>
        public double Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                if ( value < 0 )
                {
                    throw new ArgumentOutOfRangeException("Ошибка. Сумма заказа не может быть меньше 0.");
                }
                _amount = value;
            }
        }
    }
}