using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaRichie.Models
{
    public class Transactions
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public DateTime DateOfTran { get; set; }

        [NotNull]
        public string Account { get; set; }

        public string Description { get; set; }

        [NotNull]
        public string TranType { get; set; }

        [NotNull]
        public double Amount { get; set; }
    }
}
