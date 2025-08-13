using QLSPNhom2.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLSPNhom2.DTO
{
    public class ProductDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public DateTime ManuDate { get; set; }//Ngày sản xuất
        public int Quantity { get; set; }
        public string Avatar { get; set; }
        public long IdCategory { get; set; }
    }
}
