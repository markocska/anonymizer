using System;
using System.Collections.Generic;
using System.IO;

namespace TestDataGenerator
{
    //random string generálás ötletéhez forrás: https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings
    // nevek: https://github.com/smashew/NameDatabases/tree/master/NamesDatabases/first%20names
    // rand date ötlet: https://stackoverflow.com/questions/194863/random-date-in-c-sharp
    public class Program
    {
        private static string _charSetForIds = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static Random _random = new Random();

        private static DateTime _startDate = new DateTime(1910, 1, 1);
        private static int _dayRange;

        //Program()
        //{
        //    _startDate = new DateTime(1910, 1, 1); ;
        //    _dayRange = (DateTime.Today - _startDate).Days;

        //}

        static void Main(string[] args)
        {

            Program._startDate = new DateTime(1910, 1, 1); ;
            Program._dayRange = (DateTime.Today - _startDate).Days;

            var updateScript = new List<string>();
            updateScript.Add("insert into Customer values ");

            var firstNames = File.ReadAllLines(@"F:\OneDrive - edy\NameDatabases\NamesDatabases\first names\us.txt");
            var lastNames = File.ReadAllLines(@"F:\OneDrive - edy\NameDatabases\NamesDatabases\surnames\us.txt");
            var length = 100;

            for (int i = 0; i < length; i++)
            {

                //updateScript.Add(
                //    $"insert into dbo.Customer values (" +
                //        $"'{firstNames[_random.Next(firstNames.Length)]}', " +
                //        $"'{lastNames[_random.Next(lastNames.Length)]}', " +
                //        $"'{GenerateRandomIdString()}', " +
                //        $"'{GenerateRandomDateTime().ToShortDateString()}', " +
                //        $"'{firstNames[_random.Next(firstNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)]}', " +
                //        $"'{firstNames[_random.Next(firstNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)]} street {_random.Next(1, 200)}', " +
                //        $"{_random.Next(1, 100)}" +
                //        $");"
                //    );
                if (i != (length - 1))
                {
                    updateScript.Add("(" +
                            $"{i + 1}, " +
                            $"'{firstNames[_random.Next(firstNames.Length)]}', " +
                            $"'{lastNames[_random.Next(lastNames.Length)]}', " +
                            $"'{GenerateRandomIdString()}', " +
                            $"'{GenerateRandomDateTime().ToString("yyyy-MM-dd")}', " +
                            $"'{firstNames[_random.Next(firstNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)]}', " +
                            $"'{firstNames[_random.Next(firstNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)]} street {_random.Next(1, 200)}', " +
                            $"{_random.Next(1, 100)}" +
                            $"),");
                }
                else 
                {
                    updateScript.Add("(" +
                       $"{i + 1}, " +
                       $"'{firstNames[_random.Next(firstNames.Length)]}', " +
                       $"'{lastNames[_random.Next(lastNames.Length)]}', " +
                       $"'{GenerateRandomIdString()}', " +
                       $"'{GenerateRandomDateTime().ToString("yyyy-MM-dd")}', " +
                       $"'{firstNames[_random.Next(firstNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)]}', " +
                       $"'{firstNames[_random.Next(firstNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)]} street {_random.Next(1, 200)}', " +
                       $"{_random.Next(1, 100)}" +
                       $");");
                }
                //updateScript.Add(
                //        $"{i+1}," +
                //        $"{firstNames[_random.Next(firstNames.Length)]}, " +
                //        $"{lastNames[_random.Next(lastNames.Length)]}, " +
                //        $"{GenerateRandomIdString()}, " +
                //        $"{GenerateRandomDateTime().ToShortDateString()}, " +
                //        $"{firstNames[_random.Next(firstNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)]}, " +
                //        $"{firstNames[_random.Next(firstNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)] + " " + lastNames[_random.Next(lastNames.Length)]} street {_random.Next(1, 200)}, " +
                //        $"{_random.Next(1, 100)}" +
                //        $"{Environment.NewLine}");
            }

            var massUpdateScript = string.Join("", updateScript);

            File.WriteAllText(@"./insertScript.sql", massUpdateScript);
            Console.WriteLine("done");
            Console.ReadKey();
        }


        private static string GenerateRandomIdString()
        {
            int length = 10;

            var generatedCharArray = new char[length];
            for (int i = 0; i < generatedCharArray.Length; i++)
            {
                generatedCharArray[i] = _charSetForIds[_random.Next(_charSetForIds.Length)];
            }

            return new string(generatedCharArray);
        }

        private static DateTime GenerateRandomDateTime()
        {
            return _startDate.AddDays(_random.Next(_dayRange));
        }



    }
}
