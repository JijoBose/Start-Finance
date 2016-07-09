using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    public class Transactions
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string DateOfTran { get; set; }

        [NotNull]
        public string Account { get; set; }

        public string Description { get; set; }

        [NotNull]
        public string TranType { get; set; }

        [NotNull]
        public double Amount { get; set; }
    }
}
