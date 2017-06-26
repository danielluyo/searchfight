using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace searchfight
{
    /// <summary>
    /// Minimal HTML Parser, complete enough for the contest needs.
    /// </summary>
    public class HTMLPaser
    {
        private StringReader sr;
        private int i;              // Current read value from stream
        private char curChar;       // Current read character

        private static string blanksChars = "" + (char)32 + (char)10 + (char)13 + (char)9;

        public HTMLPaser(StringReader content)
        {
            this.sr = content;
        }
        
        /// <summary>
        /// Finds an Element by its tag and a attribute value.
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="attrName"></param>
        /// <param name="idValue"></param>
        /// <returns></returns>
        public HTMLElement FindElementById(string tagName, string attrName, string idValue)
        {
            HTMLElement ele;
            string[] attrs;

            ele = getNextElement();
            while (ele != null )
            {
                if (ele.tag == tagName)
                {
                    attrs = ele.attrList.GetValues(attrName);
                    if (attrs != null && attrs.Count() > 0 && attrs[0] == idValue)
                    {
                        return ele;
                    }
                }
                ele = getNextElement();
            }
            return ele;
        }

        /// <summary>
        /// Parse the next Element
        /// </summary>
        /// <returns>The found element</returns>
        private HTMLElement getNextElement()
        {
            HTMLElement ele = null;
            string curTag = "";
            bool inTag = false;

            ReadChar();
            SkipBlanks();
            while (true)
            {                
                if (i == -1) { break; }

                if (curChar == '<')
                {
                    curTag = "";
                    inTag = true;
                }
                else if (curChar == '/')
                {
                    // Its a closing tag, jump over
                    if (inTag && (curTag == "<"))
                    {
                        curTag = "";
                        inTag = false;
                    }
                }
                else if (inTag && (curChar == '>' || curChar == ' ') )
                {
                    ele = new HTMLElement();
                    ele.tag = curTag;
                    if (curChar == ' ')
                    {
                        ReadAttributes(ele);
                    }

                    SkipBlanks();
                    while (("<"+(char)0).IndexOf(PeekChar()) < 0 )
                    {
                        ReadChar();
                        ele.value += curChar;
                    }

                    return ele;
                }
                else
                {
                    if (inTag)
                    {
                        curTag += curChar;
                    }
                }
                ReadChar();
            }
            return ele;
        }

        /// <summary>
        /// Load all attributes from an XML Element to a list.
        /// </summary>
        /// <param name="ele"></param>
        private void ReadAttributes(HTMLElement ele)
        {
            string attrName = "";
            string attrValue = "";
            bool inAttrName = true;
            bool inAttrValue = !inAttrName;

            SkipBlanks();
            while (i != -1 || curChar != '>')
            {
                if (curChar == '>' && attrName != "")
                {
                    ele.attrList.Add(attrName.Trim(), attrValue.Trim().Trim('"'));
                    return;
                }
                else if (curChar == '>')
                {
                    return;
                }
                //else if ((curChar == '=' || curChar == ' ') && inAttrName)
                else if ((curChar == '=') && inAttrName)
                {
                    SkipBlanks();
                    inAttrValue = true;
                    inAttrName = !inAttrValue;

                    // Quoted value
                    if (PeekChar() == '"')
                    {
                        ReadChar();
                        attrValue += curChar;
                        do
                        {
                            ReadChar();
                            attrValue += curChar;
                        } while (curChar != '"');
                        ele.attrList.Add(attrName.Trim(), attrValue.Trim().Trim('"'));
                        attrName = "";
                        attrValue = "";
                        inAttrName = true;
                        inAttrValue = !inAttrName;                        
                    }
                }
                else if (i == 92) // "\"
                {
                    //ReadChar();
                    SkipBlanks();
                }
                else if (inAttrName)
                {
                    attrName += curChar;
                }
                else if (inAttrValue)
                {
                    attrValue += curChar;
                }
                ReadChar();
            }
        }
    
        /// <summary>
        /// Skip blanks characteres
        ///     32 blank space
        ///     13 CR
        ///     10 LF
        ///     9  TAB
        /// </summary>
        private void SkipBlanks()
        {
            if (blanksChars.IndexOf(PeekChar()) >= 0) {
                ReadChar();
            }

            while (blanksChars.IndexOf(curChar) >= 0 )
            {
                ReadChar();                
            }

        }

        /// <summary>
        /// Read next Char from Stream. Check if we reached End Of Stream.
        /// </summary>
        private void ReadChar() {
            i = sr.Read();
            if (i == -1)
            {
                curChar = (char)0;
            }
            else
            {
                curChar = (char)i;
            }
        }

        private char PeekChar() {
            int x = sr.Peek();
            if (x == -1)
            {
                return (char)0;
            }
            else
            {
                return (char)x;
            }
        }
    }

    /// <summary>
    /// HTMLElement class encapsulate tag, and attributes list from an HTML Element.
    /// In a complete HTML Parser the Element must have a pointer to its children list a pointer to its 
    /// next sibling.
    /// </summary>
    public class HTMLElement
    {
        public string tag { get; set; }
        public NameValueCollection attrList = new NameValueCollection();
        public string value { get; set; }


        public HTMLElement()
        {
            this.value = "";
        }

        public HTMLElement(string tag)
        {
            this.tag = tag;
            this.value = "";
        }
    }
}
