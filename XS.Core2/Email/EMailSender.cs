using System.Net.Mail;
using System.Text;

namespace XS.Core2.Email
{

    /// <summary>
    /// Email发送
    /// </summary>
    public class EMailSender
    {

        /// <summary>
        /// 发送一份Email
        /// </summary>
        /// <param name="sTo">发送目录，也就是发送给谁，如cqs@ebsite.net.</param>
        /// <param name="sTitle">邮件标题.</param>
        /// <param name="sBody">文件的内容.</param>
        /// <param name="sSmtpServer">The s SMTP server.</param>
        /// <param name="sFrom">发送人邮箱地址，会显示在对方.</param>
        /// <param name="sFromPass">SMTP server对应的密码.</param>
        /// <param name="Port">SMTP server 对应的端口.</param>
        /// <param name="MailEncoding">邮件的加密方式.</param>
        /// <param name="IsBodyHtml">是否支持html格式的邮件内容</param>
        /// <param name="EnableSsl">是否使用SSL</param>
        /// <param name="Priority">邮件级别</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool Send(string sTo,string sTitle,string sBody,string sSmtpServer, string sFrom,string sFromPass,int Port, Encoding MailEncoding , bool IsBodyHtml = true, bool EnableSsl = true,MailPriority Priority = System.Net.Mail.MailPriority.Normal)
        {
            if(Equals(MailEncoding,null))
                MailEncoding = System.Text.Encoding.UTF8;
            // = System.Text.Encoding.UTF8
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            mailMessage.To.Add(new System.Net.Mail.MailAddress(sTo)); //收件人地址
            mailMessage.From = new System.Net.Mail.MailAddress(sFrom); //发件人地址
            mailMessage.Subject = sTitle;
            mailMessage.Body = sBody;
            mailMessage.IsBodyHtml = IsBodyHtml;
            mailMessage.BodyEncoding = MailEncoding;//System.Text.Encoding.UTF8;
            mailMessage.Priority = Priority;//System.Net.Mail.MailPriority.Normal;          
            //smtpClient.EnableSsl = true;
            if (Port > 0)
                smtpClient.Port = Port;
            //smtpClient = new SmtpClient();
            smtpClient.Credentials = new System.Net.NetworkCredential
            (mailMessage.From.Address, sFromPass);//设置发件人身份的票据 
            smtpClient.EnableSsl = EnableSsl;
            smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtpClient.Host = sSmtpServer;//"smtp." + mailMessage.From.Host;
            smtpClient.Send(mailMessage);
            return true;
        }
    }
}