using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace feladatoknyilvantartasa_tasnadykristof
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            listBox1.ItemsSource = feladatok;
            listBox2.ItemsSource = toroltek;
       
            if (File.Exists("Igen.txt"))
            {
                string[] lines = File.ReadAllLines("Igen.txt");
                foreach (string line in lines)
                {
                    string[] parts = line.Split(new char[] { ':' }, 3);
                    if (parts.Length == 3)
                    {
                        bool deleted = parts[0] == "deleted";
                        bool done = parts[1] == "checked";
                        string text = parts[2];
                        CheckBox uj = new CheckBox();
                        uj.IsChecked = done;
                        uj.Content = text;
                        uj.Checked += new RoutedEventHandler(checkBox_Checked_1);
                        uj.Unchecked += new RoutedEventHandler(checkBox_Unchecked_1);
                        if (deleted)
                        {
                            feladatok.Add(uj);
                            listBox1.Items.Refresh();

                        } else
                        {
                            toroltek.Add(uj);
                            listBox2.Items.Refresh();
                        }
                    }
                }
            }
        }

        List<CheckBox> feladatok = new List<CheckBox>();
        List<CheckBox> toroltek = new List<CheckBox>();

        private void ujHozzaadasa_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != "")
            {
                CheckBox uj = new CheckBox();
                uj.Content = textBox.Text;
                uj.Checked += new RoutedEventHandler(checkBox_Checked_1);
                uj.Unchecked += new RoutedEventHandler(checkBox_Unchecked_1);
                feladatok.Add(uj);
                listBox1.Items.Refresh();
                textBox.Text = "";
            }
        }

        private void checkBox_Checked_1(object sender, RoutedEventArgs e)
        {
            CheckBox aktualis = (CheckBox)sender;
            aktualis.FontStyle = FontStyles.Italic;
            aktualis.Foreground = Brushes.Gray;
        }

        private void checkBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            CheckBox aktualis = (CheckBox)sender;
            aktualis.FontStyle = FontStyles.Normal;
            aktualis.Foreground = Brushes.Black;
        }

        private void kijeloltTorlese_Click(object sender, RoutedEventArgs e)
        {
            CheckBox kijelolt = (CheckBox)listBox1.SelectedItem;
            toroltek.Add(kijelolt);
            feladatok.Remove(kijelolt);
            listBox1.Items.Refresh();
            listBox2.Items.Refresh();
        }

        private void kijeloltVisszaallit_Click(object sender, RoutedEventArgs e)
        {
            CheckBox kijelolt = (CheckBox)listBox2.SelectedItem;
            feladatok.Add(kijelolt);
            toroltek.Remove(kijelolt);
            listBox1.Items.Refresh();
            listBox2.Items.Refresh();
        }

        private void kijeloltModositas_Click(object sender, RoutedEventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                int i = listBox1.SelectedIndex;

                feladatok[i].Content = textBox.Text;
                listBox1.Items.Refresh();
            }
        }
        

      

        private void kijeloltVegleg_Click(object sender, RoutedEventArgs e)
        {
        
            if (listBox2.SelectedIndex != -1)
            {
              int i = listBox2.SelectedIndex;
              toroltek.RemoveAt(i);
              listBox2.Items.Refresh();
                
               
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            StreamWriter writer = new StreamWriter("Igen.txt");
            foreach (CheckBox b in feladatok)
            {
                string done = (bool)b.IsChecked ? "checked" : "unchecked";
                writer.WriteLineAsync("undeleted:" + done + ":" + b.Content);
            }
            foreach (CheckBox b in toroltek)
            {
                string done = (bool)b.IsChecked ? "checked" : "unchecked";
                writer.WriteLineAsync("deleted:" + done + ":" + b.Content);
            }
            writer.Close();
        }
    }
}
