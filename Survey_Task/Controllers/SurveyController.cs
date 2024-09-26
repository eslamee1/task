using Microsoft.AspNetCore.Mvc;
using Survey_Task.Models;
using System.Collections.Generic;
using System.Linq;

namespace Survey_Task.Controllers
{
    public class SurveyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SurveyController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Survey survey, string SelectedMonth, int SelectedDay, string SelectedWeekDay, Dictionary<string, int> Ratings)
        {
            // Bind the selected month, day, and weekday
            survey.SelectedMonth = SelectedMonth; // Bind the selected month from the form
            survey.SelectedDay = SelectedDay;     // Bind the selected day from the form
            survey.SelectedWeekDay = SelectedWeekDay; // Bind the selected weekday from the form
            if (!string.IsNullOrEmpty(SelectedMonth) && SelectedDay > 0)
            {
                var selectedMonth = Enum.Parse<Survey_Task.Models.Month>(SelectedMonth);
                int currentYear = DateTime.Now.Year;

                // Handle potential invalid date combinations
                try
                {

                    survey.DateAndTime = new DateTime(currentYear, (int)selectedMonth + 1, SelectedDay, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                    survey.SelectedWeekDay = SelectedWeekDay.ToString(); // Store the selected weekday
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    ModelState.AddModelError("", "Invalid date selected.");
                    return View(survey);
                }
            }
            else
            {
                // Default to DateTime.Now if no selection is made
                survey.DateAndTime = DateTime.Now;
            }

            // Convert Ratings dictionary to Rating objects and add them to the survey
            if (Ratings != null && Ratings.Any())
            {
                survey.RatingList = Ratings.Select(r => new Rating
                {
                    ItemName = r.Key,   // The item name (e.g., "منطقة الخدمات")
                    Score = r.Value     // The rating score (e.g., 5)
                }).ToList();
            }

            // Save the survey data to the database
            if (ModelState.IsValid)
            {
                _context.Surveys.Add(survey);
                _context.SaveChanges();
                return RedirectToAction("Success"); // Redirect to success page or action
            }

            // If validation fails, return the view with the survey data to show errors
            return View(survey);
        }

        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }

    }
}
