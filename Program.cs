using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace PhoneBook
{

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter file path");
            var filepath = Console.ReadLine();
            var content = new List<string>(File.ReadAllLines(filepath));
            Console.WriteLine("lease choose an ordering to sort: “Ascending” or “Descending”.");
            var ordering = Console.ReadLine();
            Console.WriteLine("Please choose criteria: “Name”, “Surname” or “PhoneNumberCode.");
            var criteria = Console.ReadLine();
            var contdict=new List<Dictionary<string,string>>();
            for (int i = 1; i < content.Count; i++)
            { 
                var line = content[i].Split(" ");
                var entity = new Dictionary<string, string>();
                entity.Add("Name", line[0]);
                entity.Add("Surname", line[1].Length > 2 ? line[1] : null);
                entity.Add("Seperator", line[line.Length - 2]);
                entity.Add("PhoneNumberCode", line.Last().Substring(0, 3));
                entity.Add("PhoneNumber", line.Last());
                contdict.Add(entity);
            }
            contdict.Sort(new StringArrayComparer(ordering, criteria));
            var wsn = contdict.Where(p => p["Surname"] == null).ToList();
            contdict.RemoveAll(wsn.Contains);
            contdict.AddRange(wsn);
            Console.WriteLine("File Structure:");
            foreach (var item in contdict)
            {
                Console.WriteLine($"{item["Name"]}{(item["Surname"] != null ?" "+item["Surname"] :"")} {item["Seperator"]} {item["PhoneNumber"]}");
            }
            Console.WriteLine("\nValidation:");
            for (int i=0; i < contdict.Count; i++)
            {
                var errormess = "";
                if (contdict[i]["PhoneNumber"].Length != 9)
                {
                    errormess += "phone number should be with 9 digits.";
                }
                if (contdict[i]["Seperator"]!="-" &&  contdict[i]["Seperator"] != ":")
                {
                    errormess += "the separator should be `:` or `-`.";
                }
                if (errormess != "")
                {
                    Console.WriteLine($"Line{i + 1}:"+errormess);
                }
               
            }


        }
    }
    public class StringArrayComparer : IComparer<Dictionary<string,string>>
    {
        private string ordering;
        private string criteria;
        public StringArrayComparer(string ordering, string criteria)
        {
            this.ordering = ordering;
            this.criteria   = criteria;
        }
        public int Compare(Dictionary<string, string>? x, Dictionary<string, string>? y)
        {
            if (ordering == "Ascending")
            {
                return String.Compare(x?[criteria], y?[criteria]);
            }
            else
            {
                return String.Compare(y?[criteria], x?[criteria]);
            }
        }
    }
}