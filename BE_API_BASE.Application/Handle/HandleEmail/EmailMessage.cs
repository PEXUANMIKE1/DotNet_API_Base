using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_API_BASE.Application.Handle.HandleEmail
{
    public class EmailMessage
    {
        //bảng này chứa thông tin nội dung email (To: người được gửi, Subject: tiêu đề, Content:(body)Nội dung)
        public List<MailboxAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailMessage() { }
        public EmailMessage(IEnumerable<string> to, string subject, string content) 
        {
            To = new List<MailboxAddress>();
            To.AddRange(to.Select(x=> new MailboxAddress("email", x)));
            Subject = subject;
            Content = content;
        }

    }
}
