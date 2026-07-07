namespace WebApplication1.Services;

public class JobStatusService
{
    public string GetStatus(string message)
    {
        if (message.Contains("interview"))
            return "Interview";

        if (message.Contains("unfortunately"))
            return "Rejected";

        if (message.Contains("offer"))
            return "Offer";
        return "Pending"; 
    }
}