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

        public void Search(string[] searchTerms)
        {
            
            foreach (SearchEngine engine in searchConfig.SearchEngines)
            {
                using (WebClient wc = new WebClient() { Encoding = System.Text.Encoding.UTF8 })
                {
                    string response = wc.DownloadString(String.Format(engine.URL, searchTerms[0]));

                    StringReader sr = new StringReader(response);
                    HTMLPaser hp = new HTMLPaser(sr);
                    hp.FindTagById("div", "resultStats");
                }

            }

        }
    }
}
