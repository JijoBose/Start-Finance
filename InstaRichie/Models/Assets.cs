using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLitePCL;

namespace InstaRichie.Models
{
    public class Assets
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        [Unique, NotNull]
        public string AssetName { get; set; }

        [NotNull]
        public double AssetValue { get; set; }

    }
}
