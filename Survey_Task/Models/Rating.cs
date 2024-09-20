using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Survey_Task.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public string ItemName { get; set; } // e.g., "منطقة الخدمات"
        public int Score { get; set; }       // Rating value (1 to 5)
    }
}
