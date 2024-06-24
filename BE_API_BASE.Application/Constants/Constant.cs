using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_API_BASE.Application.Constants
{
    public class Constant
    {
        public class AppSettingKeys 
        {
            public const string DEFAULT_CONNECTION = "DefaultConnection";
        }
        public class DefaultValue
        {
            public const string DEFAULT_CONTROLLER_ROUTER = "api/[controller]/[action]";
        }
    }
}
