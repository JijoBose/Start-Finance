// **************************************************************************
//Start Finance - An to manage your personal finances.
//Copyright(C) 2016  Jijo Bose

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
// ***************************************************************************

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
using StartFinance.ViewModels;
using SQLite.Net;
using StartFinance.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashBoardPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public DashBoardPage()
        {
            this.InitializeComponent();
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            Results();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
        }

        public void Results()
        {
            Calculations nnn = new Calculations();
            AccountTotal.Text = "Accounts: " + nnn.AccountTotal().ToString();
            Assets.Text = "Assets: " + nnn.AssetCalculation().ToString();
            CreditRatio.Text = "Credit Rating: " + nnn.CreditRatio().ToString();
            Debts.Text = "Debts: " + nnn.DebtCalculation().ToString();
            FullTotal.Text = "Total : " + nnn.FullValuation().ToString();
            DebtChart.Percentage = nnn.PercentageScore();
            CenterValue.Text= ""+nnn.PercentageScore().ToString("0.00") +"%";
            MonthlyData.Text = "Monthly : " + nnn.MonthlyStatus().ToString();
            RatioReportTxt.Text = nnn.RatioReport();

            conn.CreateTable<Assets>();
            var query1 = conn.Table<Assets>();
            Assetme.ItemsSource = query1.ToList();
        }

    }
}