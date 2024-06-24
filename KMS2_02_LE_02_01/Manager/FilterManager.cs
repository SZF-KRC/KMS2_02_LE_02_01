using KMS2_02_LE_02_01.Model;
using KMS2_02_LE_02_01.UploadData;
using System;
using System.Collections.Generic;
using System.Linq;



namespace KMS2_02_LE_02_01.Manager
{
    public class FilterManager
    {
        private List<Person> persons;

        /// <summary>
        /// Methode zum Hochladen und Drucken der Daten
        /// </summary>
        public void ToDo()
        {
            Upload();
            PrintData();
        }

        /// <summary>
        /// Methode zum Drucken der Daten basierend auf verschiedenen Abfragen
        /// </summary>
        private void PrintData()
        {
            if (persons != null)
            {
                // Persons older than 30
                PrintHeader("Query Syntax: Persons older than 30");
                var olderThan30Query = from person in persons
                                       where person.Age > 30
                                       select person;
                PrintPersonList(olderThan30Query);

                PrintHeader("Method Syntax: Persons older than 30");
                var olderThan30Method = persons.Where(p => p.Age > 30);
                PrintPersonList(olderThan30Method);

                // Number of people living in Berlin
                PrintHeader("Query Syntax: Number of people living in Berlin");
                var berlinResidentsCountQuery = (from person in persons
                                                 where person.City == "Berlin"
                                                 select person).Count();
                Console.WriteLine(berlinResidentsCountQuery);

                PrintHeader("Method Syntax: Number of people living in Berlin");
                var berlinResidentsCountMethod = persons.Count(p => p.City == "Berlin");
                Console.WriteLine(berlinResidentsCountMethod);

                // Alphabetically sorted list of female first names
                PrintHeader("Query Syntax: Alphabetically sorted list of female first names");
                var femaleNamesQuery = from person in persons
                                       where person.Gender == "Female"
                                       orderby person.Name
                                       select person.Name;
                PrintNameList(femaleNamesQuery);

                PrintHeader("Method Syntax: Alphabetically sorted list of female first names");
                var femaleNamesMethod = persons.Where(p => p.Gender == "Female")
                                               .OrderBy(p => p.Name)
                                               .Select(p => p.Name);
                PrintNameList(femaleNamesMethod);

                // Oldest person in the list
                PrintHeader("Query Syntax: Oldest person");
                var oldestPersonQuery = (from person in persons
                                         orderby person.Age descending
                                         select person).FirstOrDefault();
                PrintPerson(oldestPersonQuery);

                PrintHeader("Method Syntax: Oldest person");
                var oldestPersonMethod = persons.OrderByDescending(p => p.Age).FirstOrDefault();
                PrintPerson(oldestPersonMethod);

                // Last names of people living in Munich or Vienna
                PrintHeader("Query Syntax: Last names of people living in Munich or Vienna");
                var munichOrViennaLastNamesQuery = from person in persons
                                                   where person.City == "Munchen" || person.City == "Vienna"
                                                   select person.Surname;
                PrintNameList(munichOrViennaLastNamesQuery);

                PrintHeader("Method Syntax: Last names of people living in Munich or Vienna");
                var munichOrViennaLastNamesMethod = persons.Where(p => p.City == "Munchen" || p.City == "Vienna")
                                                           .Select(p => p.Surname);
                PrintNameList(munichOrViennaLastNamesMethod);

                // Number of people per gender
                PrintHeader("Query Syntax: Number of people per gender");
                var groupByGenderQuery = from person in persons
                                         group person by person.Gender into genderGroup
                                         select new { Gender = genderGroup.Key, Count = genderGroup.Count() };
                PrintGroupCount(groupByGenderQuery);

                PrintHeader("Method Syntax: Number of people per gender");
                var groupByGenderMethod = persons.GroupBy(p => p.Gender)
                                                 .Select(g => new { Gender = g.Key, Count = g.Count() });
                PrintGroupCount(groupByGenderMethod);

                // Average number of letters in first names
                PrintHeader("Query Syntax: Average number of letters in first names");
                var averageNameLengthQuery = (from person in persons
                                              select person.Name.Length).Average();
                Console.WriteLine(averageNameLengthQuery.ToString("F2"));

                PrintHeader("Method Syntax: Average number of letters in first names");
                var averageNameLengthMethod = persons.Average(p => p.Name.Length);
                Console.WriteLine(averageNameLengthMethod.ToString("F2"));

                // Cities where only people aged between 20 and 40 live
                PrintHeader("Query Syntax: Cities where only people aged between 20 and 40 live");
                var citiesWithAge20To40Query = from person in persons
                                               where person.Age >= 20 && person.Age <= 40
                                               group person by person.City into cityGroup
                                               where cityGroup.All(p => p.Age >= 20 && p.Age <= 40)
                                               orderby cityGroup.Key
                                               select cityGroup.Key;
                PrintNameList(citiesWithAge20To40Query);

                PrintHeader("Method Syntax: Cities where only people aged between 20 and 40 live");
                var citiesWithAge20To40Method = persons.Where(p => p.Age >= 20 && p.Age <= 40)
                                                       .GroupBy(p => p.City)
                                                       .Where(g => g.All(p => p.Age >= 20 && p.Age <= 40))
                                                       .Select(g => g.Key)
                                                       .OrderBy(city => city);
                PrintNameList(citiesWithAge20To40Method);

                // List of last names shared by at least one male and one female
                PrintHeader("Query Syntax: Last names shared by at least one male and one female");
                var sharedLastNamesQuery = (from person in persons
                                            group person by person.Surname into surnameGroup
                                            where surnameGroup.Any(p => p.Gender == "Male") && surnameGroup.Any(p => p.Gender == "Female")
                                            orderby surnameGroup.Key
                                            select surnameGroup.Key).ToList();
                PrintNameList(sharedLastNamesQuery);

                PrintHeader("Method Syntax: Last names shared by at least one male and one female");
                var sharedLastNamesMethod = persons.GroupBy(p => p.Surname)
                                                   .Where(g => g.Any(p => p.Gender == "Male") && g.Any(p => p.Gender == "Female"))
                                                   .Select(g => g.Key)
                                                   .OrderBy(surname => surname)
                                                   .ToList();
                PrintNameList(sharedLastNamesMethod);

                // Oldest person per city
                PrintHeader("Query Syntax: Oldest person per city");
                var oldestPersonPerCityQuery = from person in persons
                                               group person by person.City into cityGroup
                                               let oldest = cityGroup.OrderByDescending(p => p.Age).FirstOrDefault()
                                               select new { City = cityGroup.Key, OldestPerson = oldest };
                PrintOldestPersonPerCity(oldestPersonPerCityQuery);

                PrintHeader("Method Syntax: Oldest person per city");
                var oldestPersonPerCityMethod = persons.GroupBy(p => p.City)
                                                       .Select(cityGroup => new
                                                       {
                                                           City = cityGroup.Key,
                                                           OldestPerson = cityGroup.OrderByDescending(p => p.Age).FirstOrDefault()
                                                       });
                PrintOldestPersonPerCity(oldestPersonPerCityMethod);

                Console.ReadKey();
            }
        }

        /// <summary>
        /// Methode zum Hochladen von Daten
        /// </summary>
        private void Upload()
        {
            persons = UploadCSV.Upload();
        }

        /// <summary>
        /// Methode zum Drucken eines Headers
        /// </summary>
        /// <param name="title">Titel des Headers</param>
        private void PrintHeader(string title)
        {
            Console.WriteLine();
            Console.WriteLine("=".PadRight(50, '='));
            Console.WriteLine(title);
            Console.WriteLine("=".PadRight(50, '='));
        }

        /// <summary>
        /// Methode zum Drucken einer Liste von Personen
        /// </summary>
        /// <param name="persons">Liste der Personen</param>
        private void PrintPersonList(IEnumerable<Person> persons)
        {
            foreach (var person in persons)
            {
                Console.WriteLine($"{person.Name.PadRight(10)} {person.Surname.PadRight(10)} {person.Age.ToString().PadRight(3)} {person.City.PadRight(10)} {person.Gender.PadRight(6)}");
            }
        }

        /// <summary>
        /// Methode zum Drucken einer einzelnen Person
        /// </summary>
        /// <param name="person">Person</param>
        private void PrintPerson(Person person)
        {
            if (person != null)
            {
                Console.WriteLine($"{person.Name.PadRight(10)} {person.Surname.PadRight(10)} {person.Age.ToString().PadRight(3)} {person.City.PadRight(10)} {person.Gender.PadRight(6)}");
            }
        }

        /// <summary>
        /// Methode zum Drucken einer Liste von Namen
        /// </summary>
        /// <param name="names">Liste der Namen</param>
        private void PrintNameList(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                Console.WriteLine(name);
            }
        }

        /// <summary>
        /// Methode zum Drucken der Gruppenzählung
        /// </summary>
        /// <param name="groupCounts">Gruppenzählung</param>
        private void PrintGroupCount(IEnumerable<dynamic> groupCounts)
        {
            foreach (var group in groupCounts)
            {
                Console.WriteLine($"{group.Gender.PadRight(6)}: {group.Count}");
            }
        }

        /// <summary>
        /// Methode zum Drucken der ältesten Personen pro Stadt
        /// </summary>
        /// <param name="oldestPersonsPerCity">Älteste Personen pro Stadt</param>
        private void PrintOldestPersonPerCity(IEnumerable<dynamic> oldestPersonsPerCity)
        {
            foreach (var item in oldestPersonsPerCity)
            {
                var person = item.OldestPerson;
                Console.WriteLine($"{item.City.PadRight(10)}: {person.Name.PadRight(10)} {person.Surname.PadRight(10)} {person.Age}");
            }
        }
    }
}


