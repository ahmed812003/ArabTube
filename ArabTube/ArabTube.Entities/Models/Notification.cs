using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Entities.Models
{
    public class Notification
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Message { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string SenderId { get; set; }
        public string VideoId { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
