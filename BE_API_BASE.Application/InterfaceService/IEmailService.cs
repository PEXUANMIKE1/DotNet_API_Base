using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE_API_BASE.Application.Handle.HandleEmail;

namespace BE_API_BASE.Application.InterfaceService
{
    public interface IEmailService
    {
        string SendEmail(EmailMessage emailMessage);
        
    }
}
