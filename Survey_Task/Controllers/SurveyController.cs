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


        ////if you want to force the user to enter all the data 
        //[HttpPost]
        //public IActionResult Create(Survey survey, string[] SelectedMonths, int[] SelectedDays, string[] SelectedWeekDays, Dictionary<string, int> Ratings)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Assign the selected values to the survey object
        //        survey.SelectedMonths = SelectedMonths ?? new string[0];
        //        survey.SelectedDays = SelectedDays ?? new int[0];
        //        survey.SelectedWeekDays = SelectedWeekDays ?? new string[0];

        //        // Convert the dictionary to a list of Rating objects
        //        if (Ratings != null && Ratings.Any())
        //        {
        //            survey.RatingList = Ratings.Select(r => new Rating
        //            {
        //                ItemName = r.Key, // Key from the dictionary (e.g. "منطقة الخدمات")
        //                Score = r.Value   // Value from the dictionary (e.g. 5)
        //            }).ToList();
        //        }

        //        // Save the survey to the database
        //        _context.Surveys.Add(survey);
        //        _context.SaveChanges();

        //        return RedirectToAction("Success"); // Redirect to success page or another action
        //    }

        //    return View(survey); // Return the same view if the model state is invalid
        //}



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

        // GET: /Survey/Scan
        public IActionResult Scan()
        {
            return View(); 
        }

        [HttpPost]
        public JsonResult ProcessScan([FromBody] QRCodeDataModel model)
        {
            if (model != null && !string.IsNullOrEmpty(model.QRCodeData))
            {
                // Log the scanned QR code data (for example, writing it to a log file)
                Console.WriteLine("QR Code Data: " + model.QRCodeData);

                // Save the scanned QR code data to a database
                // Assuming you have a DbContext set up for your database
                using (var context = new ApplicationDbContext())
                {
                    var qrRecord = new ScannedQRCode
                    {
                        QRCodeData = model.QRCodeData,
                        ScannedAt = DateTime.Now // Optionally, add a timestamp
                    };

                    context.ScannedQRCodes.Add(qrRecord);
                    context.SaveChanges();
                }

                // Return success message
                return Json(new { success = true, message = "QR code processed and saved successfully" });
            }

            return Json(new { success = false, message = "Invalid QR code data" });
        }



    }
}
