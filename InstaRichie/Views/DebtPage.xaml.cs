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
            DateStamp.Date = DateTime.Now; // gets current date and time
            DateStamp1.Date = DateTime.Now;
            Resuts();
        }

        public void Resuts()
        {
            conn.CreateTable<Debt>();
            var query = conn.Table<Debt>();
            DebtList1.ItemsSource = query.ToList();
            DebtList.ItemsSource = query.ToList();

            conn.CreateTable<Accounts>();
            var accupdate = conn.Table<Accounts>();
            AccountSelct.ItemsSource = accupdate.ToList();
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
                Resuts();
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
                string AccSelection = ((Debt)DebtList1.SelectedItem).DebtName;
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
                    DebtList1.ItemsSource = query1.ToList();
                }

                conn.CreateTable<Debt>();
                var query = conn.Table<Debt>();
                DebtList1.ItemsSource = query.ToList();

            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Resuts();
        }

        private async void PayDebt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Calculations nnn = new  Calculations();
                string CDay = DateStamp.Date.Value.Day.ToString();
                string CMonth = DateStamp.Date.Value.Month.ToString();
                string CYear = DateStamp.Date.Value.Year.ToString();
                string FinalDate = "" + CMonth + "/" + CDay + "/" + CYear;
                double Money = Convert.ToDouble(MoneyIn1.Text);
                double DebtBalance = nnn.DebtCalculation();
                double Dmoney = DebtBalance + Money;

                string AccountSelection = ((Accounts)AccountSelct.SelectedItem).AccountName;
                if(AccountSelection.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No Account Selected", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if(AccountBalance() < Money)
                {
                    MessageDialog dialog = new MessageDialog("You're spending more than what you have", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if(Dmoney > 0)
                {
                    MessageDialog dialog = new MessageDialog("You can't pay more than the Debt Amount : " + DebtBalance + "", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.Insert(new Debt
                    {
                        DateofDebt = FinalDate,
                        DebtName = Desc1.Text,
                        DebtAmount = Money
                    });
                    double FinalAmount = AccountBalance() - Money;
                    var query3 = conn.Query<Accounts>("UPDATE Accounts SET InitialAmount = " + FinalAmount + " WHERE AccountName ='" + AccountSelection + "'");
                    MessageDialog Confirmed = new MessageDialog("Debt Paid successfully");
                    await Confirmed.ShowAsync();
                    Resuts();

                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Value or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is NullReferenceException)
                {
                    MessageDialog dialog = new MessageDialog("Please enter the Debt Details", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }
        public double AccountBalance()
        {
            string AccountSelection = ((Accounts)AccountSelct.SelectedItem).AccountName;
            conn.CreateTable<Accounts>();
            var query12 = conn.Query<Accounts>("SELECT * FROM Accounts WHERE AccountName ='" + AccountSelection + "'");
            var sumProd = query12.AsEnumerable().Sum(o => o.InitialAmount);
            double Total = sumProd;
            return Total;
        }

        private void DebtPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int no = DebtPivot.SelectedIndex;
            if(no == 0)
            {
                AddDebtFooter.Visibility = Visibility.Visible;
                PayDebtFooter.Visibility = Visibility.Collapsed;
            }
            else
            {
                PayDebtFooter.Visibility = Visibility.Visible;
                AddDebtFooter.Visibility = Visibility.Collapsed;
            }
        }
    }
}
