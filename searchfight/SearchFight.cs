using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchfight
{
    class SearchFight
    {
        public static SearchFactory searchFactory;

        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">The programs requires a list of search terms. If no arguments are provided a
        ///                    help text is provided.</param>
        static void Main(string[] args)
        {
            

            if (args.Length == 0)
            {
                ShowUsage();

            }
            else
            {
                searchFactory = SearchFactory.Instance;
                searchFactory.Search(args);

                Console.WriteLine(searchFactory.ToString());
            }
            Console.ReadLine();
        }

        /// <summary>
        /// Show help for the program.
        /// </summary>
        static private void ShowUsage()
        {
            Console.WriteLine("Usage: searchfight [search_term...] | [-?]");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("    -?            Shows this help.");            
        }
    }
}
