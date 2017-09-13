using CSharpStringSort;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class Indexer
    {
        /// <summary>
        /// counts the number of unique terms
        /// </summary>
        private int UNIQUE;
        public int unique
        {
            get { return UNIQUE; }
            set { UNIQUE = value; }
        }
        
        /// <summary>
        /// data structure for dictionary. key-term, value-[num of docs in which the term appears,
        /// num of appearances in corpus, corresponding  line in posting file]
        /// </summary>
        private Dictionary<string, int[]> Dict;
        public Dictionary<string, int[]> Dictionary
        {
            get { return Dict; }
            set { Dict = value; }
        }

        /// <summary>
        /// data structure for posting. key-term, value- list of tuples <document id, num of appearances in said socument>
        /// </summary>
        private Dictionary<string, List<Tuple<String, int>>> Post;
        public Dictionary<string, List<Tuple<String, int>>> post
        {
            get { return Post; }
            set { Post = value; }
        }

        /// <summary>
        /// indexr object
        /// </summary>
        public Indexer()
        {
            Dictionary = new Dictionary<string, int[]>();
            post = new Dictionary<string, List<Tuple<String, int>>>();
            unique = 0;
        }
       
        /// <summary>
        /// adds new terms to dictionary, updates the values of existing terms in dictionary
        /// </summary>
        /// <param name="fordict"></param>
        public void Indexing(Dictionary<string, List<Tuple<string, int>>> fordict)
        {
            Dictionary<string, int[]> temp = new Dictionary<string, int[]>();
            foreach (string s in fordict.Keys)
            {
                temp[s] = new int[3];
                temp[s][0] = fordict[s].Count();
                temp[s][1] = 0;
                temp[s][2] = 0;
                foreach (Tuple<string, int> t in fordict[s])
                    temp[s][1] += t.Item2;
                if (Dictionary.ContainsKey(s))
                {
                    Dictionary[s][0] += temp[s][0];
                    Dictionary[s][1] += temp[s][1];
                }
                else
                    Dictionary[s] = temp[s];
                if (post.ContainsKey(s))
                {
                    foreach (Tuple<string, int> t in fordict[s])
                        post[s].Add(t);
                }
                else
                    post[s] = fordict[s];
            }
        }
        
        /// <summary>
        /// sorts the terms in alphabetical order
        /// opens, writes and closes tamporary posting files, then clears the posting dictionary
        /// </summary>
        /// <param name="path"></param>
        /// <param name="i"></param>
        public void CreatePosting(string path, int i)
        {
            Dictionary<string, List<Tuple<string, int>>> temp2 = new Dictionary<string, List<Tuple<string, int>>>();
            string[] tosort = post.Keys.ToArray<string>();
            string[] sorted = Sedgewick.Sort(tosort);
            for (int j = 0; j < sorted.Length; j++)
            {
                string s = sorted[j];
                temp2[s] = post[s];
            }
            System.IO.Directory.CreateDirectory(path);
            StreamWriter sw = new StreamWriter(path + i + "others.txt", true);
            StreamWriter swa = new StreamWriter(path + i + "a.txt", true);
            StreamWriter swb = new StreamWriter(path + i + "b.txt", true);
            StreamWriter swc = new StreamWriter(path + i + "c.txt", true);
            StreamWriter swd = new StreamWriter(path + i + "d.txt", true);
            StreamWriter swe = new StreamWriter(path + i + "e.txt", true);
            StreamWriter swf = new StreamWriter(path + i + "f.txt", true);
            StreamWriter swg = new StreamWriter(path + i + "g.txt", true);
            StreamWriter swh = new StreamWriter(path + i + "h.txt", true);
            StreamWriter swi = new StreamWriter(path + i + "i.txt", true);
            StreamWriter swj = new StreamWriter(path + i + "j.txt", true);
            StreamWriter swk = new StreamWriter(path + i + "k.txt", true);
            StreamWriter swl = new StreamWriter(path + i + "l.txt", true);
            StreamWriter swm = new StreamWriter(path + i + "m.txt", true);
            StreamWriter swn = new StreamWriter(path + i + "n.txt", true);
            StreamWriter swo = new StreamWriter(path + i + "o.txt", true);
            StreamWriter swp = new StreamWriter(path + i + "p.txt", true);
            StreamWriter swq = new StreamWriter(path + i + "q.txt", true);
            StreamWriter swr = new StreamWriter(path + i + "r.txt", true);
            StreamWriter sws = new StreamWriter(path + i + "s.txt", true);
            StreamWriter swt = new StreamWriter(path + i + "t.txt", true);
            StreamWriter swu = new StreamWriter(path + i + "u.txt", true);
            StreamWriter swv = new StreamWriter(path + i + "v.txt", true);
            StreamWriter sww = new StreamWriter(path + i + "w.txt", true);
            StreamWriter swx = new StreamWriter(path + i + "x.txt", true);
            StreamWriter swy = new StreamWriter(path + i + "y.txt", true);
            StreamWriter swz = new StreamWriter(path + i + "z.txt", true);
            foreach (string s in temp2.Keys)
            {
                if (s[0] == 'a')
                    foreach (Tuple<string, int> t in temp2[s])
                        swa.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'b')
                    foreach (Tuple<string, int> t in temp2[s])
                        swb.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'c')
                    foreach (Tuple<string, int> t in temp2[s])
                        swc.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'd')
                    foreach (Tuple<string, int> t in temp2[s])
                        swd.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'e')
                    foreach (Tuple<string, int> t in temp2[s])
                        swe.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'f')
                    foreach (Tuple<string, int> t in temp2[s])
                        swf.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'g')
                    foreach (Tuple<string, int> t in temp2[s])
                        swg.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'h')
                    foreach (Tuple<string, int> t in temp2[s])
                        swh.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'i')
                    foreach (Tuple<string, int> t in temp2[s])
                        swi.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'j')
                    foreach (Tuple<string, int> t in temp2[s])
                        swj.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'k')
                    foreach (Tuple<string, int> t in temp2[s])
                        swk.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'l')
                    foreach (Tuple<string, int> t in temp2[s])
                        swl.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'm')
                    foreach (Tuple<string, int> t in temp2[s])
                        swm.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'n')
                    foreach (Tuple<string, int> t in temp2[s])
                        swn.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'o')
                    foreach (Tuple<string, int> t in temp2[s])
                        swo.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'p')
                    foreach (Tuple<string, int> t in temp2[s])
                        swp.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'q')
                    foreach (Tuple<string, int> t in temp2[s])
                        swq.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'r')
                    foreach (Tuple<string, int> t in temp2[s])
                        swr.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 's')
                    foreach (Tuple<string, int> t in temp2[s])
                        sws.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 't')
                    foreach (Tuple<string, int> t in temp2[s])
                        swt.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'u')
                    foreach (Tuple<string, int> t in temp2[s])
                        swu.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'v')
                    foreach (Tuple<string, int> t in temp2[s])
                        swv.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'w')
                    foreach (Tuple<string, int> t in temp2[s])
                        sww.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'x')
                    foreach (Tuple<string, int> t in temp2[s])
                        swx.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'y')
                    foreach (Tuple<string, int> t in temp2[s])
                        swy.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else if (s[0] == 'z')
                    foreach (Tuple<string, int> t in temp2[s])
                        swz.WriteLine(s + " " + t.Item1 + " " + t.Item2);
                else
                    foreach (Tuple<string, int> t in temp2[s])
                        sw.WriteLine(s + " " + t.Item1 + " " + t.Item2);
            }
            swa.WriteLine("---"); swb.WriteLine("---"); swc.WriteLine("---"); swd.WriteLine("---"); swe.WriteLine("---");
            swf.WriteLine("---"); swg.WriteLine("---"); swh.WriteLine("---"); swi.WriteLine("---"); swj.WriteLine("---");
            swk.WriteLine("---"); swl.WriteLine("---"); swm.WriteLine("---"); swn.WriteLine("---"); swo.WriteLine("---");
            swp.WriteLine("---"); swq.WriteLine("---"); swr.WriteLine("---"); sws.WriteLine("---"); swt.WriteLine("---");
            swu.WriteLine("---"); swv.WriteLine("---"); sww.WriteLine("---"); swx.WriteLine("---"); swy.WriteLine("---");
            swz.WriteLine("---"); sw.WriteLine("---");

            sw.Close(); swa.Close(); swb.Close(); swc.Close(); swd.Close(); swe.Close();
            swf.Close(); swg.Close(); swh.Close(); swi.Close(); swj.Close(); swk.Close();
            swl.Close(); swm.Close(); swn.Close(); swo.Close(); swp.Close(); swq.Close();
            swr.Close(); sws.Close(); swt.Close(); swu.Close(); swv.Close(); sww.Close();
            swx.Close(); swy.Close(); swz.Close();

            post.Clear();
        }

        /// <summary>
        /// sorts the dictionary keys(the terms) in alphabetical order
        /// </summary>
        internal void SortDictionary()
        {
            Dictionary<string, int[]> temp = new Dictionary<string, int[]>();
            string[] tosort = Dictionary.Keys.ToArray<string>();
            string[] sorted = Sedgewick.Sort(tosort);
            for (int i = 0; i < sorted.Length; i++)
            {
                string s = sorted[i];
                    temp[s] = new int[3];
                temp[s][0] = Dictionary[s][0];
                temp[s][1] = Dictionary[s][1];
                temp[s][2] = Dictionary[s][2];
                if (temp[s][1] == 1)
                    unique++;
            }
            Dictionary = temp;
        }

        /// <summary>
        ///unify all temporary posting files in to 1 alphabeticly sorted posting file (for each letter and "others")
        /// </summary>
        /// <param name="path"></param>
        public void Merge(string path)
        {
            int i = 49;
            int count=0;
            string currentterm="";
            List<string> allpost = Directory.EnumerateFiles(path).ToList();
            List<string>[] alldocs = new List<string>[27];
            for (int num = 0; num < alldocs.Length; num++)
                alldocs[num] = new List<string>();
            foreach (string s in allpost)
            {
                if (!s.Contains("documents.txt") && !s.Contains("dictionary.txt") && !s.Contains("languages.txt"))
                {
                    if (s.Contains("a.txt"))
                        alldocs[0].Add(s);
                    else if (s.Contains("b.txt"))
                        alldocs[1].Add(s);
                    else if (s.Contains("c.txt"))
                        alldocs[2].Add(s);
                    else if (s.Contains("d.txt"))
                        alldocs[3].Add(s);
                    else if (s.Contains("e.txt"))
                        alldocs[4].Add(s);
                    else if (s.Contains("f.txt"))
                        alldocs[5].Add(s);
                    else if (s.Contains("g.txt"))
                        alldocs[6].Add(s);
                    else if (s.Contains("h.txt"))
                        alldocs[7].Add(s);
                    else if (s.Contains("i.txt"))
                        alldocs[8].Add(s);
                    else if (s.Contains("j.txt"))
                        alldocs[9].Add(s);
                    else if (s.Contains("k.txt"))
                        alldocs[10].Add(s);
                    else if (s.Contains("l.txt"))
                        alldocs[11].Add(s);
                    else if (s.Contains("m.txt"))
                        alldocs[12].Add(s);
                    else if (s.Contains("n.txt"))
                        alldocs[13].Add(s);
                    else if (s.Contains("o.txt"))
                        alldocs[14].Add(s);
                    else if (s.Contains("p.txt"))
                        alldocs[15].Add(s);
                    else if (s.Contains("q.txt"))
                        alldocs[16].Add(s);
                    else if (s.Contains("others.txt"))
                        alldocs[26].Add(s);
                    else if (s.Contains("r.txt"))
                        alldocs[17].Add(s);
                    else if (s.Contains("s.txt"))
                        alldocs[18].Add(s);
                    else if (s.Contains("t.txt"))
                        alldocs[19].Add(s);
                    else if (s.Contains("u.txt"))
                        alldocs[20].Add(s);
                    else if (s.Contains("v.txt"))
                        alldocs[21].Add(s);
                    else if (s.Contains("w.txt"))
                        alldocs[22].Add(s);
                    else if (s.Contains("x.txt"))
                        alldocs[23].Add(s);
                    else if (s.Contains("y.txt"))
                        alldocs[24].Add(s);
                    else if (s.Contains("z.txt"))
                        alldocs[25].Add(s);
                }
            }
            char c = 'a';
            StreamWriter sw1;
            for (int k = 0; k < alldocs.Length; k++)
            {
                count = 0;
                while (alldocs[k].Count > 1)
                {
                    if (alldocs[k].Count == 3)
                    {
                        if (k == alldocs.Length - 1)
                        {
                            sw1 = new StreamWriter(path + "\\" + i + "others.txt", true);
                            alldocs[k].Add(path + "\\" + i + "others.txt");
                        }
                        else
                        {
                            sw1 = new StreamWriter(path + "\\" + i + c + ".txt", true);
                            alldocs[k].Add(path + "\\" + i + c + ".txt");

                        }
                        sw1.WriteLine("---");
                        sw1.Close();
                    }
                    StreamReader sr0 = new StreamReader(alldocs[k].ElementAt(0)), sr1 = new StreamReader(alldocs[k].ElementAt(1));
                    StreamReader sr2 = new StreamReader(alldocs[k].ElementAt(2)), sr3 = new StreamReader(alldocs[k].ElementAt(3));
                    string s0 = sr0.ReadLine() + " s0", s1 = sr1.ReadLine() + " s1", s2 = sr2.ReadLine() + " s2", s3 = sr3.ReadLine() + " s3";
                    StreamWriter sw;
                    if (alldocs[k].Count == 4)
                    {
                        if (k == alldocs.Length - 1)
                            sw = new StreamWriter(path + "\\" + "others.txt", true);
                        else
                            sw = new StreamWriter(path + "\\" + c + ".txt", true);
                    }
                    else if (k == alldocs.Length - 1)
                    {
                        sw = new StreamWriter(path + "\\" + i + "others.txt", true);
                        alldocs[k].Add(path + "\\" + i + "others.txt");
                    }
                    else
                    {
                        sw = new StreamWriter(path + "\\" + i + c + ".txt", true);
                        alldocs[k].Add(path + "\\" + i + c + ".txt");
                    }
                    while (!sr0.EndOfStream || !sr1.EndOfStream || !sr2.EndOfStream || !sr3.EndOfStream)
                    {
                        string[] notsorted = new string[] { s0, s1, s2, s3 };
                        string[] array = Sedgewick.Sort(notsorted);

                        if (array[0] != "eof" && !array[0].Contains("--- s"))
                        {
                            if (array[0][array[0].Length - 1] == '0')
                            {
                                if (!sr0.EndOfStream)
                                    s0 = sr0.ReadLine() + " s0";
                            }
                            else if (array[0][array[0].Length - 1] == '1')
                            {
                                if (!sr1.EndOfStream)
                                    s1 = sr1.ReadLine() + " s1";
                            }
                            else if (array[0][array[0].Length - 1] == '2')
                            {
                                if (!sr2.EndOfStream)
                                    s2 = sr2.ReadLine() + " s2";
                            }
                            else if (array[0][array[0].Length - 1] == '3')
                            {
                                if (!sr3.EndOfStream)
                                    s3 = sr3.ReadLine() + " s3";
                            }
                            string x = array[0];
                            if (alldocs[k].Count == 4)
                            {
                                int index = array[0].IndexOf(" ");
                                int place= array[0].IndexOf(" ");
                                x = array[0].Substring(index + 1);
                                for (int ind = 0; ind < 2; ind++)
                                {
                                    if (x[0] != 'F')
                                    {
                                        index = x.IndexOf(" ");
                                        x = x.Substring(index + 1);
                                        place += index+1;
                                    }
                                }
                                if (currentterm != array[0].Substring(0, place))
                                {
                                    currentterm = array[0].Substring(0, place);
                                    Dictionary[currentterm][2] = count;
                                }
                                count++;

                            }
                            sw.WriteLine(x.Substring(0, x.Length - 3));
                        }
                        else if (array[1] != "eof" && !array[1].Contains("--- s"))
                        {
                            if (array[1][array[1].Length - 1] == '0')
                            {
                                if (!sr0.EndOfStream)
                                    s0 = sr0.ReadLine() + " s0";
                            }

                            else if (array[1][array[1].Length - 1] == '1')
                            {
                                if (!sr1.EndOfStream)
                                    s1 = sr1.ReadLine() + " s1";
                            }
                            else if (array[1][array[1].Length - 1] == '2')
                            {
                                if (!sr2.EndOfStream)
                                    s2 = sr2.ReadLine() + " s2";
                            }
                            else if (array[1][array[1].Length - 1] == '3')
                            {
                                if (!sr3.EndOfStream)
                                    s3 = sr3.ReadLine() + " s3";
                            }
                            string x = array[1];
                            if (alldocs[k].Count == 4)
                            {
                                int index = array[1].IndexOf(" ");
                                x = array[1].Substring(index + 1);
                                int place = array[1].IndexOf(" ");
                                for (int ind = 0; ind < 2; ind++)
                                    if (x[0] != 'F')
                                    {
                                        index = x.IndexOf(" ");
                                        x = x.Substring(index + 1);
                                        place += index + 1;
                                    }
                                if (currentterm != array[1].Substring(0, place))
                                {
                                    currentterm = array[1].Substring(0, place);
                                    Dictionary[currentterm][2] = count;
                                }
                                count++;
                            }
                            
                            sw.WriteLine(x.Substring(0, x.Length - 3));
                        }
                        else if (array[2] != "eof" && !array[2].Contains("--- s"))
                        {
                            if (array[2][array[2].Length - 1] == '0')
                            {
                                if (!sr0.EndOfStream)
                                    s0 = sr0.ReadLine() + " s0";
                            }
                            else if (array[2][array[2].Length - 1] == '1')
                            {
                                if (!sr1.EndOfStream)
                                    s1 = sr1.ReadLine() + " s1";
                            }
                            else if (array[2][array[2].Length - 1] == '2')
                            {
                                if (!sr2.EndOfStream)
                                    s2 = sr2.ReadLine() + " s2";
                            }
                            else if (array[2][array[2].Length - 1] == '3')
                            {
                                if (!sr3.EndOfStream)
                                    s3 = sr3.ReadLine() + " s3";
                            }
                            string x = array[2];
                            if (alldocs[k].Count == 4)
                            {
                                int index = array[2].IndexOf(" ");
                                x = array[2].Substring(index + 1);
                                int place = array[2].IndexOf(" ");
                                for (int ind = 0; ind < 2; ind++)
                                    if (x[0] != 'F')
                                    {
                                        index = x.IndexOf(" ");
                                        x = x.Substring(index + 1);
                                        place += index + 1;
                                    }
                                if (currentterm != array[2].Substring(0, place))
                                {
                                    currentterm = array[2].Substring(0, place);
                                    Dictionary[currentterm][2] = count;
                                }
                                count++;
                            }
                            sw.WriteLine(x.Substring(0, x.Length - 3));
                        }
                        else
                        {
                            if (array[3][array[3].Length - 1] == '0')
                            {
                                if (!sr0.EndOfStream)
                                    s0 = sr0.ReadLine() + " s0";
                            }
                            else if (array[3][array[3].Length - 1] == '1')
                            {
                                if (!sr1.EndOfStream)
                                    s1 = sr1.ReadLine() + " s1";
                            }
                            else if (array[3][array[3].Length - 1] == '2')
                            {
                                if (!sr2.EndOfStream)
                                    s2 = sr2.ReadLine() + " s2";
                            }
                            else if (array[3][array[3].Length - 1] == '3')
                            {
                                if (!sr3.EndOfStream)
                                    s3 = sr3.ReadLine() + " s3";
                            }
                            string x = array[3];
                            if (alldocs[k].Count == 4)
                            {
                                int index = array[3].IndexOf(" ");
                                x = array[3].Substring(index + 1);
                                int place = array[3].IndexOf(" ");
                                for (int ind = 0; ind < 2; ind++)
                                    if (x[0] != 'F')
                                    {
                                        index = x.IndexOf(" ");
                                        x = x.Substring(index + 1);
                                        place += index + 1;
                                    }
                                if (currentterm != array[3].Substring(0, place))
                                {
                                    currentterm = array[3].Substring(0, place);
                                    Dictionary[currentterm][2] = count;
                                }
                                count++;
                            }
                            sw.WriteLine(x.Substring(0, x.Length - 3));
                        }
                        if (sr0.EndOfStream)
                            s0 = "eof";
                        if (sr1.EndOfStream)
                            s1 = "eof";
                        if (sr2.EndOfStream)
                            s2 = "eof";
                        if (sr3.EndOfStream)
                            s3 = "eof";
                    }
                    sr0.Close(); sr1.Close(); sr2.Close(); sr3.Close();
                    sw.WriteLine("---");
                    sw.Close();
                    File.Delete(alldocs[k].ElementAt(0)); alldocs[k].RemoveAt(0);
                    File.Delete(alldocs[k].ElementAt(0)); alldocs[k].RemoveAt(0);
                    File.Delete(alldocs[k].ElementAt(0)); alldocs[k].RemoveAt(0);
                    File.Delete(alldocs[k].ElementAt(0)); alldocs[k].RemoveAt(0);
                    i++;
                }
                c++;
            }
        }

    }
}

