using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_API_BASE.Doman.Entities
{
    public class ConfirmEmail : BaseEntity
    {
        public string ConfirmCode { get; set; }
        public long UserId { get; set; }
        public virtual User? User { get; set; }
        public DateTime ExpiryTime { get; set; }
        public bool IsConfirmed { get; set; } = false;
    }
}
