using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_API_BASE.Application.Payloads.ResponseModels
{
    public class DataResponseBase
    {
        public long Id { get; set; }
    }
}
