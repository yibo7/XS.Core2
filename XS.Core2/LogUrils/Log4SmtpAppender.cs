using log4net.Appender;
using System.Text;

namespace XS.Core2
{

    /// <summary>
    /// 为log4日志发送邮件实现的一个SmtpAppender
    /// </summary>
    public class Log4SmtpAppender: SmtpAppender
    {
        public bool EnableSsl { get; set; }
        public bool IsBodyHtml { get; set; }
        protected override void SendEmail(string messageBody)
        {
            
            Encoding MailEncoding = System.Text.Encoding.UTF8;
            EMailSender.Send(To, Subject, messageBody, SmtpHost,Username, Password, Port, MailEncoding, IsBodyHtml, EnableSsl);
        }
    }
}