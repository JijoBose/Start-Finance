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
using InstaRichie.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace InstaRichie.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashBoardPage : Page
    {

        public DashBoardPage()
        {
            this.InitializeComponent();
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
            FullTotal.Text = "Total Value: " + nnn.FullValuation().ToString();
            myDebtFlow.Value = nnn.PercentageScore();
            MonthlyData.Text = "Monthly : " + nnn.MonthlyStatus().ToString();
            RatioReportTxt.Text = nnn.RatioReport();
        }

    }
}