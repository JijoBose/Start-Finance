﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace JustWallet.Models
{
    public class Debt
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }

        [NotNull]
        public string DebtName { get; set; }

        [NotNull]
        public double DebtAmount { get; set; }

    }
}
