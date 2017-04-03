﻿using System;
using System.Runtime.InteropServices;

namespace testJob.Model
{
    /// <summary>
    /// Продукт.
    /// </summary>
    public class Product
    {
        public int id { get; set; }

        public string name { get; set; }

        public Product(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        public Product()
        {
        }

    }
}