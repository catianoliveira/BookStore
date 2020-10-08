namespace BookStore.Helpers
{
    public interface IMailHelper
    {
        void SendMail(string to, string subject, string body);


        void SendMailWithPDF(string to, string subject, string body, byte[] pdf);

    }
}
