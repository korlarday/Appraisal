using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AprraisalApplication.Services
{
    public class EmailServices
    {
        public async static Task<string> SendEmailToken(string email, string content)
        {
            // string path = context.Request.Scheme.Current.Server.MapPath("~/Views/EmailTemplate.html");
            //string file = System.IO.Path.GetFullPath("Services/EmailTemplate.html");
            //StreamReader str = new StreamReader(file);
            //string MailText = str.ReadToEnd();
            //str.Close();

            //Replace [content] with the content of the mail  
            //MailText = MailText.Replace("[Content]", content);


            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@notifications.ieianchorpensions.com", "IEI Anchor Pensions");
            var subject = "Email Verification";
            var to = new EmailAddress(email, "Name");
            var plainTextContent = "content";
            var htmlContent = content;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return response.ToString();
        }
        public static string GenerateValidationToken()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateValidationToken();
            }
            return r;
        }

    }
}