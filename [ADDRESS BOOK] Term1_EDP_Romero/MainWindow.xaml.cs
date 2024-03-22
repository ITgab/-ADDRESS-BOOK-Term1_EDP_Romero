using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Policy;
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

namespace _ADDRESS_BOOK__Term1_EDP_Romero
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<int, List<string>> _AddressBook = new Dictionary<int, List<string>>();
        public string _FileName = "AddressBook.csv";
        public string _line = "";

        public bool _isCreate = false;

        public MainWindow()
        {
            InitializeComponent();
            ReadCSV();
        }

        public void ReadCSV()
        {
            _isCreate = false; //reset

            string[] temp = new string[] { };
            int rows = 0;
            _AddressBook.Clear();

            using (StreamReader sr = new StreamReader(_FileName))
            {
                while ((_line = sr.ReadLine()) != null)
                {
                    if (_line.Length > 0)
                    {
                        if (rows > 0)
                        {
                            temp = _line.Split(',');
                            _AddressBook.Add(rows, new List<string> { temp[0], temp[1], temp[2], temp[3], temp[4], temp[5] });
                        }
                    }
                    rows++;
                }
            }
            AddtoList();
        }

        private void AddtoList()
        {
            AddressListBox.Items.Clear();

            for (int i = 1; i < _AddressBook.Count+1; i++) //i is the key and key starts at 1
            {
                ListViewItem account = new ListViewItem();
                account.Tag = i;
                account.Content = _AddressBook[i][2] + ", " + _AddressBook[i][0];
                AddressListBox.Items.Add(account); //surname, first name
            }
        }

        private void WriteCSV()
        {
            string[] header = new string[5];
            int rows = 0;

            //get the header
            using (StreamReader sr = new StreamReader(_FileName))
            {
                while ((_line = sr.ReadLine()) != null)
                {
                    if (rows == 0)
                    {
                        header = _line.Split(',');
                    }
                    rows++;
                }
            }

            //write csv file
            using (StreamWriter sw = new StreamWriter(_FileName))
            {
                //write header
                for (int x = 0; x < header.Length; x++)
                {
                    sw.Write(header[x]);
                    if (x < header.Length - 1)
                        sw.Write(",");
                }
                sw.WriteLine();

                foreach (KeyValuePair<int, List<string>> kvp in _AddressBook)
                {
                    foreach (string value in kvp.Value)
                    {
                        sw.Write(value);
                        sw.Write(",");
                    }
                    sw.WriteLine();
                }
            }
        }

        private void AddressListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchTB.Text = "Search"; //reset

            ListBoxItem selectedACC = (ListBoxItem)AddressListBox.SelectedItem;
            
            if (selectedACC != null)
            {
                int key = (int)selectedACC.Tag;

                FirstNameTB.Text = _AddressBook[key][0];
                MiddleNameTB.Text = _AddressBook[key][1];
                LastNameTB.Text = _AddressBook[key][2];
                EmailTB.Text = _AddressBook[key][3];
                PhoneTB.Text = _AddressBook[key][4];
                AddressTB.Text = _AddressBook[key][5];
            }           
        }

        private void SearchBttn_Click(object sender, RoutedEventArgs e)
        {
            AddressListBox.Items.Clear();

            string searchItem = SearchTB.Text.ToString();

            for (int i = 1; i < _AddressBook.Count+1; i++) //i is the key and key starts at 1
            {
                for (int j = 0; j < _AddressBook[i].Count; j++)
                {
                    if (_AddressBook[i][j].Contains(searchItem))
                    {
                        ListViewItem account = new ListViewItem();
                        account.Tag = i;
                        account.Content = _AddressBook[i][2] + ", " + _AddressBook[i][0];
                        AddressListBox.Items.Add(account); //surname, first name
                        break;
                    }
                }
            }
        }

        private void SaveBttn_Click(object sender, RoutedEventArgs e)
        {
            SearchTB.Text = "Search"; //reset

            string firstName = "";
            string middleName = "";
            string lastName = "";
            string email = "";
            string phone = "";
            string address = "";

            int key = 0;

            if (_isCreate) //for create
            {
                //requires first name or last name at least
                if (FirstNameTB.Text == "" && LastNameTB.Text == "")
                {
                    MessageBox.Show("Please fill up the first name and last name. Please Click Create New Account again.");
                }


                else
                {                    
                    firstName = FirstNameTB.Text.ToString();
                    middleName = MiddleNameTB.Text.ToString();
                    lastName = LastNameTB.Text.ToString();
                    email = EmailTB.Text.ToString();
                    phone = PhoneTB.Text.ToString();
                    address = AddressTB.Text.ToString();

                    key = _AddressBook.Count + 1;

                    _AddressBook.Add(key, new List<string> { firstName, middleName, lastName, email, phone });
                }
            }

            else //for edit
            {
                ListViewItem saveACC = (ListViewItem)AddressListBox.SelectedItem;
                key = (int)saveACC.Tag;

                firstName = FirstNameTB.Text.ToString();
                middleName = MiddleNameTB.Text.ToString();
                lastName = LastNameTB.Text.ToString();
                email = EmailTB.Text.ToString();
                phone = PhoneTB.Text.ToString();
                address = AddressTB.Text.ToString();

                _AddressBook[key] = new List<string> { firstName, middleName, lastName, email, phone };
            }           

            WriteCSV();
            ReadCSV();
        }

        private void DeleteBttn_Click(object sender, RoutedEventArgs e)
        {
            SearchTB.Text = "Search"; //reset

            FirstNameTB.Clear();
            MiddleNameTB.Clear();
            LastNameTB.Clear();
            EmailTB.Clear();
            PhoneTB.Clear();
            AddressTB.Clear();

            ListViewItem deleteACC = (ListViewItem)AddressListBox.SelectedItem;
            int key = (int)deleteACC.Tag;

            _AddressBook.Remove(key);
            WriteCSV();
            ReadCSV();
        }

        private void CreateBttn_Click(object sender, RoutedEventArgs e)
        {
            SearchTB.Text = "Search"; //reset

            FirstNameTB.Clear();
            MiddleNameTB.Clear();
            LastNameTB.Clear();
            EmailTB.Clear();
            PhoneTB.Clear();
            AddressTB.Clear();

            _isCreate = true;
        }

        private void ViewBttn_Click(object sender, RoutedEventArgs e)
        {
            SearchTB.Text = "Search";
            ReadCSV();
        }
    }
}
