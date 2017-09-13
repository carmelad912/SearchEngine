using SearchEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    class Parse
    {
        /// <summary>
        /// indicates where we are in the document 
        /// (in order to go over each word, and skip some words if needed by parsing rules)
        /// </summary>
        int i;

        /// <summary>
        /// holds the documents which we will parse
        /// </summary>
        Dictionary<string, string> documents;

        /// <summary>
        /// holds the stopwords
        /// </summary>
        HashSet<string> StopWords;

        /// <summary>
        /// will be used by the indexer. key- term, value- list of tuple<document id, num of appearences in document)
        /// </summary>
        Dictionary<string, List<Tuple<string, int>>> toindexer;

        /// <summary>
        /// will be used in order to create a txt file containing all information about the documents
        /// </summary>
        private Dictionary<string, Documet> FORDOCSDOC;
        public Dictionary<string, Documet> fordocsdoc
        {
            get { return FORDOCSDOC; }
            set { FORDOCSDOC = value; }
        }

        /// <summary>
        /// constructor of the class
        /// </summary>
        /// <param name="docs">dictionary of docs to parse. the key is the docid.</param>
        public Parse(Dictionary<string, string> docs)
        {
            documents = docs;
            StopWords = new HashSet<string>();
            toindexer = new Dictionary<string, List<Tuple<string, int>>>();
            fordocsdoc = new Dictionary<string, Engine.Documet>();
            i = 0;
        }
        
        /// <summary>
        /// save "language" and "type" using tags
        /// go over every term in document and check every parsing rule, then insert in any relevant data structure
        /// </summary>
        /// <param name="path">path from where we get the stop words.</param>
        /// <param name="isstem">bool if we need to do stemming</param>
        /// <returns></returns>
        public Dictionary<string, List<Tuple<string, int>>> Parsing(string path, bool isstem)
        {
            AddStopWords(path+"\\stop_words.txt");
            List<string> docids = documents.Keys.ToList();
            int count = 0;
            foreach (string s in documents.Values)
            {
                string alldoc = s;
                fordocsdoc[docids.ElementAt(count)] = new Engine.Documet();
                fordocsdoc[docids.ElementAt(count)].docid = docids.ElementAt(count);
                string currentdoc = docids.ElementAt(count);
                int indexl=0, indexle=0, indext=0, indexte=0;
                if(alldoc.Contains("<F P=105>"))
                {
                    indexl = alldoc.IndexOf("<F P=105>");
                    indexle = alldoc.IndexOf("</F>", indexl);
                    fordocsdoc[docids.ElementAt(count)].language = StripPunctuation(alldoc.Substring(indexl + 8, indexle - indexl -8));
                    alldoc = alldoc.Remove(indexl, indexle - indexl + 3); 
                    if (fordocsdoc[docids.ElementAt(count)].language.Contains(" "))
                        fordocsdoc[docids.ElementAt(count)].language=fordocsdoc[docids.ElementAt(count)].language.Remove(alldoc.IndexOf(" "));
                }
                if (alldoc.Contains("Article Type:"))
                {
                    indext = alldoc.IndexOf("Article Type:");
                    indexte = alldoc.IndexOf("\n",indext);
                    fordocsdoc[docids.ElementAt(count)].type = StripPunctuation(alldoc.Substring(indext + 12, indexte - indext - 12));
                    alldoc= alldoc.Remove(indext, indexte - indext + 1);
                }
                string[] terms = alldoc.Split(' ');
                while (i < terms.Length)
                {
                    string term = StripPunctuation(terms[i].ToLower());
                    if (!StopWords.Contains(term) && term != "")
                    {
                        if (!term.Contains('-'))
                        {
                            string date;
                            if (i == terms.Length - 1)
                                date = WhichDate(term, "", "");
                            else if (i == terms.Length - 2)
                                date = WhichDate(term, StripPunctuation(terms[i + 1]).ToLower(), "");
                            else
                                date = WhichDate(term, StripPunctuation(terms[i + 1]).ToLower(), StripPunctuation(terms[i + 2]).ToLower());
                            if (date != "notdate")
                            {
                                if (date.Length == 11)
                                    this.i = i + 2;
                                else
                                    this.i = i + 1;
                                term = date;
                            }
                            else if (IsNumber(term))
                            {
                                if (i == terms.Length - 1)
                                    term = ifNumber(term, "", "", i);
                                else if (i == terms.Length - 2)
                                    term = ifNumber(term, StripPunctuation(terms[i + 1]).ToLower(), "", i);
                                else
                                    term = ifNumber(term, StripPunctuation(terms[i + 1]).ToLower(), StripPunctuation(terms[i + 2]).ToLower(), i);
                                if (i < terms.Length && terms[i].ToLower() == "dollar")
                                {
                                    term = term + " dollar";
                                    this.i++;
                                }
                                else if (this.i < terms.Length && terms[this.i].ToLower() == "dollars")
                                {
                                    term = term + " dollars";
                                    this.i++;
                                }
                                else if (this.i < terms.Length - 1 && terms[this.i].ToLower() == "u.s" && terms[this.i + 1].ToLower() == "dollars")
                                {
                                    term = term + " dollars";
                                    this.i = this.i + 2;
                                }
                                else if (this.i < terms.Length - 1 && terms[this.i].ToLower() == "u.s" && terms[this.i + 1].ToLower() == "dollar")
                                {
                                    term = term + " dollar";
                                    this.i = this.i + 2;
                                }
                            }
                            else
                            {
                                if (this.i == terms.Length - 1)
                                    term = ifNotNumber(term, "", "", "", this.i);
                                else if (this.i == terms.Length - 2)
                                    term = ifNotNumber(term, StripPunctuation(terms[this.i + 1]).ToLower(), "", "", this.i);
                                else if (this.i == terms.Length - 3)
                                    term = ifNotNumber(term, StripPunctuation(terms[this.i + 1]).ToLower(), StripPunctuation(terms[this.i + 2]).ToLower(), "", this.i);
                                else
                                    term = ifNotNumber(term, StripPunctuation(terms[this.i + 1]).ToLower(), StripPunctuation(terms[this.i + 2]).ToLower(), StripPunctuation(terms[this.i + 3]).ToLower(), this.i);
                            }
                        }
                        if (term != "")
                        {
                            
                            if(isstem)
                            {
                                Stemmer stemmer = new SearchEngine.Stemmer();
                                term = stemmer.stemTerm(term);
                            }
                            if (!toindexer.Keys.Contains(term))
                            {
                                toindexer[term] = new List<Tuple<string, int>>();
                                toindexer[term].Add(new Tuple<string, int>(docids.ElementAt(count), 1));
                                fordocsdoc[currentdoc].dist_words = fordocsdoc[currentdoc].dist_words + 1;

                            }
                            else if (toindexer[term].ElementAt(toindexer[term].Count - 1).Item1 != currentdoc)
                            {
                                toindexer[term].Add(new Tuple<string, int>(currentdoc, 1));
                                fordocsdoc[currentdoc].dist_words = fordocsdoc[currentdoc].dist_words + 1;
                            }
                            else
                            {
                                int x = toindexer[term].ElementAt(toindexer[term].Count - 1).Item2;
                                x++;
                                if (x == 2)
                                    fordocsdoc[currentdoc].dist_words = fordocsdoc[currentdoc].dist_words - 1;
                                if (x >= fordocsdoc[currentdoc].max_tf)
                                {
                                    fordocsdoc[currentdoc].freq_term = term;
                                    fordocsdoc[currentdoc].max_tf = x;
                                }
                                toindexer[term].RemoveAt(toindexer[term].Count - 1);
                                toindexer[term].Add(new Tuple<string, int>(currentdoc, x));
                            }  
                        }
                    }
                    this.i++;
                }
                this.i = 0;
                count++;
            }
            return toindexer;
        }

        /// <summary>
        /// add all stopwords from the "stop_words" txt file
        /// words that are considered stop-words will not be inserted
        /// </summary>
        /// <param name="path"></param>
        public void AddStopWords(string path)
        {
            string[] lines = System.IO.File.ReadAllLines(path);
            foreach (string s in lines)
                StopWords.Add(s.ToLower());
        }

        /// <summary>
        /// if a term is a number- check the terms after it, to determine what is the next move using the parse rules
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public string ifNumber(string s1, string s2, string s3, int i)
        {
            string ans = ConvertNumbers(s1);
            int x;
            string whats2 = WhatsAfter(s2);
            if (whats2 != "nothing")
            {
                if (whats2 == "B")
                    ans = ans + "000M";
                else if (whats2 == "T")
                    ans = ans + "000000M";

                else if (whats2.Contains('.') || Int32.TryParse(whats2, out x))
                {
                    double number;
                    number = Double.Parse(s1) + Double.Parse(whats2);
                    string whats3 = WhatsAfter(s3);
                    ans = ConvertNumbers(number.ToString());
                    if (whats3 != "nothing" && (whats3.Contains('.') == true && Int32.TryParse(whats3, out x) == true))
                    {
                        if ((whats3 == "B"))
                            ans = ConvertNumbers((number * 1000).ToString());
                        else if (whats3 == "T")
                            ans = ConvertNumbers((number * 1000000).ToString());
                        else
                            ans = ans + whats3;
                    }
                    i = i + 1;
                }
                else
                    ans = ans + whats2;
                i++;
            }
            //Console.WriteLine("finished ifnumber " + stop.ElapsedMilliseconds);
            return ans;
        }

        /// <summary>
        /// if a term is not a number- check the terms after it, to determine what is the next move using the parse rules
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="s3"></param>
        /// <param name="s4"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        public string ifNotNumber(string s1, string s2, string s3, string s4, int i)
        {
            string ans = s1;
            int x;
            string whats1 = WhatsAfter(s1), whats2 = WhatsAfter(s2);
            if (s1 != "" && (whats1.Contains('.') || Int32.TryParse(whats1, out x)))
            {
                double number = Double.Parse(whats1);
                ans = ConvertNumbers(number.ToString());
                if (whats2 != "nothing" && whats2.Contains('.') == true && Int32.TryParse(whats2, out x) == true)
                {
                    if ((whats2 == "B"))
                        ans = ConvertNumbers((number * 1000).ToString()) + "M";
                    else if (whats2 == "T")
                        ans = ConvertNumbers((number * 1000000).ToString()) + "M";
                    else
                        ans = number + whats2;
                    i++;
                    if (s3 == "dollar")
                    {
                        ans = ans + " dollar";
                        i++;
                    }
                    else if (s3 == "dollars")
                    {
                        ans = ans + " dollars";
                        i++;
                    }
                    else if (s3 == "u.s" && s4 == "dollars")
                    {

                        ans = ans + " dollars";
                        i = i + 2;
                    }
                    else if (s3 == "u.s" && s4 == "dollar")
                    {
                        ans = ans + " dollar";
                        i = i + 2;
                    }
                }
            }
            else if (s1[0].Equals('$') && s1.Length>1)
            {
                ans = s1.Replace("$", "");
                if (whats2 != "nothing")
                {
                    if (IsNumber(ans))
                    {

                        if ((whats2 == "B"))
                            ans = (Double.Parse(ConvertNumbers(s1.Replace("$", ""))) * 1000).ToString() + "M";
                        else if (whats2 == "T")
                            ans = (Double.Parse(ConvertNumbers(s1.Replace("$", ""))) * 1000000).ToString() + "M";
                        else
                            ans = ConvertNumbers(ans) + whats2;
                        this.i=this.i+1;
                    }
                }
                ans = ans + " dollars";
            }

            else if (IsNumber(s1[0].ToString()))
            {
                if (s1[s1.Length - 1].Equals('m') && IsNumber(s1[s1.Length - 2].ToString()))
                    ans = ConvertNumbers(s1.Remove(s1.Length - 1)) + "M";
                else if (s1[s1.Length - 2].Equals('b') && s1[s1.Length - 1].Equals('n'))
                    ans = (Double.Parse(ConvertNumbers(s1.Remove(s1.Length - 2))) * 1000).ToString() + "M";
                if (s2 == "dollar")
                {
                    ans = ans + " dollar";
                    i++;
                }
                else if (s2 == "dollars")
                {
                    ans = ans + " dollars";
                    i++;
                }
                else if (s2 == "u.s" && s3 == "dollars")
                {

                    ans = ans + " dollars";
                    i = i + 2;
                }
                else if (s2 == "u.s" && s3 == "dollar")
                {
                    ans = ans + " dollar";
                    i = i + 2;
                }
            }
            else if (MassUnit(s1) != "notmass")
            {
                ans = MassUnit(s1);
            }
            else if (DistUnit(s1) != "notdist")
            {
                ans = DistUnit(s1);
            }
            else if (s1 == "between" && IsNumber(s2) && s3 == "and" && IsNumber(s4))
            {
                ans = s2 + "-" + s4;
                i = i + 3;
            }
            return ans;
        }

        /// <summary>
        /// determine if given string is a number or not
        /// </summary>
        /// <param name="numericterm"></param>
        /// <returns></returns>
        public bool IsNumber(string numericterm)
        {
            double x;
            return (!numericterm.Contains('-') && Double.TryParse(numericterm, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.CurrentCulture, out x));
        }

        /// <summary>
        /// converts from string to number
        /// if the number is bigger then 100,000 return the correct number in the form of (num)M
        /// </summary>
        /// <param name="numericterm"></param>
        /// <returns></returns>
        public string ConvertNumbers(string numericterm)
        {
            if (!IsNumber(numericterm))
                return numericterm;
            double x = Double.Parse(numericterm, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.AllowDecimalPoint);
            if (x > 1000000)
                return x / 1000000 + "M";
            return numericterm;
        }

        /// <summary>
        /// used to check which term comes after the current term the program is working on to determine which parser rule to enforce 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public string WhatsAfter(string exp)
        {
            if (exp == "million")
                return "M";
            if (exp == "billion")
                return "B";
            if (exp == "trillion")
                return "T";
            if (exp.Contains('/'))
            {
                int x, y;
                string[] isfraction = exp.Split('/');
                if (isfraction.Length == 2 && Int32.TryParse(isfraction[0], out x) && Int32.TryParse(isfraction[1], out y) && exp.Any(z => !char.IsLetter(z)) && Int32.Parse(isfraction[0].ElementAt(0).ToString()) != 0)
                    return ((double)x / y).ToString();
            }
            if (exp == "percent" || exp == "percentage")
                return "%";
            if (exp == "grams" || exp == "milligrams" || exp == "kilograms" || exp == "tonnes")
                return " " + exp;
            if (exp == "kilometer" || exp == "mile" || exp == "centimeter" || exp == "inch" || exp == "foot")
                return " " + exp;
            return "nothing";
        }
        /// <summary>
        /// used to seperate the diffrent cases of date identification
        /// </summary>
        /// <param name="current"></param>
        /// <param name="next1"></param>
        /// <param name="next2"></param>
        /// <returns></returns>
        public string WhichDate(string current, string next1, string next2)
        {
            bool numc = IsNumber(current), num1 = IsNumber(next1), num2 = IsNumber(next2);
            string whichc = WhichMonth(current), which1 = WhichMonth(next1);
            if (numc && which1 != "0" && num2)
            {
                if (IsDay(current) && (next2.Length == 2 || next2.Length == 4))
                {
                    if (next2.Length == 4)
                    {
                        if (current.Length == 1)
                            return next2 + "-" + which1 + "-0" + current;
                        return next2 + "-" + which1 + "-" + current;
                    }
                    else if (next2.Length == 2)
                    {
                        if (next2.CompareTo("00") >= 0 && next2.CompareTo("20") <= 0)
                            next2 = "20" + next2;
                        else
                            next2 = "19" + next2;
                        if (current.Length == 1)
                            return next2 + "-" + which1 + "-0" + current;
                        return next2 + "-" + which1 + "-" + current;
                    }
                }
            }

            if (whichc != "0" && num1 && num2 && next2.Length == 4 && IsDay(next1))
            {
                if (next1.Length == 1)
                    return next2 + "-" + whichc + "-0" + next1;
                return next2 + "-" + whichc + "-" + next1;
            }

            if (which1 != "0" && num2 && next2.Length == 4 && IsNumber(current[0].ToString()) && current.Substring(current.Length - 2, 2) == "th" && IsDay(current.Substring(0, current.Length - 2)))
            {
                if (current.Substring(0, current.Length - 2).Length == 1)
                    return next2 + "-" + which1 + "-0" + current.Substring(0, 1);
                return next2 + "-" + which1 + "-" + current.Substring(0, 2);
            }
            if (numc && IsDay(current) && which1 != "0")
            {
                if (current.Length == 1)
                    return which1 + "-0" + current;
                return which1 + "-" + current;
            }
            if (num1 && IsDay(next1) && whichc != "0")
            {
                if (next1.Length == 1)
                    return whichc + "-0" + next1;
                return whichc + "-" + next1;
            }
            if (whichc != "0" && num1 && next1.Length == 4)
                return next1 + "-" + whichc;
            return "notdate";
        }
        
        /// <summary>
        /// all rules regarding mass units parsing 
        /// </summary>
        /// <param name="mass"></param>
        /// <returns></returns>
        public string MassUnit(string mass)
        {
            if (mass.Length >= 2)
            {
                bool place = IsNumber(mass[mass.Length - 2].ToString());
                if (IsNumber(mass[0].ToString()) && !IsNumber(mass[mass.Length - 1].ToString()))
                {
                    if (!place && mass[mass.Length - 1] == 'g')
                        return mass.Substring(0, mass.Length - 1) + " gram";
                    if (!place && mass[mass.Length - 1] == 't')
                        return mass.Substring(0, mass.Length - 1) + " tonne";
                    if (place && mass.Substring(mass.Length - 2, 2) == "mg")
                        return mass.Substring(0, mass.Length - 2) + " milligram";
                    if (place && mass.Substring(mass.Length - 2, 2) == "kg")
                        return mass.Substring(0, mass.Length - 2) + " kilogram";
                }
            }
            return "notmass";
        }
        
        /// <summary>
        /// all rules regarding distance units parsing 
        /// </summary>
        /// <param name="dist"></param>
        /// <returns></returns>
        public string DistUnit(string dist)
        {
            if (dist.Length >= 2)
            {
                bool place = IsNumber(dist[dist.Length - 2].ToString());
                if (IsNumber(dist[0].ToString()) && !IsNumber(dist[dist.Length - 1].ToString()))
                {

                    if (place && dist.Substring(dist.Length - 2, 2) == "km")
                        return dist.Substring(0, dist.Length - 2) + " kilometer";
                    if (place && dist.Substring(dist.Length - 2, 2) == "mi")
                        return dist.Substring(0, dist.Length - 2) + " mile";
                    if (place && dist.Substring(dist.Length - 2, 2) == "cm")
                        return dist.Substring(0, dist.Length - 2) + " centimeter";
                    if (place && dist.Substring(dist.Length - 2, 2) == "in")
                        return dist.Substring(0, dist.Length - 2) + " inch";
                    if (place && dist.Substring(dist.Length - 2, 2) == "ft")
                        return dist.Substring(0, dist.Length - 2) + " foot";
                }
            }
            return "notdist";
        }
       
        /// <summary>
        /// used to determine if a number is suitable to be a day (between 1 and 31)
       /// </summary>
       /// <param name="day"></param>
       /// <returns></returns>
        public bool IsDay(string day)
        {
            string s;
            int x = 0;
            if (IsNumber(day))
            {
                s = ConvertNumbers(day);
                if (!s.Contains('.') && IsNumber(s))
                    x = Int32.Parse(day, System.Globalization.NumberStyles.AllowThousands);
            }
            return (x >= 1 && x <= 31);
        }
        
        /// <summary>
        /// used to determine  if we're dealing with a month.
        /// </summary>
        /// <param name="month">gets a string for check</param>
        /// <returns>the number of the month if it is a month, and 0 if not.</returns>
        public string WhichMonth(string month)
        {
            if (month == "january" || month == "jan")
                return "01";
            if (month == "february" || month == "feb")
                return "02";
            if (month == "march" || month == "mar")
                return "03";
            if (month == "april" || month == "apr")
                return "04";
            if (month == "may")
                return "05";
            if (month == "june" || month == "jun")
                return "06";
            if (month == "july" || month == "jul")
                return "07";
            if (month == "august" || month == "aug")
                return "08";
            if (month == "september" || month == "sep")
                return "09";
            if (month == "october" || month == "oct")
                return "10";
            if (month == "november" || month == "nov")
                return "11";
            if (month == "december" || month == "dec")
                return "12";
            return "0";
        }
        
        /// <summary>
        /// gets a string and removes all of it's punctuation.
        /// </summary>
        /// <param name="s">string to strip</param>
        /// <returns>string without punctuation</returns>
        public string StripPunctuation(string s)
        {
            string news = s;
            if (news.Contains("\n"))
                news = news.Replace("\n", "");
            while (news.Length > 0 && (AnotherPunc(news[0]) || AnotherPunc(news[news.Length - 1]) || char.IsPunctuation(news[0]) || (char.IsPunctuation(news[news.Length - 1]) && news[news.Length - 1] != '%')))
            {
                if (char.IsPunctuation(news[0]) || AnotherPunc(news[0]))
                    news = news.Remove(0, 1);
                if (news.Length > 0 && (char.IsPunctuation(news[news.Length - 1]) || AnotherPunc(news[news.Length - 1])) && news[news.Length - 1] != '%')
                    news = news.Remove(news.Length - 1);
            }
            return news;
        }
        
        /// <summary>
        /// checks if a certein char is a unique punctuation.
        /// </summary>
        /// <param name="c">char to check</param>
        /// <returns>true if it is a punctuation, and false otherwise.</returns>
        public bool AnotherPunc(char c)
        {
            if (c == '~' || c == '`' || c == '^' || c == '+' || c == '=' || c == '|' || c == '<' || c == '>' || c == ' ')
                return true;
            return false;
        }
    }
}
