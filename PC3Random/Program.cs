using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PC3Random
{
    public class Program
    {
        private static int totalThreads = 1;
        private static int totalNumbers = 10000000 / totalThreads;
        private static HashSet<int> numbers = new HashSet<int>(totalNumbers);
        private static string path = "";

        public static void Main(string[] args)
        {
            var timer = new Stopwatch();

            timer.Start();
            Console.WriteLine($"[{timer.Elapsed}] Inicio");

            GenerateNumbers();
            Console.WriteLine($"[{timer.Elapsed}] Números generados: {numbers.Count} ({totalNumbers - numbers.Distinct().Count()} repetidos)");

            CreateFile();
            Console.WriteLine($"[{timer.Elapsed}] Archivo creado: {path}");

            timer.Stop();
            Console.WriteLine($"[{timer.Elapsed}] Fin");
        }

        private static void GenerateNumbers()
        {
            Random random = new Random();
            int index = 0;
            while (index < totalNumbers)
            {
                if (numbers.Add(random.Next(int.MaxValue))) index++;
            }
        }

        private static void CreateFile()
        {
            try
            {
                File.WriteAllText("output.txt", String.Join(",", numbers));

                if (File.Exists("output.txt"))
                {
                    FileInfo file = new FileInfo("output.txt");
                    path = file.FullName;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error al crear el archivo: {ex.Message}");
            }
        }
    }
}
