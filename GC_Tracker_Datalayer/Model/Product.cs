using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_Tracker_Datalayer.Model
{
    public class Product
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("price")]
        public decimal Price { get; set; }
        [Column("producentcode")]
        public string ProducentCode { get; set; }
        [Column("imageaddress")]
        public string ImageAddress { get; set; }
        [Column("storename")]
        public string? StoreName { get; set; }
    }
}
