using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using sinavproje.Models;


namespace sinavproje.models
{
    public class modelmarkakategori
    {
        public dbmarka markatables { get; set; }
        public dbkategori kategoritables { get; set; }
        public dbbirim birimtables { get; set; }
        public dbmusteri musteritables { get; set; }
        public ilceler ilcelertables { get; set; }
        public iller illertables { get; set; }
        public dburunler uruntables { get; set; }
        public dbsepet sepettables { get; set; }
        public dbSatislar satistables { get; set; }
        public login logintables { get; set; }
    }
}