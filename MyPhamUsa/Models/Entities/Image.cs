using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyPhamUsa.Models.Entities
{
    public class Image : BaseEntity
    {
        public string Path { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
