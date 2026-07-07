namespace WebApplication1.Models;

public class EmailNotifications
{
    public int EmailId { get; set; }
    public int ApplicationId { get; set; }
    public string? Sender { get; set; }
    public string? Subject { get; set; }
    public int ReceivedDate { get; set; }
    public string? ExtractedStatus { get; set; }
}