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
using System.IO;
using Microsoft.Win32;
using System.Security.Cryptography;

namespace Nevgenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> originalcsaladNevek = new List<string>();
        List<string> originalutoNevek = new List<string>();
        List<string> csaladNevek = new List<string>();
        List<string> utoNevek = new List<string>();
        Random r;
        public MainWindow()
        {
            InitializeComponent();
            r = new Random();
        }

        private void btnBetoltCsaladnevek_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == true)
            {
                foreach (var csnev in File.ReadAllLines(ofd.FileName).ToList())
                {
                    csaladNevek.Add(csnev);
                    originalcsaladNevek.Add(csnev);
                }
                lbCsaladnevek.ItemsSource = csaladNevek;
            }
        }

        private void btnBetoltUtonevek_Click(object sender, RoutedEventArgs e)
        {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == true)
                {
                    foreach (var nev in File.ReadAllLines(ofd.FileName).ToList())
                    {
                        utoNevek.Add(nev); 
                        originalutoNevek.Add(nev);
                    }
                    lbUtonevek.ItemsSource = utoNevek;
                }
        }

        private void btnGeneral_Click(object sender, RoutedEventArgs e)
        {
            stbRendezes.Content = "";
            if (lbCsaladnevek.Items.Count == 0 || lbUtonevek.Items.Count == 0)
            {
                MessageBox.Show("Nincs elég név a listákban!");
                return;
            }
            int sliderErtek = Convert.ToInt32(txbLetrehozando.Text);
            for (int i = 0; i < sliderErtek; i++)
            {
                if (radioEgy.IsChecked == true)
                {
                    int kivalasztottCsaladnev = r.Next(csaladNevek.Count());
                    int kivalasztottUtonev = r.Next(utoNevek.Count());
                    lbGeneraltNevek.Items.Add($"{csaladNevek[kivalasztottCsaladnev]} {utoNevek[kivalasztottUtonev]}");
                    csaladNevek.RemoveAt(kivalasztottCsaladnev);
                    utoNevek.RemoveAt(kivalasztottUtonev);
                }
                else
                {
                    int kivalasztottCsaladnev = r.Next(csaladNevek.Count());
                    int kivalasztottUtonev = r.Next(utoNevek.Count());
                    int kivalasztottUtonev2 = r.Next(utoNevek.Count());
                    lbGeneraltNevek.Items.Add($"{csaladNevek[kivalasztottCsaladnev]} {utoNevek[kivalasztottUtonev]} {utoNevek[kivalasztottUtonev2]}");
                    csaladNevek.RemoveAt(kivalasztottCsaladnev);
                    if (kivalasztottUtonev > kivalasztottUtonev2)
                    {
                        utoNevek.RemoveAt(kivalasztottUtonev);
                        utoNevek.RemoveAt(kivalasztottUtonev2);
                    }
                    else
                    {
                        utoNevek.RemoveAt(kivalasztottUtonev2);
                        utoNevek.RemoveAt(kivalasztottUtonev);
                    }
                }
                lbCsaladnevek.Items.Refresh();
                lbUtonevek.Items.Refresh();
            }
            lbGeneraltNevek.Items.MoveCurrentToLast();
            lbGeneraltNevek.ScrollIntoView(lbGeneraltNevek.Items.CurrentItem);
        }

        private void btnMent_Click(object sender, RoutedEventArgs e)
        {
            string asd = "aaaaaaaaaa";
            if (lbGeneraltNevek.Items.IsEmpty == true)
            {
                MessageBox.Show("Nincsen menthető név a listában!");
            }
            else
            {
                List<string> ls = new List<string>();
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.AddExtension = true;
                sfd.DefaultExt = "txt";
                sfd.Filter = "szöveges fájl (*txt)|*.txt|csv fájl (*csv)|*.csv|összes fájl (*.*)|*.*";
                sfd.Title = "Adja meg a névsor nevét!";
                if (sfd.ShowDialog() == true)
                {
                    var ex = System.IO.Path.GetExtension(sfd.FileName);
                    switch (ex.ToLower())
                    {
                        case ".csv":
                            foreach (var item in lbGeneraltNevek.Items)
                                ls.Add(Convert.ToString(item).Replace(' ', ';'));
                            File.WriteAllLines(sfd.FileName, ls);
                            break;
                        case ".txt":
                            foreach (var item in lbGeneraltNevek.Items)
                                ls.Add((string)item);
                            File.WriteAllLines(sfd.FileName, ls);
                            break;
                        default:
                            break;
                    } 
                }
                MessageBox.Show("Sikeres mentés!");
            }
        }

        private void btnRendez_Click(object sender, RoutedEventArgs e)
        {
            if (lbGeneraltNevek.Items.IsEmpty == true)
            {
                MessageBox.Show("Nincsen rendezendő név a listában!");
            }
            else
            {
                lbGeneraltNevek.Items.SortDescriptions.Add(new System.ComponentModel.SortDescription("", System.ComponentModel.ListSortDirection.Ascending));
                stbRendezes.Content = "Rendezett névsor!";
            }
        }

        private void btnTorol_Click(object sender, RoutedEventArgs e)
        {
            if (lbGeneraltNevek.Items.IsEmpty == true)
            {
                MessageBox.Show("A generált nevek listája üres!");
            }
            else
            {
                lbGeneraltNevek.Items.Clear();
                csaladNevek.Clear();
                foreach (var item in originalcsaladNevek)
                    csaladNevek.Add(item);
                utoNevek.Clear();
                foreach (var item in originalutoNevek)
                    utoNevek.Add(item);
                lbCsaladnevek.Items.Refresh();
                lbUtonevek.Items.Refresh();
                stbRendezes.Content = "";
            }   
        }

        private void lbGeneraltnevek_Nevkivetel(object sender, MouseButtonEventArgs e)
        {
            string nev = lbGeneraltNevek.SelectedItem.ToString();
            var nevek = nev.Split(' ');
            csaladNevek.Add(nevek[0]);
            utoNevek.Add(nevek[1]);
            if (nevek.Length == 3)
                utoNevek.Add(nevek[2]);
            lbUtonevek.Items.Refresh();
            lbCsaladnevek.Items.Refresh();
            lbGeneraltNevek.Items.RemoveAt(lbGeneraltNevek.SelectedIndex);
        }
    }
}
