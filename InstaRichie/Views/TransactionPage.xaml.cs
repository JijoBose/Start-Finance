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
using JustWallet.Models;
using Windows.UI.Popups;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace JustWallet.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransactionPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public TransactionPage()
        {
            this.InitializeComponent();

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            conn.CreateTable<Transactions>();
            DateStamp.Date = DateTime.Now; // gets current date and time
            TranDateStamp.Date = DateTime.Now;

            Results();
        }

        public void Results()
        {
            conn.CreateTable<Accounts>();
            var query1 = conn.Table<Accounts>();
            AccountsListSel.ItemsSource = query1.ToList();
            FromAccountsSel.ItemsSource = query1.ToList();
            ToAccountSel.ItemsSource = query1.ToList();
        }

        private async void AddData(object sender, RoutedEventArgs e)
        {
            //string AccountSelection = ((Accounts)AccountsListSel.SelectedItem).AccountName;
            ///// inserts the data if money value is null
            try
            {
                string CDay = DateStamp.Date.Value.Day.ToString();
                string CMonth = DateStamp.Date.Value.Month.ToString();
                string CYear = DateStamp.Date.Value.Year.ToString();
                string FinalDate = "" + CMonth + "/" + CDay + "/" + CYear;

                string AccountSelection = ((Accounts)AccountsListSel.SelectedItem).AccountName;
                /// inserts the data if money value is null
                /// 
                string Doublecheck = AccountsListSel.SelectedIndex.ToString();
                if (AccountsListSel.SelectedItem.ToString() == " ") // Needed Improvement
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Amount or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    //// detects if income 
                    if (IncExpSelect.SelectionBoxItem.ToString() == "Income")
                    {
                        conn.Insert(new Transactions()
                        {
                            DateOfTran = FinalDate,
                            TranType = IncExpSelect.SelectionBoxItem.ToString(),
                            Description = Desc.Text,
                            Account = AccountSelection,
                            Amount = Convert.ToDouble(MoneyIn.Text)
                        });
                        var query3 = conn.Query<Accounts>("UPDATE Accounts SET InitialAmount = " + TransactionToAccountIncome() + " WHERE AccountName ='" + AccountSelection + "'");
                        MessageDialog Confirmed = new MessageDialog("Transaction successful");
                        await Confirmed.ShowAsync();
                    }
                    else /// detects if expense
                    {
                        double EMoney = Convert.ToDouble(MoneyIn.Text);
                        double FMoney = 0 - EMoney;
                        if (EMoney > AccountBalance())
                        {
                            MessageDialog dialog = new MessageDialog("You're spending more than what you have");
                            await dialog.ShowAsync();
                        }
                        else
                        {
                            conn.Insert(new Transactions()
                            {
                                DateOfTran = FinalDate,
                                TranType = IncExpSelect.SelectionBoxItem.ToString(),
                                Description = Desc.Text,
                                Account = AccountSelection,
                                Amount = FMoney
                            });

                            double Exp = FMoney + TransactionToExpense();
                            var query3 = conn.Query<Accounts>("UPDATE Accounts SET InitialAmount = " + Exp + " WHERE AccountName ='" + AccountSelection + "'");
                            MessageDialog Confirmed = new MessageDialog("Transaction successful");
                            await Confirmed.ShowAsync();
                        }
                    }
                }

                Results();
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Amount or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is NullReferenceException)
                {
                    MessageDialog dialog = new MessageDialog("Please select an Account", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    // none
                }
            }

        }

        public double AccountBalance()
        {
            string AccountSelection = ((Accounts)AccountsListSel.SelectedItem).AccountName;
            conn.CreateTable<Accounts>();
            var query12 = conn.Query<Accounts>("SELECT * FROM Accounts WHERE AccountName ='" + AccountSelection + "'");
            var sumProd = query12.AsEnumerable().Sum(o => o.InitialAmount);
            double Total = sumProd;
            return Total;
        }
        public double TransactionToAccountIncome()
        {
            string AccountSelection = ((Accounts)AccountsListSel.SelectedItem).AccountName;

            conn.CreateTable<Accounts>();
            var query1 = conn.Query<Accounts>("SELECT * FROM Accounts WHERE AccountName ='" + AccountSelection + "'");
            var sumProdQty1 = query1.AsEnumerable().Sum(o => o.InitialAmount);

            //conn.CreateTable<Transactions>();
            //var query2 = conn.Query<Transactions>("SELECT * FROM Transactions WHERE Account = '" + AccountSelection + "'");
            //var sumProdQty2 = query2.AsEnumerable().Sum(o => o.Amount);
            double DumbMney = Convert.ToDouble(MoneyIn.Text);
            double FinalAmountShip = sumProdQty1 + DumbMney;

            return FinalAmountShip;
        }

        public double TransactionToExpense()
        {
            string AccountSelection = ((Accounts)AccountsListSel.SelectedItem).AccountName;

            conn.CreateTable<Accounts>();
            var query1 = conn.Query<Accounts>("SELECT * FROM Accounts WHERE AccountName ='" + AccountSelection + "'");
            var sumProdQty1 = query1.AsEnumerable().Sum(o => o.InitialAmount);
            double FinalTot = sumProdQty1;
            return FinalTot;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            Results();
        }

        /// <summary>
        /// Implementing Internal Transfer for Transactions.
        /// Code Below
        /// </summary>
        ///
        private async void Transfer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string CDay = DateStamp.Date.Value.Day.ToString();
                string CMonth = DateStamp.Date.Value.Month.ToString();
                string CYear = DateStamp.Date.Value.Year.ToString();
                string FinalDate = "" + CMonth + "/" + CDay + "/" + CYear;

                // Getting data from the combobox
                string FromData = ((Accounts)FromAccountsSel.SelectedItem).AccountName;
                string ToData = ((Accounts)ToAccountSel.SelectedItem).AccountName;

                // Populating data in TO and From Account
                //conn.CreateTable<Accounts>();
                double TMoney = Convert.ToDouble(TransferMoney.Text);
                double TINMoney = TMoney;
                double FMoney = 0 - TINMoney;

                if (FromData == ToData)
                {
                    MessageDialog dialog = new MessageDialog("Both Accounts are the Same", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (TINMoney > TAccountBalance())
                {
                    MessageDialog dialog = new MessageDialog("You can't transfer more than what you have", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    var query3 = conn.Query<Accounts>("UPDATE Accounts SET InitialAmount = " + TransferToAccount() + " WHERE AccountName ='" + FromData + "'");

                    // Transfered TO
                    conn.Insert(new Transactions()
                    {
                        Account = ToData,
                        Amount = TINMoney,
                        Description = TDesc.Text,
                        TranType = "Internal Transfer",
                        DateOfTran = FinalDate
                    });
                    double Exp = TINMoney + TransferToExpense();
                    var query4 = conn.Query<Accounts>("UPDATE Accounts SET InitialAmount = " + Exp + " WHERE AccountName ='" + ToData + "'");

                    MessageDialog Confirmed2 = new MessageDialog("Transfer successful");
                    await Confirmed2.ShowAsync();
                }
                Results();
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("Please enter Amount", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is NullReferenceException)
                {
                    MessageDialog dialog = new MessageDialog("Please select an Account", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    /// 
                }
            }

        }

        public double TAccountBalance()
        {
            string AccountSelection = ((Accounts)FromAccountsSel.SelectedItem).AccountName;
            conn.CreateTable<Accounts>();
            var query12 = conn.Query<Accounts>("SELECT * FROM Accounts WHERE AccountName ='" + AccountSelection + "'");
            var sumProd = query12.AsEnumerable().Sum(o => o.InitialAmount);
            double Total = sumProd;
            return Total;
        }
        public double TransferToAccount()
        {
            // Getting data from the combobox
            string FromData = ((Accounts)FromAccountsSel.SelectedItem).AccountName;

            conn.CreateTable<Accounts>();
            var query1 = conn.Query<Accounts>("SELECT * FROM Accounts WHERE AccountName ='" + FromData + "'");
            var sumProdQty1 = query1.AsEnumerable().Sum(o => o.InitialAmount);

            //conn.CreateTable<Transactions>();
            //var query2 = conn.Query<Transactions>("SELECT * FROM Transactions WHERE Account = '" + FromData + "'");
            //var sumProdQty2 = query2.AsEnumerable().Sum(o => o.Amount);
            double Money = Convert.ToDouble(TransferMoney.Text);
            double FinalAmountShip = sumProdQty1 - Money;
            return FinalAmountShip;
        }

        public double TransferToExpense()
        {
            // Getting data from the combobox
            string ToData = ((Accounts)ToAccountSel.SelectedItem).AccountName;

            conn.CreateTable<Accounts>();
            var query1 = conn.Query<Accounts>("SELECT * FROM Accounts WHERE AccountName ='" + ToData + "'");
            var sumProdQty1 = query1.AsEnumerable().Sum(o => o.InitialAmount);
            double FinalTot = sumProdQty1;
            return FinalTot;
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int num = pagecontent.SelectedIndex;
            if(num == 0)
            {
                InternFooter.Visibility = Visibility.Collapsed;
                pageFooter.Visibility = Visibility.Visible;
            }
            else
            {
                pageFooter.Visibility = Visibility.Collapsed;
                InternFooter.Visibility = Visibility.Visible;
            }
        }
    }
}
