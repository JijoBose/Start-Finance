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
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

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
            Calculations nnn = new Calculations();
            AccountTotal.Text = "Accounts: " + nnn.AccountTotal().ToString();
            Assets.Text = nnn.AssetCalculation().ToString();
            Debts.Text = nnn.CreditRatio().ToString();
            string dmo = nnn.DebtCalculation().ToString();
            FullTotal.Text = nnn.FullValuation().ToString();
            string dmdm = nnn.MonthlyStatus().ToString();
            string fgf = nnn.PercentageScore().ToString();

            this.Loaded += DashBoardPage_Loaded;
        }

        private void DashBoardPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadGraphContents();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Calculations nnn = new Calculations();
            AccountTotal.Text = "Accounts: " + nnn.AccountTotal().ToString();
            Assets.Text = nnn.AssetCalculation().ToString();
            Debts.Text = nnn.CreditRatio().ToString();
            string dmo = nnn.DebtCalculation().ToString();
            FullTotal.Text = nnn.FullValuation().ToString();
            string dmdm = nnn.MonthlyStatus().ToString();
            string fgf = nnn.PercentageScore().ToString();
        }

        private void LoadGraphContents()
        {
            Calculations nomnom = new Calculations();
            double Monthly = nomnom.MonthlyStatus();
            List<FinancialStuff> financialstuffList = new List<FinancialStuff>();
            financialstuffList.Add(new FinancialStuff() { Year = "2015", Amount = 2500 });
            financialstuffList.Add(new FinancialStuff() { Year = "2016", Amount = Monthly });
            (PieChart.Series[0] as ColumnSeries).ItemsSource = financialstuffList;
        }
    }
}

public class FinancialStuff
{
    public string Year { get; set; }
    public double Amount { get; set; }
}
