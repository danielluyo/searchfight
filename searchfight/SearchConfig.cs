using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace searchfight
{
    /// <summary>
    /// Singleton, load and maintains Search Configs
    /// </summary>
    public sealed class SearchConfig
    {
        private static readonly SearchConfig instance = new SearchConfig();

        private string path;
        private String confFileName = @"SearchEngines.conf";
        private SearchEngineList EngineList;

        public SearchEngineList SearchEngines { get { return EngineList; } }

        static SearchConfig()
        {
        }

        private SearchConfig()
        {
            EngineList = new SearchEngineList();
            path = AppDomain.CurrentDomain.BaseDirectory + @"\" + confFileName;
            LoadConfig();
        }

        public static SearchConfig Instance
        {
            get
            {
                return instance;
            }
        }


        /// <summary>
        /// Loads the Search Engines from file.
        /// If the file does not exists, e.g. on the first run, a default engine is created using Google.
        /// </summary>
        /// <returns></returns>
        public bool LoadConfig()
        {
            XmlDocument confDoc = new XmlDocument();         

            if (!File.Exists(path))
            {
                SearchEngine defaultEngine = new SearchEngine();
                defaultEngine.Name = "Google";
                //defaultEngine.URL = @"https://www.google.com";
                defaultEngine.URL = @"https://www.google.com/search?hl=en&q={0}";
                defaultEngine.FilterCounter = "resultStats";
                EngineList.Add(defaultEngine);
                SaveConfig();
            }

            XmlSerializer serialzer = new XmlSerializer(typeof(SearchEngineList));

            using (StreamReader sr = new StreamReader(path))
            {
                EngineList = (SearchEngineList)serialzer.Deserialize(sr);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SaveConfig()
        {
            XmlSerializer serialzer = new XmlSerializer(typeof(SearchEngineList));

            using (StreamWriter sw = new StreamWriter(path))
            {
                using (XmlWriter writer = XmlWriter.Create(sw))
                {
                    serialzer.Serialize(writer, EngineList);
                }
            }
            return false;
        }
    }

    [XmlRoot("SearchEngineList")]
    public class SearchEngineList : List<SearchEngine> { }
}
