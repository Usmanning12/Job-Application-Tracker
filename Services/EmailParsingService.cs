using WebApplication1.Models;

public class EmailParsingService
{
    public Application Parse(string email)
    {
        var application = new Application();

        if(email.Contains("Google"))
            application.Company = "Google";

        if(email.Contains("Microsoft"))
            application.Company = "Microsoft";

        if(email.Contains("Software Engineer"))
            application.Position =
                "Software Engineer";

        if(email.Contains("Developer"))
            application.Position =
                "Developer";

        return application;
    }
}