using Microsoft.AspNetCore.Mvc;
using Survey_Task.Models;
using System.Collections.Generic;
using System.Linq;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

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
        [ValidateAntiForgeryToken]
        public IActionResult Create(Survey survey, string SelectedMonth, int SelectedDay, DayOfWeek SelectedWeekDay, Dictionary<string, int> Ratings)
        {
            if (ModelState.IsValid)
            {
                // Parse the selected month
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

                // Process the ratings as before
                if (Ratings != null && Ratings.Any())
                {
                    survey.RatingList = Ratings.Select(r => new Rating
                    {
                        ItemName = r.Key,
                        Score = r.Value
                    }).ToList();
                }
                // Generate a unique URL for the survey
                survey.QRCodeUrl = Url.Action("Details", "Survey", new { id = survey.Id }, Request.Scheme); // Adjust as needed

                // Save the survey to the database
                _context.Surveys.Add(survey);
                _context.SaveChanges();

                // Generate QR code
                using (var qrGenerator = new QRCodeGenerator())
                {
                    var qrCodeData = qrGenerator.CreateQrCode($"https://yourdomain.com/Survey/Details/{survey.Id}", QRCodeGenerator.ECCLevel.Q);
                    using (var qrCode = new QRCoder.QRCode(qrCodeData)) // Ensure this is correct
                    {
                        using (var bitmap = qrCode.GetGraphic(20))
                        {
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", $"{survey.Id}.png");
                            bitmap.Save(filePath, ImageFormat.Png); // Save the QR code image
                        }
                    }
                }

                return RedirectToAction("Success", new { surveyId = survey.Id });
            }

            return View(survey);
        }


        [HttpGet]
        public IActionResult Success(int surveyId) // Accept surveyId as a parameter
        {
            var survey = _context.Surveys.Find(surveyId); // Get the survey from the database
            if (survey == null)
            {
                return NotFound(); // Handle case where survey doesn't exist
            }
            return View(survey); // Pass the survey object to the view
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
