using System;
using System.Collections.Generic;
using System.IO;

namespace DecisionTreeAssignment
{
    class Program
    {
        private static double acceptableError, sumLevels = 0.0;
        private static int numOfLeaves = 0, leaveDepth = 0, deepestLevel = 0;


        private static List<string> l_buying = new List<string>();
        private static List<string> l_maint = new List<string>();
        private static List<string> l_doors = new List<string>();
        private static List<string> l_persons = new List<string>();
        private static List<string> l_lug_boot = new List<string>();
        private static List<string> l_safety = new List<string>();
        private static List<string> l_acceptability = new List<string>();


        private static List<string> c_buying = new List<string>();
        private static List<string> c_maint = new List<string>();
        private static List<string> c_doors = new List<string>();
        private static List<string> c_persons = new List<string>();
        private static List<string> c_lug_boot = new List<string>();
        private static List<string> c_safety = new List<string>();
        private static List<string> c_acceptability = new List<string>();

        private static List<string> attributeList = new List<string>();


        static void Main(string[] args)
        {
            string path = @"C:\Users\austi\OneDrive\Desktop\GitHubProjects\DecisionTreeAssignment\DecisionTreeAssignment\Program.cs";

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');

                    l_buying.Add(values[0]);
                    l_maint.Add(values[1]);
                    l_doors.Add(values[2]);
                    l_persons.Add(values[3]);
                    l_lug_boot.Add(values[4]);
                    l_safety.Add(values[5]);
                    l_acceptability.Add(values[6]);
                }
            }

            c_buying = AddToList("vhigh", "high", "med", "low");
            c_maint = AddToList("vhigh", "high", "med", "low");
            c_doors = AddToList("2", "3", "4", "5more");
            c_persons = AddToList("2", "4", "more");
            c_lug_boot = AddToList("small", "med", "big");
            c_safety = AddToList("low", "med", "high");
            c_acceptability = AddToList("unacc", "acc", "good", "vgood");

            attributeList = AddToList("buying", "maint", "doors", "persons", "lug_boot", "safetly");




            


            Console.Write("Enter and acceptable error amount: ");
            acceptableError = Convert.ToDouble(Console.ReadLine());

            GenerateTree("", "");

            Console.WriteLine("Number Of Leaves: {0}", numOfLeaves);
            Console.WriteLine("Deepest Level: {0}", deepestLevel);
            Console.WriteLine("Number Of Leaves: {0}", sumLevels / numOfLeaves);

            Console.ReadKey();
        }






        private static void GenerateTree(string attribute, string criteria)
        {


            if (NodeEntropy(attribute, criteria, false) < acceptableError)
            {
                numOfLeaves++;
                sumLevels += leaveDepth;
                //leaveDepth = 0;
                return;
            }
            
            //deepestLevel = leaveDepth;
            //leaveDepth++;

            string currentAttribute = SplitAttribute();

            List<string> criteriaList = new List<string>();
            criteriaList.Clear();

            switch (currentAttribute)
            {
                case "buying":
                    criteriaList = c_buying;
                    break;
                case "maint":
                    criteriaList = c_maint;
                    break;
                case "doors":
                    criteriaList = c_doors;
                    break;
                case "persons":
                    criteriaList = c_persons;
                    break;
                case "lug_boot":
                    criteriaList = c_lug_boot;
                    break;
                case "safety":
                    criteriaList = c_safety;
                    break;
                default: 
                    break;
            }

            

            foreach (var item in criteriaList)
            {
                GenerateTree(currentAttribute, item);
            }

        }


        private static double NodeEntropy(string attribute, string criteria, bool splitEntropy)
        {
            int criteriaCount = 0;

            double unaccept = 0.0;
            double accept = 0.0;
            double good = 0.0;
            double vgood = 0.0;
            double entropy = 0.0;

            List<string> currentList = new List<string>();
            currentList.Clear();

            switch (attribute)
            {
                case "buying":
                    currentList = l_buying;
                    break;
                case "maint":
                    currentList = l_maint;
                    break;
                case "doors":
                    currentList = l_doors;                    
                    break;
                case "persons":
                    currentList = l_persons;
                    break;
                case "lug_boot":
                    currentList = l_lug_boot;
                    break;
                case "safety":
                    currentList = l_safety;
                    break;
                default:
                    currentList = l_acceptability;
                    break;
            }

            for (int i = 0; i < l_acceptability.Count; i++)
            {
                if (currentList[i] == criteria)
                {
                    criteriaCount++;
                    switch (l_acceptability[i])
                    {
                        case "unacc":
                            unaccept++;
                            break;
                        case "acc":
                            accept++;
                            break;
                        case "good":
                            good++;
                            break;
                        case "vgood":
                            vgood++;
                            break;
                        default:
                            break;
                    }
                }
            }

            unaccept = unaccept / criteriaCount;
            accept = accept / criteriaCount;
            good = good / criteriaCount;
            vgood = vgood / criteriaCount;

            //entropy = 0.0 - (unaccept * Math.Log(unaccept, 2)) - (accept * Math.Log(accept, 2)) - (good * Math.Log(good, 2)) - (vgood * Math.Log(vgood, 2));
            if (unaccept > 0)
            {
                entropy = entropy - (unaccept * Math.Log(unaccept, 2));
            }

            if (accept > 0)
            {
                entropy = entropy - (accept * Math.Log(accept, 2));
            }

            if (good > 0)
            {
                entropy = entropy - (good * Math.Log(good, 2));
            }

            if (vgood > 0)
            {
                entropy = entropy - (vgood * Math.Log(vgood, 2));
            }


            if (splitEntropy == true)
            {
                entropy = (criteriaCount / currentList.Count) * entropy;
            }

            if (attribute == "")
            {
                entropy = 10;
            }
            return entropy;
        }






        private static string SplitAttribute()
        {
            double minEnt = 2.0, entropy = 0.0;
            string bestAttribute = "";
            foreach (var attribute in attributeList)
            {
                List<string> criteriaList = new List<string>();
                criteriaList.Clear();

                switch (attribute)
                {
                    case "buying":
                        criteriaList = c_buying;
                        break;
                    case "maint":
                        criteriaList = c_maint;
                        break;
                    case "doors":
                        criteriaList = c_doors;
                        break;
                    case "persons":
                        criteriaList = c_persons;
                        break;
                    case "lug_boot":
                        criteriaList = c_lug_boot;
                        break;
                    case "safety":
                        criteriaList = c_safety;
                        break;
                    default:
                        break;
                }

                //Using this instead of SplitEntropy
                foreach (var item in criteriaList)
                {
                    entropy = entropy + (NodeEntropy(attribute, item, true));
                }
                
                if (entropy < minEnt)
                {
                    minEnt = entropy;
                    bestAttribute = attribute;
                }
            }
            attributeList.Remove(bestAttribute);
            return bestAttribute;


            
        }

        

        private static List<string> AddToList(params string[] list)
        {
            List<string> aList = new List<string>();
            aList.Clear();
            for (int i = 0; i < list.Length; i++)
            {
                aList.Add(list[i]);
            }
            return aList;
        }




    }
}
