using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GC_Tracker_Logic.Models
{
    public class GpuDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ProducentCode { get; set; }
        public string ImageAddress { get; set; }
        public string StoreName { get; set; }
        public byte[] Image { get; set; }
    }
}
