using System.Configuration;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace huhu.Commom
{
    public class SendEmail
    {
        private static string EmailUserName = ConfigurationManager.AppSettings["EmailUserName"].ToString();
        private static string EmailAuthCode = ConfigurationManager.AppSettings["EmailAuthCode"].ToString();

        /// <summary>
        /// 读取邮件模板及替换指定内容
        /// </summary>
        /// <param name="templateName">邮件模板名</param>
        /// <param name="title">标题</param>
        /// <param name="authCode">验证码</param>
        /// <param name="expires">过期时间</param>
        /// <returns>模板</returns>
        public static string Template(string templateName, string title, string authCode, int expires)
        {
            string emailTemplate = HttpContext.Current.Server.MapPath("~/Models/" + templateName), cache = string.Empty;
            if (FileUtil.FileExists(emailTemplate))
            {
                StreamReader sr = new StreamReader(emailTemplate, Encoding.UTF8);
                cache = sr.ReadToEnd();
                //替换模板指定内容
                cache = cache.Replace("<strong>{0}</strong>", title);
                cache = cache.Replace("<strong>{1}</strong>", authCode);
                cache = cache.Replace("<strong>{2}</strong>", expires.ToString());
                //释放资源
                sr.Close();
            }
            return cache;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="MailTo">收信人地址</param>
        /// <param name="Subject">标题</param>
        /// <param name="Body">信件内容</param>
        public static void SendMails(string MailTo, string Subject, string Body)
        {
            SmtpClient mailclient = new SmtpClient();
            //服务器端口
            mailclient.Port = 587;
            //发送方式
            mailclient.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp服务器
            mailclient.Host = "smtp.qq.com";
            //用户名凭证
            mailclient.Credentials = new System.Net.NetworkCredential(EmailUserName, EmailAuthCode);
            //邮件信息
            MailMessage message = new MailMessage();
            //发件人
            message.From = new MailAddress(EmailUserName);
            //收件人
            message.To.Add(new MailAddress(MailTo, MailTo.ToString(), Encoding.UTF8));
            //邮件标题编码
            message.SubjectEncoding = Encoding.UTF8;
            //主题
            message.Subject = Subject;
            //内容
            message.Body = Body;
            //正文编码
            message.BodyEncoding = Encoding.UTF8;
            //设置为HTML格式
            message.IsBodyHtml = true;
            //优先级
            message.Priority = MailPriority.High;
            //异步发送
            mailclient.SendAsync(message, message.To);
        }
    
    }
}
