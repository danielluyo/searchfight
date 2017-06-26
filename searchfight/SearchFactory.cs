using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace searchfight
{
    /// <summary>
    /// Singleton, keeps a Search Engine Factory.
    /// </summary>
    public sealed class SearchFactory
    {
        private static readonly SearchFactory instance = new SearchFactory();

        public SearchConfig searchConfig { get; }
        public List<SearchResult> ResultList = new List<SearchResult>();

        static SearchFactory()
        {
        }

        private SearchFactory()
        {
            searchConfig = SearchConfig.Instance;
        }

        public static SearchFactory Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Starts the search
        /// </summary>
        /// <param name="searchTerms">An array of terms to look for</param>
        public void Search(string[] searchTerms)
        {            
            foreach (SearchEngine engine in searchConfig.SearchEngines)
            {
                using (WebClient wc = new WebClient() { Encoding = System.Text.Encoding.UTF8 })
                {
                    foreach (string term in searchTerms)
                    {
                        string response = wc.DownloadString(String.Format(engine.URL, term));

                        StringReader sr = new StringReader(response);
                        HTMLPaser parser = new HTMLPaser(sr);
                        HTMLElement result = parser.FindElementById(engine.FilterElement, engine.FilterId, engine.FilterCounter);

                        SearchResult res = new SearchResult();
                        res.engineName = engine.Name;
                        res.searchTerm = term;
                        res.searchCount = GetNumberValue(result.value);

                        ResultList.Add(res);
                    }
                }

            }
        }

        /// <summary>
        /// Finds out a number inside a string
        /// </summary>
        /// <param name="sourceStr">String that contains a number, like the search count</param>
        /// <returns>The number found</returns>
        private long GetNumberValue(string sourceStr)
        {
            string tmp = "";

            foreach (char c in sourceStr)
            {
                if (Char.IsNumber(c)) {
                    tmp += c;
                }                
            }
            return Int32.Parse(tmp);
        }

        /// <summary>
        /// Show the results as per requested by the contest
        /// </summary>
        public void ShowResults()
        {
            string termCount = "";

            // List Results for each search term
            foreach (string term in ResultList.Select(x => x.searchTerm).Distinct())
            {
                termCount = "";
                foreach (SearchResult res in ResultList.Where(r => r.searchTerm == term))
                {
                    termCount += String.Format("{0}: {1} ",  res.engineName, res.searchCount);
                }

                Console.WriteLine(String.Format("{0}: {1} ", term, termCount));
            }


            // List Max Search Count for Engine
            foreach (string engine in ResultList.Select(x => x.engineName).Distinct())
            {
                foreach (SearchResult res in ResultList.Where(r => r.searchCount == (ResultList.Where(e => e.engineName == engine).Max(count => count.searchCount))))
                {
                    Console.WriteLine(String.Format("{0} winner: {1} ", engine, res.searchTerm));
                }
            }

            // Total winner 
            foreach (SearchResult res in ResultList.Where(r => r.searchCount == (ResultList.Max(count => count.searchCount))))
            {
                Console.WriteLine(String.Format("Total winner: {0}: {1} ", res.searchTerm, res.searchCount));
            }
        }
    }
}
