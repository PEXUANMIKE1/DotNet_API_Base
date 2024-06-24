using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE_API_BASE.Application.Handle.HandleEmail
{
    public class EmailConfiguration
    {
        //bảng này chứa thông tin phía gửi email
        public string From { get; set; } = string.Empty;// email của người gửi,
        public string SmtpServer { get; set; } = string.Empty;//  Địa chỉ máy chủ SMTP,
        public int Port { get; set; }//  Cổng kết nối đến máy chủ SMTP,
        public string Username { get; set; }// Tên đăng nhập cho máy chủ SMTP.,
        public string Password { get; set; } //  Mật khẩu cho máy chủ SMTP
    }
}
