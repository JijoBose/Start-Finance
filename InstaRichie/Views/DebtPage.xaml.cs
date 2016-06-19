using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using InstaRichie.Models;
using InstaRichie.ViewModels;
using Windows.UI.Popups;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace InstaRichie.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DebtPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public DebtPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            conn.CreateTable<Debt>();
            DateStamp.Date = DateTime.Now; // gets current date and time

            conn.CreateTable<Debt>();
            var query = conn.Table<Debt>();
            DebtList.ItemsSource = query.ToList();
        }

        private async void AddData(object sender, RoutedEventArgs e)
        {
            Calculations nnn = new Calculations();
            try
            {
                string CDay = DateStamp.Date.Value.Day.ToString();
                string CMonth = DateStamp.Date.Value.Month.ToString();
                string CYear = DateStamp.Date.Value.Year.ToString();
                string FinalDate = "" + CMonth + "/" + CDay + "/" + CYear;

                if (DebtModeSelect.SelectionBoxItem.ToString() == "Add Debts")
                {
                    if (Desc.Text == "")
                    {
                        MessageDialog dialog = new MessageDialog("Not entered Debt name");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        double Money = Convert.ToDouble(MoneyIn.Text);
                        double Dmoney = 0 - Money;
                        conn.Insert(new Debt()
                        {
                            DateofDebt = FinalDate,
                            DebtName = Desc.Text,
                            DebtAmount = Dmoney
                        });
                    }
                }
                else
                {
                    double Money = Convert.ToDouble(MoneyIn.Text);
                    double Dmoney = Money;
                    double DebtAmt = nnn.DebtCalculation() + Dmoney;

                    if (DebtAmt > 0)
                    {
                        MessageDialog dialog = new MessageDialog("You Entered more than the debt value " + nnn.DebtCalculation() + " ", "Oops..!");
                        await dialog.ShowAsync();
                    }
                    else
                    {
                        conn.Insert(new Debt()
                        {
                            DateofDebt = FinalDate,
                            DebtName = Desc.Text,
                            DebtAmount = Dmoney
                        });
                    }
                }
                conn.CreateTable<Debt>();
                var query = conn.Table<Debt>();
                DebtList.ItemsSource = query.ToList();
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Value or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("A Similar Debt Name already exists, try different name", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void ClearFileds_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = ((Debt)DebtList.SelectedItem).DebtName;
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<Debt>();
                    var query1 = conn.Table<Debt>();
                    var query3 = conn.Query<Debt>("DELETE FROM Debt WHERE DebtName ='" + AccSelection + "'");
                    DebtList.ItemsSource = query1.ToList();
                }

                conn.CreateTable<Debt>();
                var query = conn.Table<Debt>();
                DebtList.ItemsSource = query.ToList();

            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            conn.CreateTable<Debt>();
            var query = conn.Table<Debt>();
            DebtList.ItemsSource = query.ToList();
        }
    }
}
