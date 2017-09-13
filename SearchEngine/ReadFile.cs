using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Threading;
using CSharpStringSort;

namespace Engine
{
    class ReadFile
    {
        /// <summary>
        /// Allfiles will store all the file names we will parse and index
        /// </summary>
        private string[] ALLFILES;
        public string[] AllFiles
        {
            get { return ALLFILES; }
            set { ALLFILES = value; }
        }
        
        /// <summary>
        /// an instant of indexer 
        /// </summary>
        private Indexer INDEXER;
        public Indexer indexer
        {
            get { return INDEXER; }
            set { INDEXER = value; }
        }
        

        /// <summary>
        /// docs will store all the information about the document we handle
        /// </summary>
        private Dictionary<string, Documet> DOCS;
        public Dictionary<string, Documet> docs
        {
            get { return DOCS; }
            set { DOCS = value; }
        }

        /// <summary>
        /// languages will store all the languages that appear in the language tag in the documents
        /// </summary>
        private List<string> languages;
        public List<string> Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        /// <summary>
        /// holds the path where the posting files will be saved
        /// </summary>
        private string SAVEPATH;
        public string savepath
        {
            get { return SAVEPATH; }
            set { SAVEPATH = value; }
        }

        /// <summary>
        /// holds the path from where we will load the corpus.
        /// </summary>
        private string LOADPATH;
        public string loadpath
        {
            get { return LOADPATH; }
            set { LOADPATH = value; }
        }

        /// <summary>
        /// true if we need to stem and false otherwise.
        /// </summary>
        bool Stem;

        /// <summary>
        /// holds the number of all the documnets in the corpus.
        /// </summary>
        private int NUMOFDOCS;
        public int numofdocs
        {
            get { return NUMOFDOCS; }
            set { NUMOFDOCS = value; }
        }
        
        /// <summary>
        /// ReadFile- will recive a path where all the files and stop word txt file are located and constract
        /// all the needed data storge
        /// </summary>
        /// <param name="LoadPath">the path from where we load the file</param>
        /// <param name="SavePath">where we need to save the file</param>
        /// <param name="isstem">truf if we need to stem, false otherwise</param>
        public ReadFile(string LoadPath,string SavePath, bool isstem)
        {
            savepath = SavePath;
            loadpath = LoadPath;
            Stem = isstem;
            AllFiles = Directory.EnumerateFiles(LoadPath).ToArray();
            indexer = new Engine.Indexer();
            Languages = new List<string>();
            numofdocs = 0;

        }

        /// <summary>
        /// divides all the files to iteration of 10 files each time, and sends them for parsing.
        /// After all the iterations finishe we send to the sort dictionary method, merge posting and 
        /// turn the dictionary and languages to file.
        /// </summary>
        public void readFiles()
        {
            for (int i = 0; i < 48; i++)
            {
                if (i < 47)
                    Reading(i * 10, i * 10 + 10, i);
                else
                    Reading(i * 10, AllFiles.Length, i);
            }
            indexer.SortDictionary();
            indexer.Merge(savepath);
            DictAndLangDoc();
        }

        /// <summary>
        /// will split a specifit amout (end-start) of files into documents by the indicator <doc></doc> 
        /// will retrive the text portion of the document by the indicator <text></text>
        /// each doc will be entered to the Documents dictionary
        /// </summary>
        /// <param name="start">the index of the first file to parse</param>
        /// <param name="end">the index of the last file to parse</param>
        /// <param name="h">the number of itaration</param>
        private void Reading(int start, int end, int h)
        {
            Dictionary<string, string> Documents = new Dictionary<string, string>();
            for (int i = start; i < end; i++)
            {
                if (!AllFiles[i].Contains("stop_words.txt"))
                {
                    string doc = File.ReadAllText(AllFiles[i], Encoding.GetEncoding("iso-8859-1"));
                    doc = Regex.Replace(doc, @"[^\S\r\n]+", " ");
                    string[] separator = new string[1] { "</DOC>" };
                    string[] alldocs = doc.Split(separator, StringSplitOptions.None);
                    for (int j = 0; j < alldocs.Length - 1; j++)
                    {
                        int start1 = alldocs[j].IndexOf(@"<DOCNO>");
                        int end1 = alldocs[j].IndexOf(@"</DOCNO>");
                        string docid = alldocs[j].Substring(start1 + 8, end1 - start1 - 9);
                        start1 = alldocs[j].IndexOf("<TEXT>");
                        end1 = alldocs[j].IndexOf("</TEXT>");
                        string text = alldocs[j].Substring(start1 + 7, end1 - start1 - 8);
                        Documents.Add(docid, text);
                    }
                }
            }
            Parse parse = new Engine.Parse(Documents);
            indexer.Indexing(parse.Parsing(loadpath,Stem));
            indexer.CreatePosting(savepath, h);
            docs = parse.fordocsdoc;
            numofdocs += docs.Count;
            CreateDocuments(h);
            docs.Clear();
        }

        /// <summary>
        /// Getting through the dictionary and writes each entrey to the file of dictionary.
        /// Getting through the languages list and write each language to document of languages.
        /// </summary>
        private void DictAndLangDoc()
        {
            StreamWriter sw = new StreamWriter(savepath + "\\dictionary.txt", true);
            StreamWriter sw1 = new StreamWriter(savepath + "\\languages.txt", true);
            foreach (string s in indexer.Dictionary.Keys)
                sw.WriteLine(s + " " + indexer.Dictionary[s][0] + " " + indexer.Dictionary[s][1] + " " + indexer.Dictionary[s][2]);
            foreach (string s in languages)
                sw1.WriteLine(s);
            sw.Close(); sw1.Close();
        }

        /// <summary>
        /// will save all document information to a txt file
        /// will create a list of languages that the documents support
        /// </summary>
        /// <param name="h"></param>
        private void CreateDocuments(int h)
        {
            StreamWriter sw = new StreamWriter(savepath+ "\\documents.txt", true);
            foreach (string s in docs.Keys)
            {
                if (!Languages.Contains(docs[s].language.ToLower()) && docs[s].language.ToLower() != "nolanguage")
                    Languages.Add(docs[s].language.ToLower());
                sw.WriteLine(docs[s].tostring());
            }
            sw.Close();
        }
        
    }
}
