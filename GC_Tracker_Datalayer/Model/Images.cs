using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GC_Tracker_Datalayer.Model
{
    public class Images
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("producentcode")]
        public string ProducentCode { get; set; }
        [Column("img")]
        public byte[] Img { get; set; }
        [Column("imgsmall")]
        public byte[] Imgsmall { get; set; }
        [Column("productid")]
        public int ProductId { get; set; } 
    }
}
