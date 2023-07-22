using PC2Catalog.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Xml.Serialization;

namespace PC2Catalog
{
    public class Program
    {
        private static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static string filenameCategories = Path.Combine(baseDirectory, "Resources", "Categories.csv");
        private static string filenameProducts = Path.Combine(baseDirectory, "Resources", "Products.csv");

        private static string filenameJSON = "Catalog.json";
        private static string filenameXML = "Catalog.xml";

        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            List<Category> listCategory = ReadCategories();
            List<Product> listProduct = ReadProducts();

            if (listCategory.Count > 0 && listProduct.Count > 0)
            {
                listCategory.ForEach(x => {
                    x.Products = listProduct.Select(product => product).Where(product => product.CategoryId == x.Id).ToList();
                });

                CreateJSON(listCategory);
                CreateXML(listCategory);
            }
            else
            {
                Console.WriteLine("No se ha podido realizar la operación");
            }
        }

        private static List<Category> ReadCategories()
        {
            List<Category> listCategory = new List<Category>();
            string idError = null;

            try
            {
                if (File.Exists(filenameCategories))
                {
                    var readed = File.ReadAllLines(filenameCategories, Encoding.GetEncoding(1252)).Skip(1).Select(x => x.Split(';'));

                    foreach (var x in readed)
                    {
                        idError = x[0];

                        listCategory.Add(new Category
                        {
                            Id = int.Parse(x[0]),
                            Name = x[1],
                            Description = x[2]
                        });
                    }
                }
                else
                {
                    Console.WriteLine($"Archivo no encontrado: {filenameCategories}");
                }
            }
            catch (Exception ex)
            {
                listCategory.Clear();
                Console.WriteLine($"Error al leer el archivo {filenameCategories} ({ex.Message}). Id {idError}");
            }

            return listCategory;
        }
        private static List<Product> ReadProducts()
        {
            List<Product> listProduct = new List<Product>();
            string idError = null;

            try
            {
                if (File.Exists(filenameProducts))
                {
                    var readed = File.ReadAllLines(filenameProducts, Encoding.GetEncoding(1252)).Skip(1).Select(x => x.Split(';'));

                    foreach (var x in readed)
                    {
                        idError = x[0];

                        listProduct.Add(new Product
                        {
                            Id = int.Parse(x[0]),
                            CategoryId = int.Parse(x[1]),
                            Name = x[2],
                            Price = float.Parse(x[3])
                        });
                    }
                }
                else
                {
                    Console.WriteLine($"Archivo no encontrado: {filenameProducts}");
                }
            }
            catch (Exception ex)
            {
                listProduct.Clear();
                Console.WriteLine($"Error al leer el archivo {filenameProducts} ({ex.Message}). Id {idError}");
            }

            return listProduct;
        }

        private static void CreateJSON(List<Category> listCategory)
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            string json = JsonSerializer.Serialize(listCategory, options);

            File.WriteAllText(filenameJSON, json);

            if (File.Exists(filenameJSON))
            {
                FileInfo file = new FileInfo(filenameJSON);
                Console.WriteLine($"Creado JSON: {file.FullName}");
            }
        }
        private static void CreateXML(List<Category> listCategory)
        {
            XmlSerializer writer = new XmlSerializer(typeof(List<Category>));
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filenameXML))
            {
                writer.Serialize(file, listCategory);
            }

            if (File.Exists(filenameXML))
            {
                FileInfo file = new FileInfo(filenameXML);
                Console.WriteLine($"Creado XML: {file.FullName}");
            }
        }
    }
}
