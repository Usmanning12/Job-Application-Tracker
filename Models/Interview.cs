namespace WebApplication1.Models;

public class Interview
{
    public int InterviewId { get; set; }
    public int ApplicationId { get; set; }
    public int InterviewDate {get; set;}
    public string? InterviewType { get; set; }
    public string? Notes { get; set; }
    public string? Status { get; set; }
}