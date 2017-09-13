
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Windows.Forms;
using Engine;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SearchEngine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// readfile instance
        /// </summary>
        ReadFile readfile;
        /// <summary>
        /// paths from/to we will load/save 
        /// </summary>
        string LoadPath, SavePath;
        /// <summary>
        /// stopwatch used to measure time
        /// </summary>
        Stopwatch stop;
        /// <summary>
        /// our way of knowing if we initiated an instance of "readfile"
        /// </summary>
        bool isread;
        /// <summary>
        /// initialize the mainwindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            isread = false;
        }
        /// <summary>
        /// will be used in part 2- not yet implemented
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// used to clear out all instances and computer memory 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restart_Click(object sender, RoutedEventArgs e)
        {
            if (isread)
            {
                Directory.Delete(SavePath, true);
                readfile.indexer.Dictionary.Clear();
                readfile.Languages.Clear();
                comboBox.ItemsSource = readfile.Languages;
                load.Text = "";
                save.Text = "";
                readfile.numofdocs = 0;
                readfile.indexer.unique = 0;
                isread = false;
            }
        }

        /// <summary>
        /// open a "browse" dialog to choose folder through the windows interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loadb_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            fbd.ShowNewFolderButton = false;
            load.Text = fbd.SelectedPath;
            LoadPath = load.Text;
        }

        /// <summary>
        /// open a "browse" dialog to choose folder through the windows interface
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveb_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            fbd.ShowNewFolderButton = true;
            save.Text = fbd.SelectedPath;
            SavePath = save.Text;
        }

        /// <summary>
        /// creates a dictionary, documents,language and posting files given a valid load path and a valid save path.
        /// will use stemming if choosen to and will announce Number of indexed documents, Number of unique terms
        /// and Running time in seconds when finished
              
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void start_Click(object sender, RoutedEventArgs e)
        {
            stop = new Stopwatch();
            if (load.Text == "" || save.Text == "")
                System.Windows.MessageBox.Show("Please enter the save and load paths");
            if(!Directory.Exists(load.Text) || !Directory.Exists(save.Text))
                System.Windows.MessageBox.Show("Directory does not exists");
            else
            {
                if (stemming.IsChecked == true)
                {
                    readfile = new Engine.ReadFile(LoadPath, SavePath + "\\stem\\", true);
                }
                else
                {
                    readfile = new Engine.ReadFile(LoadPath, SavePath + "\\withoutstem\\", false);
                }
                isread = true;
                stop.Start();
                readfile.readFiles();
                stop.Stop();
                System.Windows.MessageBox.Show("Number of indexed documents: " + readfile.numofdocs + "\nNumber of unique terms: " + readfile.indexer.unique + "\nRunning time in seconds: " + stop.ElapsedMilliseconds / 1000);
                comboBox.ItemsSource = readfile.Languages;
            }
        }
        /// <summary>
        /// will display all terms and their Number of appearances in alphabetical order, if dictioanry is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void display_dictionary_Click(object sender, RoutedEventArgs e)
        {
            if (!isread)
                System.Windows.MessageBox.Show("There is no dictionary to show");
            else
            {
                System.Windows.Controls.ListView showDic = new System.Windows.Controls.ListView();
                foreach (string s in readfile.indexer.Dictionary.Keys)
                    showDic.Items.Add(s + " - Number of appearances: " + readfile.indexer.Dictionary[s][1]);

                Window1 win1 = new Window1();
                win1.Content = showDic;
                win1.Show();
            }
        }

        /// <summary>
        /// will load a dictionary( and all documents,language and posting files) to the program
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void load_dictionary_Click(object sender, RoutedEventArgs e)
        {
            StreamReader sr, sr1;
            if (save.Text == "")
                System.Windows.MessageBox.Show("Please enter the save path");
            else
            {
                if (stemming.IsChecked == true)
                    sr = new StreamReader(SavePath + "\\stem\\dictionary.txt");
                else
                    sr = new StreamReader(SavePath + "\\withoutstem\\dictionary.txt");
                if (isread)
                    readfile.indexer.Dictionary.Clear();
                else
                {
                    bool stemm;
                    if (stemming.IsChecked == true)
                        stemm = true;
                    else
                        stemm = false;
                    readfile = new Engine.ReadFile(save.Text,save.Text, stemm);
                    isread = true;
                }
                string[] s;
                do
                {
                    s = sr.ReadLine().Split(' ');
                    if (s.Length == 4)
                    {
                        readfile.indexer.Dictionary[s[0]] = new int[3] { Int32.Parse(s[1]), Int32.Parse(s[2]), Int32.Parse(s[3]) };
                        if (readfile.indexer.Dictionary[s[0]][1] == 1)
                            readfile.indexer.unique++;
                    }
                        
                    else
                    {
                        string term = "";
                        for (int i = 0; i < s.Length - 3; i++)
                            term += " " + s[i];
                        term = term.Substring(1);
                        readfile.indexer.Dictionary[term] = new int[3] { Int32.Parse(s[s.Length - 3]), Int32.Parse(s[s.Length - 2]), Int32.Parse(s[s.Length - 1]) };
                        if (readfile.indexer.Dictionary[term][1] == 1)
                            readfile.indexer.unique++;
                    }

                } while (!sr.EndOfStream);
                if (stemming.IsChecked == true)
                    sr1 = new StreamReader(SavePath + "\\stem\\languages.txt");
                else
                    sr1 = new StreamReader(SavePath + "\\withoutstem\\languages.txt");
                readfile.Languages.Clear();
                do
                {
                    readfile.Languages.Add(sr1.ReadLine());
                } while (!sr1.EndOfStream);
                comboBox.ItemsSource = readfile.Languages;
                sr.Close(); sr1.Close();
                System.Windows.MessageBox.Show("Done loading");
            }
        }

        /// <summary>
        /// if a user changes the path- update the path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void load_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadPath = load.Text;
        }
        /// <summary>
        /// if a user changes the path- update the path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_TextChanged(object sender, TextChangedEventArgs e)
        {
            SavePath = save.Text;
        }

        /// <summary>
        /// display languages in combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBox_Loaded(object sender, SelectionChangedEventArgs e)
        {
            comboBox.ItemsSource = readfile.Languages;

        }
    }
}
