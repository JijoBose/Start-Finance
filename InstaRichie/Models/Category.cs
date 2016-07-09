using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace StartFinance.Models
{
    public class Category
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        [NotNull, Unique]
        public string CategoryName { get; set; }

        [NotNull]
        public string Mode { get; set; }
    }
}
