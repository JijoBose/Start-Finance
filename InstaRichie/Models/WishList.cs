using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace InstaRichie.Models
{
    public class WishList
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        [Unique]
        public string WishName { get; set; }

        [NotNull]
        public double Money { get; set; }

    }
}
