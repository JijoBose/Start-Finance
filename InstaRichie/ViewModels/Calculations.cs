// **************************************************************************
//Start Finance - An to manage your personal finances.
//Copyright(C) 2016  Jijo Bose

//Start Finance is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//Start Finance is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with Start Finance.If not, see<http://www.gnu.org/licenses/>.
// ***************************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using StartFinance.Models;
using System.IO;

namespace StartFinance.ViewModels
{
    public class Calculations
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public double DebtCalculation() // Getting Total Debt
        {
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            conn.CreateTable<Debt>();

            //// getting values of debt
            var SumQuery1 = conn.Query<Debt>("SELECT * FROM Debt");
            var sumProdQty1 = SumQuery1.AsEnumerable().Sum(o => o.DebtAmount);
            double SUMOF = sumProdQty1;
            return SUMOF;
        }

        public double FullValuation()
        {
            double Temp1 = AssetCalculation() + AccountTotal();
            return Temp1;
        }

        public double MonthlyStatus()
        {
            double Expense = ExpenseReport();
            double Income = IncomeReport();
            double Result = (Expense) + Income;
            return Result;
        }

        public double ExpenseReport()
        {
            try
            {
                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;
                int DaysinMonth = DateTime.DaysInMonth(currentYear, currentMonth);
                conn.CreateTable<Transactions>();
                var Expense = conn.Query<Transactions>("SELECT * FROM Transactions WHERE TranType = 'Expense' AND DateOfTran BETWEEN '" + currentMonth + "/01/" + currentYear + "' AND '" + currentMonth + "/" + DaysinMonth + "/" + currentYear + "'");
                var InEXP = Expense.AsEnumerable().Sum(o => o.Amount);
                double TempE = InEXP;
                double SUM = InEXP;
                return SUM;
            }
            catch (NullReferenceException)
            {
                double Temp = 0;
                return Temp;
            }
        }

        public double IncomeReport()
        {
            try
            {
                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;
                int DaysinMonth = DateTime.DaysInMonth(currentYear, currentMonth);
                conn.CreateTable<Transactions>();
                var Expense = conn.Query<Transactions>("SELECT * FROM Transactions WHERE TranType = 'Income' AND DateOfTran BETWEEN '" + currentMonth + "/01/" + currentYear + "' AND '" + currentMonth + "/" + DaysinMonth + "/" + currentYear + "'");
                var InEXP = Expense.AsEnumerable().Sum(o => o.Amount);
                double TempE = InEXP;
                double SUM = InEXP;
                return SUM;
            }
            catch (NullReferenceException)
            {
                double Temp = 0;
                return Temp;
            }
        }

        public double AssetCalculation()
        {
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            conn.CreateTable<Assets>();

            double _Tot;
            var SumQuery = conn.Query<Assets>("SELECT * FROM Assets");
            var sumProdQty = SumQuery.AsEnumerable().Sum(o => o.AssetValue);
            _Tot = sumProdQty;
            return _Tot;
        }

        public double AccountTotal()
        {
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            conn.CreateTable<Accounts>();

            //// getting values
            double Toto;
            var SumQuery = conn.Query<Accounts>("SELECT * FROM Accounts");
            var sumProdQty = SumQuery.AsEnumerable().Sum(o => o.InitialAmount);
            Toto = sumProdQty;
            return Toto;
        }

        public double PercentageScore()
        {
            try
            {
                double TotalAssetincome = FullValuation();
                double percentage = (-DebtCalculation() / (TotalAssetincome) * 100);
                if((double.IsInfinity(percentage)))
                {
                    double Temp = 100;
                    return Temp;
                }
                else if(double.IsNaN(percentage))
                {
                    double Temp = 0;
                    return Temp;
                }
                else
                {
                    return percentage;
                }

            }
            catch (DivideByZeroException)
            {
                double Temp = 100;
                return Temp;
            }

        }

        public string RatioReport()
        {
            string output;
            string input = CreditRatio();
            if(input == "AAA+")
            {
                output = "Extremely strong capacity to meet its financial commitments";
                return output;
            }
            else if(input == "AAA")
            {
                output = "Strong capacity to meet its financial commitments";
                return output;
            }
            else if(input == "AA")
            {
                output = "'AA' has High capacity with very low credit risk, but susceptibility to long-term risks appears somewhat greater";
                return output;
            }
            else if(input == "A")
            {
                output = "'A' has strong capacity to meet its financial commitments but is somewhat more susceptible to the adverse effects of changes in circumstances and economic conditions than obligors in higher-rated categories";
                return output;
            }
            else if(input == "BBB")
            {
                output = "'BBB' has adequate capacity to meet its financial commitments. However, adverse economic conditions or changing circumstances are more likely to lead to a weakened capacity of the obligor to meet its financial commitments";
                return output;
            }
            else if(input == "BB")
            {
                output = "'BB' is less vulnerable in the near term than other lower-rated obligors. However, it faces major ongoing uncertainties and exposure to adverse business, financial, or economic conditions, which could lead to the obligor's inadequate capacity to meet its financial commitments";
                return output;
            }
            else if(input == "B")
            {
                output = "'B' is more vulnerable than the obligors rated 'BB', but the obligor currently has the capacity to meet its financial commitments. Adverse business, financial, or economic conditions will likely impair the obligor's capacity or willingness to meet its financial commitments";
                return output;
            }
            else if(input == "CCC")
            {
                output = "'CCC' is currently vulnerable, and is dependent upon favorable business, financial, and economic conditions to meet its financial commitments";
                return output;
            }
            else if(input == "CC")
            {
                output = "'CC' is currently highly vulnerable";
                return output;
            }
            else if(input == "C")
            {
                output = "highly vulnerable, perhaps in bankruptcy or in arrears but still continuing to pay out on obligations";
                return output;
            }
            else
            {
                output = "has defaulted on obligations and believes that it will generally default on most or all obligations";
                return output;
            }
        }

        public string CreditRatio()
        {
            string CreditRating;
            double Percentage = PercentageScore();

            if (Percentage >= 100)
            {
                CreditRating = "D";
                return CreditRating;
            }
            else
            {
                if (Percentage > 90 || Percentage >= 100)
                {
                    CreditRating = "C";
                    return CreditRating;
                }
                else
                {
                    if (Percentage > 80 || Percentage >= 90)
                    {
                        CreditRating = "CC";
                        return CreditRating;
                    }
                    else
                    {
                        if (Percentage > 70 || Percentage >= 80)
                        {
                            CreditRating = "CCC";
                            return CreditRating;
                        }
                        else
                        {
                            if (Percentage > 60 || Percentage >= 70)
                            {
                                CreditRating = "B";
                                return CreditRating;
                            }
                            else
                            {
                                if (Percentage > 50 || Percentage >= 60)
                                {
                                    CreditRating = "BB";
                                    return CreditRating;
                                }
                                else
                                {
                                    if (Percentage > 40 || Percentage >= 50)
                                    {
                                        CreditRating = "BBB";
                                        return CreditRating;
                                    }
                                    else
                                    {
                                        if (Percentage > 30 || Percentage >= 40)
                                        {
                                            CreditRating = "A";
                                            return CreditRating;
                                        }
                                        else
                                        {
                                            if (Percentage > 20 || Percentage >= 30)
                                            {
                                                CreditRating = "AA";
                                                return CreditRating;
                                            }
                                            else
                                            {
                                                if (Percentage >= 10 || Percentage >= 20)
                                                {
                                                    CreditRating = "AAA";
                                                    return CreditRating;
                                                }
                                                else
                                                {
                                                    CreditRating = "AAA+";
                                                    return CreditRating;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

        }

    }
}
