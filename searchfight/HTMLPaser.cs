using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchfight
{
    public class HTMLPaser
    {
        private StringReader sr;

        //private string curTag = "";


        public HTMLPaser(StringReader content)
        {
            this.sr = content;
        }

        public void FindTagById(string tag, string id)
        {
            string aTag = "";

            aTag = getNextTag();
            while (true)
            {
                if (aTag == " ")
                {
                    break;
                }
                else if (aTag == tag)
                {
                    aTag = aTag;
                }
                aTag = getNextTag();
            }
            
        }

        private string getNextTag()
        {
            string curTag = "";
            int i;
            char c;
            bool inTag = false;

            while (true)
            {
                i = sr.Read();
                if (i == -1) { break; }

                c = (char)i;

                if (c == '<')
                {
                    curTag = "";
                    inTag = true;
                }
                else if (c == '>' || c == ' ')
                {
                    if (inTag)
                    {
                        return curTag;
                    }
                }
                else {
                    if (inTag)
                    {
                        curTag += c;
                    }
                    
                }
            }
            return curTag;
        }
    }

    public class HTMLElement
    {
        public string tag;

    }
}
