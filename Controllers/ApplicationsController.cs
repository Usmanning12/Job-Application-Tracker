using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ: /Applications
        public async Task<IActionResult> Index(string searchString, ApplicationStatus? statusFilter)
        {
            var applications = _context.Applications
                .Where(a => !string.IsNullOrEmpty(a.Company)
                            && !string.IsNullOrEmpty(a.Position))
                .AsQueryable();

            // Search
            if (!string.IsNullOrEmpty(searchString))
            {
                applications = applications.Where(a => a.Company.Contains(searchString));
            }

            // Filter
            if (statusFilter.HasValue)
            {
                applications = applications.Where(a => a.Status == statusFilter);
            }

            // Dashboard stats
            ViewBag.Total = await applications.CountAsync();
            ViewBag.Applied = await applications.CountAsync(a => a.Status == ApplicationStatus.Applied);
            ViewBag.Interviewing = await applications.CountAsync(a => a.Status == ApplicationStatus.Interviewing);
            ViewBag.Offers = await applications.CountAsync(a => a.Status == ApplicationStatus.Offer);
            ViewBag.Rejected = await applications.CountAsync(a => a.Status == ApplicationStatus.Rejected);
            return View(await applications.ToListAsync());
        }

        // DETAILS
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var application = await _context.Applications
                .FirstOrDefaultAsync(m => m.Id == id);

            if (application == null) return NotFound();

            return View(application);
        }

        // CREATE (GET)
        public IActionResult Create()
        {
            return View();
        }

        // CREATE (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Application application)
        {
            if (string.IsNullOrEmpty(application.Company) ||
                string.IsNullOrEmpty(application.Position))
            {
                ModelState.AddModelError("", "Company and Position are required.");
                return View(application);
            }

            if (ModelState.IsValid)
            {
                application.AppliedDate = DateTime.UtcNow;
                application.Status = ApplicationStatus.Applied;

                _context.Add(application);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(application);
        }

        // EDIT (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var application = await _context.Applications.FindAsync(id);

            if (application == null) return NotFound();

            return View(application);
        }

        // EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Application application)
        {
            if (!ModelState.IsValid)
                return View(application);

            var existing = await _context.Applications.FindAsync(application.Id);

            if (existing == null)
                return NotFound();

            existing.Company = application.Company;
            existing.Position = application.Position;
            existing.Status = application.Status;
            existing.AppliedDate = application.AppliedDate;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // DELETE (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var application = await _context.Applications
                .FirstOrDefaultAsync(m => m.Id == id);

            if (application == null) return NotFound();

            return View(application);
        }

        // DELETE (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application != null)
            {
                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // DASHBOARD (PIE CHART)
        public async Task<IActionResult> Dashboard()
        {
            var apps = await _context.Applications
                .Where(a => !string.IsNullOrEmpty(a.Company)
                            && !string.IsNullOrEmpty(a.Position))
                .ToListAsync();
            
            var chartData = apps
                .Select(a => a.Status.ToString().Trim())
                .GroupBy(s => s)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count()
                }).OrderBy(a => a.Status.ToString())
                .ToList();

            ViewBag.ChartLabels = chartData.Select(x => x.Status).ToList();
            ViewBag.ChartValues = chartData.Select(x => x.Count).ToList();
            
            Console.WriteLine($"Total Applications: {apps.Count}");

            foreach (var app in apps)
            {
                Console.WriteLine($"{app.Id} | {app.Company} | {app.Position} | {app.Status}");
            }

            return View(chartData);
        }
       

        public async Task<IActionResult> CleanStatuses()
            {
                var apps = await _context.Applications.ToListAsync();

                foreach (var app in apps)
                {
                    if (app.Status.ToString().Trim().Equals("Applied", StringComparison.OrdinalIgnoreCase))
                    {
                        app.Status = ApplicationStatus.Applied;
                    }

                    if (app.Status.ToString().Trim().Equals("Interviewing", StringComparison.OrdinalIgnoreCase))
                    {
                        app.Status = ApplicationStatus.Interviewing;
                    }

                    if (app.Status.ToString().Trim().Equals("Offer", StringComparison.OrdinalIgnoreCase))
                    {
                        app.Status = ApplicationStatus.Offer;
                    }

                    if (app.Status.ToString().Trim().Equals("Rejected", StringComparison.OrdinalIgnoreCase))
                    {
                        app.Status = ApplicationStatus.Rejected;
                    }
                }

                await _context.SaveChangesAsync();

                return Content("Database cleaned successfully");
            }
            
        public async Task<IActionResult> FixStatuses()
        {
            var apps = await _context.Applications.ToListAsync();

            foreach (var app in apps)
            {
                app.Status = app.Status switch
                {
                    ApplicationStatus.Applied => ApplicationStatus.Applied,
                    ApplicationStatus.Interviewing => ApplicationStatus.Interviewing,
                    ApplicationStatus.Offer => ApplicationStatus.Offer,
                    ApplicationStatus.Rejected => ApplicationStatus.Rejected,
                    _ => ApplicationStatus.Applied
                };
            }

            await _context.SaveChangesAsync();

            return Content("Statuses fixed");
        }
        public async Task<IActionResult> NormalizeStatuses()
        {
            var apps = await _context.Applications.ToListAsync();

            foreach (var a in apps)
            {
                var clean = a.Status.ToString().Trim().ToLower();

                a.Status = clean switch
                {
                    "applied" => ApplicationStatus.Applied,
                    "interviewing" => ApplicationStatus.Interviewing,
                    "offer" => ApplicationStatus.Offer,
                    "rejected" => ApplicationStatus.Rejected,
                    _ => ApplicationStatus.Applied
                };
            }

            await _context.SaveChangesAsync();

            return Content("Statuses normalized");
        }
    }
}            