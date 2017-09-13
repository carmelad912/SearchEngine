using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    /// <summary>
    /// Data structure for document.
    /// </summary>
    class Documet
    {
        /// <summary>
        /// The property that holds the docID.
        /// </summary>
        private string DOCID;
        public string docid
        {
            get { return DOCID; }
            set { DOCID = value; }
        }

        /// <summary>
        /// The property that holds the number of appearances of the most frequent term. 
        /// </summary>
        private int MAX_TF;
        public int max_tf
        {
            get { return MAX_TF; }
            set { MAX_TF = value; }
        }

        /// <summary>
        /// The property that holds the most frequent term.
        /// </summary>
        private string FREQ_TERM;
        public string freq_term
        {
            get { return FREQ_TERM; }
            set { FREQ_TERM = value; }
        }

        /// <summary>
        /// The property that holds the number of the distinctive words.
        /// </summary>
        private int DIST_WORDS;
        public int dist_words
        {
            get { return DIST_WORDS; }
            set { DIST_WORDS = value; }
        }

        /// <summary>
        /// The property that holds the language of the document.
        /// </summary>
        private string LANGUAGE;    
        public string language
        {
            get { return LANGUAGE; }
            set { LANGUAGE = value; }
        }

        /// <summary>
        /// The property that holds the type of the document.
        /// </summary>
        private string TYPE;
        public string type
        {
            get { return TYPE; }
            set { TYPE = value; }
        }

        /// <summary>
        /// Constructor that builds a default type of document.
        /// </summary>
        public Documet()
        {
            max_tf = 0;
            freq_term = "";
            dist_words = 0;
            language = "NOLANGUAGE";
            docid = "";
            type = "NOTYPE";
        }

        /// <summary>
        /// Returns the document as a string.
        /// </summary>
        /// <returns></returns>
        public string tostring()
        {
            return docid + " " + dist_words + " " + freq_term + " " + max_tf + " " + language + " " + type;
        }

        /// <summary>
        /// gets string and returns a document by splitting the string and getting each property of the document.
        /// </summary>
        /// <param name="s">string to turn into a document</param>
        /// <returns></returns>
        public Documet FromStringToDoc(string s)
        {
            string[] document = s.Split(' ');
            Documet doc = new Documet();
            doc.docid = document[0];
            doc.dist_words = Int32.Parse(document[1]);
            doc.freq_term = document[2];
            doc.max_tf = Int32.Parse(document[3]);
            doc.language = document[4];
            doc.type = document[5];
            return doc;
        }

    }   
}
