using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OireachtasAPI
{

    //Class for display pick lists and results from JToken queries
    internal class CLI
    {
        /// <summary>
        /// displays results on the console and blocks until user presses a key
        /// </summary>
        /// <param name="results">a list of JTokens to be displayed to the user</param>
        public void displayResults(List<JToken> results)
        {
            foreach (var r in results)
            {
                Console.WriteLine(r.ToString());
            }
            Console.WriteLine("Press any button to continue.");
            Console.ReadLine();
        }

        /// <summary>
        /// Askes user to enter a date range (inclusive) and returns parsed dates with out params
        /// </summary>
        /// <param name="since">low bound of date range</param>
        /// <param name="until">high bound of date range</param>
        /// <returns>bool indicating if dates could be parsed</returns>
        public bool askForDates(out DateTime since, out DateTime until)
        {
            bool parsedSince = true;
            bool parsedUntil = true;

            string sinceDateStr = askForInput("Type in a date since...");
            string untilDateStr = askForInput("Type in a date until...");

            parsedSince = DateTime.TryParse(sinceDateStr, out since);
            parsedUntil = DateTime.TryParse(untilDateStr, out until);

            if (!parsedSince || !parsedUntil)
            {
                Console.WriteLine("Unable to parse dates. Press any key to close");
                Console.ReadLine();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Askes user for input
        /// </summary>
        /// <param name="message">message to display when asking for input</param>
        /// <returns>returns input</returns>
        public string askForInput(string message = "Waiting for input")
        {
            Console.Clear();
            Console.WriteLine(message);
            string input = Console.ReadLine();
            return input;           
        }

        /// <summary>
        /// displays a pick list to the user
        /// </summary>
        /// <param name="list">list of items to display to user</param>
        /// <param name="type">optional type for flavor text</param>
        /// <returns>returns index of selected item</returns>
        public int pickList(IEnumerable<string> list, string type = "Item")
        {
            int oldSlection = -99;
            int selection = 0;
            ConsoleKey pressedKey;
            do
            {
                if (oldSlection != selection)
                {
                    UpdateMenu(list, type, selection);
                    oldSlection = selection;
                }
                pressedKey = Console.ReadKey().Key;

                if (pressedKey == ConsoleKey.DownArrow && selection + 1 < list.Count())
                {
                    selection++;
                }
                else if (pressedKey == ConsoleKey.UpArrow && selection - 1 >= 0)
                {
                    selection--;
                }

            } while (pressedKey != ConsoleKey.Enter);
            Console.Clear();

            return selection;
        }
        /// <summary>
        /// updates menue
        /// </summary>
        /// <param name="list">list of strings to print</param>
        /// <param name="type">string for flavor text</param>
        /// <param name="index">index indidicating selected item</param>
        private void UpdateMenu(IEnumerable<string> list, string type, int index)
        {
            Console.Clear();
            Console.WriteLine($"Please Select a {type}...");

            foreach (var item in list)
            {
                bool isSelected = item == list.ElementAt(index);
                if (isSelected)
                    DrawSelectedMenuItem(item);
                else
                    Console.WriteLine($"  {item}");
            }
        }
        /// <summary>
        /// draws slected menue item
        /// </summary>
        /// <param name="item">item to draw</param>
        private void DrawSelectedMenuItem(string item)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"> {item}");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }


    }
}
