namespace CaravanApi.Models
{
    public class EmailModel
    {
        public string MailReciever { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public EmailModel(string mailreciever, string subject, string content)
        {
            MailReciever = mailreciever;
            Subject = subject;
            Content = content;
        }
    }
}
