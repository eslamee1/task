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
        public IActionResult Create(Survey survey, string[] SelectedMonths, int[] SelectedDays, string[] SelectedWeekDays, Dictionary<string, int> Ratings)
        {
            if (ModelState.IsValid)
            {
                // Assign the selected values to the survey object
                survey.SelectedMonths = SelectedMonths ?? new string[0];
                survey.SelectedDays = SelectedDays ?? new int[0];
                survey.SelectedWeekDays = SelectedWeekDays ?? new string[0];

                // Convert the dictionary to a list of Rating objects
                if (Ratings != null && Ratings.Any())
                {
                    survey.RatingList = Ratings.Select(r => new Rating
                    {
                        ItemName = r.Key, // Key from the dictionary (e.g. "منطقة الخدمات")
                        Score = r.Value   // Value from the dictionary (e.g. 5)
                    }).ToList();
                }

                // Save the survey to the database
                _context.Surveys.Add(survey);
                _context.SaveChanges();

                return RedirectToAction("Success"); // Redirect to success page or another action
            }

            return View(survey); // Return the same view if the model state is invalid
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
