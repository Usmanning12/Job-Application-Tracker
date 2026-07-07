using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

public class EmailController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly GmailService _gmailService = new();
    private readonly JobStatusService _statusService = new();
    private readonly EmailParsingService _parser = new();

    public EmailController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Import()
    {
        var emails = _gmailService.GetEmails();

        foreach (var email in emails)
        {
            var app = _parser.Parse(email);

            Console.WriteLine($"Company: {app.Company}");
            Console.WriteLine($"Position: {app.Position}");

            // FIX: use correct variable name
            Console.WriteLine($"Now: {DateTime.Now}");
            Console.WriteLine($"UTC: {DateTime.UtcNow}");
            Console.WriteLine($"Kind: {DateTime.Now.Kind}");

            app.AppliedDate = DateTime.Now;

            Console.WriteLine($"AppliedDate: {app.AppliedDate}");
            
            _context.Applications.Add(app);
        }

        _context.SaveChanges();

        return RedirectToAction("Index", "Applications");
    }
        

        
    
}