using System.Collections.Generic;

namespace testJob.Model
{
    /// <summary>
    /// Продукт.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Название продукта.
        /// </summary>
        public string name { get; set; }

        public Product(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Product()
        {
        }

        public static List<Product> GetDemoProducts()
        {
            return new List<Product>
                   {
                       new Product(1, "A"),
                       new Product(2, "B"),
                       new Product(3, "C"),
                       new Product(4, "D"),
                       new Product(5, "E"),
                       new Product(6, "F"),
                       new Product(7, "G")
                   };
        }
    }
}