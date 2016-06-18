using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;

namespace InstaRichie.Models
{
    public class Debt
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public DateTime DateofDebt { get; set; }

        [Unique]
        public string DebtName { get; set; }

        [NotNull]
        public double DebtAmount { get; set; }

    }
}
