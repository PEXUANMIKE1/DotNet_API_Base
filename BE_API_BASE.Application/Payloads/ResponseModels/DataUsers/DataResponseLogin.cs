using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_API_BASE.Application.Payloads.ResponseModels.DataUsers
{
    public class DataResponseLogin
    {
        public string AccessToken { get; set; }
        public string RefeshToken { get; set; }
    }
}
