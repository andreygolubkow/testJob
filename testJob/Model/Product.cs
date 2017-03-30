using System;
using System.Runtime.InteropServices;

namespace testJob.Model
{
    /// <summary>
    /// Продукт.
    /// </summary>
    public class Product
    {
        private int _id;
        private string _name;

        public Product(int id,string name)
        {
            Id = id;
            Name = name;
        }

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
                    throw new ArgumentOutOfRangeException("Ошибка в значении ID");
                    
                }
                _id = value;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if ( value.Length == 0 )
                {
                    throw new ArgumentException("Ошибка. Пустое название.");
                }
                _name = value;
            }
        }
    }
}