using System.Threading.Tasks;

namespace ApartmanWeb.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
