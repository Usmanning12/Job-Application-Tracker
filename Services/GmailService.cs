namespace WebApplication1.Services;

public class GmailService
{
    public List<string> GetEmails()
    {
        return new List<string>()
        {
            "We would like to schedule an interview",
            "Unfortunately we moved forward with another candidate",
            "Application received",
            "We would like to offer you"
        };
    }
}

