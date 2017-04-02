using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;

using testJob.Model;

namespace testJob
{
    public class SqliteManager
    {
        private SQLiteConnection _connection;
        /// <summary>
        /// Открытие существующего или создания нового файла БД.
        /// </summary>
        /// <param name="fileName">Путь к файлу.</param>
        public SqliteManager(string fileName)
        {
            if ( !File.Exists(fileName) )
            {
                SQLiteConnection.CreateFile(fileName);
                if ( !File.Exists(fileName) )
                {
                    throw new FileNotFoundException("Проблема при создании файла.");
                }
            }
            _connection = new SQLiteConnection($"Data Source={fileName}");
            _connection.Open();
            var command = new SQLiteCommand("SELECT name FROM sqlite_master WHERE type = 'table'",_connection);
            SQLiteDataReader reader = command.ExecuteReader();
            var isOrderTableExist = false;
            var isProductTableExist = false;
            foreach (DbDataRecord record in reader)
            {
                isOrderTableExist = (string)record["name"] == "order" || isOrderTableExist;
                isProductTableExist = (string)record["name"] == "product" || isProductTableExist;
                if ( isOrderTableExist && isProductTableExist )
                {
                    break;
                }
            }
            if ( !isProductTableExist )
            {
                CreateTableProduct(_connection);
            }
            if ( !isOrderTableExist )
            {
                CreateTableOrder(_connection);
            }
            _connection.Close();
        }

        private static void CreateTableProduct(SQLiteConnection openedConnection)
        {
            var command = new SQLiteCommand("CREATE TABLE `product` (id INTEGER PRIMARYKEY, name TEXT);",openedConnection);
            command.ExecuteNonQuery();
        }

        private static void CreateTableOrder(SQLiteConnection openedConnection)
        {
            var command = new SQLiteCommand("CREATE TABLE `order` (id INTEGER PRIMARYKEY, dt DATETIME, product_id INTEGER, amount REAL);", openedConnection);
            command.ExecuteNonQuery();
        }

        public List<Order> OrderSqlQuery(string query)
        {
            _connection.Open();
            var command = new SQLiteCommand(query);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Order> list = (from DbDataRecord record in reader
                        select new Order
                               {
                                   Amount = (double)record["amount"],
                                   Id = (int)record["id"],
                                   ProductId = (int)record["product_id"],
                                   Dt = (DateTime)record["dt"]
                               }).ToList();
            _connection.Close();
            return list;
        }

        public List<Product> ProductSqlQuery(string query)
        {
            _connection.Open();
            var command = new SQLiteCommand(query,_connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Product> list = (from DbDataRecord record in reader
                        select new Product
                               {
                                   Id = (int)record["id"],
                                   Name = (string)record["name"]
                               }).ToList();
            _connection.Close();
            return list;
        }

        public void SqlQuery(string query)
        {
            _connection.Open();
            (new SQLiteCommand(query, _connection)).ExecuteNonQuery();
            _connection.Close();
        }

        public SQLiteDataReader ReaderSqlQuery(string query)
        {
            _connection.Open();
            var command = new SQLiteCommand(query,_connection);
            SQLiteDataReader reader = command.ExecuteReader();
            _connection.Close();
            return reader;
        }

    }
}