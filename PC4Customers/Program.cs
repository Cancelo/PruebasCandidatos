using PC4Customers.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace PC4Customers
{
    public class Program
    {
        // Modificar con nombre de server y nombre bbdd. Debería ir en appsettings pero no lo veo necesario para este propósito
        private static string serverName = "_SERVER_NAME_";
        private static string dbName = "PC4Customers";

        private static string connectionString = $"Data Source={serverName};Initial Catalog={dbName};Integrated Security=true;";

        private static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string filenameCustomers = Path.Combine(baseDirectory, "Resources", "Customers.csv");

        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            List<Customer> listCustomers = ReadCustomers();

            if (listCustomers.Count > 0)
            {
                if (InsertCustomersData(listCustomers)) Console.WriteLine("Operación completada. Datos insertados");
            }
        }

        private static List<Customer> ReadCustomers()
        {
            List<Customer> listCustomers = new List<Customer>();
            string idError = null;

            try
            {
                if (File.Exists(filenameCustomers))
                {
                    var readed = File.ReadAllLines(filenameCustomers, Encoding.GetEncoding(1252)).Skip(1).Select(x => x.Split(';'));

                    foreach (var x in readed)
                    {
                        idError = x[0];

                        listCustomers.Add(new Customer
                        {
                            Id = x[0],
                            Name = x[1],
                            Address = x[2],
                            City = x[3],
                            Country = x[4],
                            PostalCode = x[5],
                            Phone = x[6]
                        });
                    }
                }
                else
                {
                    Console.WriteLine($"Archivo no encontrado: {filenameCustomers}");
                }
            }
            catch (Exception ex)
            {
                listCustomers.Clear();
                Console.WriteLine($"Error al leer el archivo {filenameCustomers} ({ex.Message}). Id {idError}");
            }

            return listCustomers;
        }

        private static bool InsertCustomersData(List<Customer> listCustomers)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlBulkCopy bulkCopy = new SqlBulkCopy(connection);
            bulkCopy.DestinationTableName = "Customers";
            bulkCopy.ColumnMappings.Add("Id", "Id");
            bulkCopy.ColumnMappings.Add("Name", "Name");
            bulkCopy.ColumnMappings.Add("Address", "Address");
            bulkCopy.ColumnMappings.Add("City", "City");
            bulkCopy.ColumnMappings.Add("Country", "Country");
            bulkCopy.ColumnMappings.Add("PostalCode", "PostalCode");
            bulkCopy.ColumnMappings.Add("Phone", "Phone");

            try
            {
                DataTable dataTable = ListToDataTable(listCustomers);
                bulkCopy.WriteToServer(dataTable);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al insertar datos: " + ex.Message);
                return false;
            }
        }

        private static DataTable ListToDataTable(List<Customer> listCustomers)
        {
            DataTable dataTable = new DataTable();

            try
            {
                dataTable.Columns.Add("Id", typeof(string));
                dataTable.Columns.Add("Name", typeof(string));
                dataTable.Columns.Add("Address", typeof(string));
                dataTable.Columns.Add("City", typeof(string));
                dataTable.Columns.Add("Country", typeof(string));
                dataTable.Columns.Add("PostalCode", typeof(string));
                dataTable.Columns.Add("Phone", typeof(string));

                foreach (var customer in listCustomers)
                {
                    dataTable.Rows.Add(
                        customer.Id,
                        customer.Name,
                        customer.Address,
                        customer.City,
                        customer.Country,
                        customer.PostalCode,
                        customer.Phone
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al convertir la Lista a DataTable: " + ex.Message);
            }

            return dataTable;
        }
    }
}
