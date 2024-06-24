using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_API_BASE.Doman.Entities
{
    public class RefeshToken : BaseEntity
    {
        public string Token { get; set; }
        public long UserId { get; set; }
        public virtual User? User { get; set; }
        public DateTime ExpiryTime { get; set; }
    }
}
