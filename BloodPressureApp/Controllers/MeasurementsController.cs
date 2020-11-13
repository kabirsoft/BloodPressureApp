using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using BloodPressureApp.Data.Models;
using BloodPressureApp.Data;
using BloodPressureApp.Data.IRepositories;

namespace BloodPressureApp.Controllers
{
    public class MeasurementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMeasurementRepo measurementRepo;
        public MeasurementsController(ApplicationDbContext context, IMeasurementRepo _measurementRepo)
        {
            _context = context;
            measurementRepo = _measurementRepo;
        }

        // GET: Measurements
        public IActionResult Index(string sortby)
        {
            var measurements = measurementRepo.GetAll();
            ViewBag.OrderByDate = string.IsNullOrEmpty(sortby)? "date_asc" : "";
            switch (sortby)
            {
                case "date_asc":
                   measurements = measurements.OrderBy(x=>x.Date).ToList();
                    break;
                default:
                    measurements = measurements.OrderByDescending(x => x.Date).ToList();
                  break;
            }           
            return  View(measurements);            
        }

        // GET: Measurements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var measurement = await _context.Measurements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (measurement == null)
            {
                return NotFound();
            }
            return View(measurement);
        }

        // GET: Measurements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Measurements/Create        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Systolic,Diastolic")] Measurement measurement)
        {
            if (ModelState.IsValid)
            {               
                
                if(measurement.Systolic > 180 || measurement.Diastolic > 120)
                {
                    measurement.Category = Category.HighpertensiveCrisis;
                    measurement.Suggestion = Suggestion.HighpertensiveCrisis;
                }
                else if(measurement.Systolic >=140 || measurement.Diastolic >= 90)
                {
                    measurement.Category = Category.HypertensionStage2;
                }
                else if ((measurement.Systolic >= 130 && measurement.Systolic <=139) || ( measurement.Diastolic >=80  && measurement.Diastolic <= 89))
                {
                    measurement.Category = Category.HypertensionStage1;
                }
                else if((measurement.Systolic >= 120 && measurement.Systolic <=129) && (measurement.Diastolic < 80))
                {
                    measurement.Category = Category.Elevated;
                    measurement.Suggestion = Suggestion.Elevated;
                }
                else if( measurement.Systolic <120  && measurement.Diastolic < 80)
                {
                    measurement.Category = Category.Normal;
                }

                measurementRepo.AddNew(measurement);
                return RedirectToAction(nameof(Index));
            }
            return View(measurement);
        }

        // GET: Measurements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var measurement = await _context.Measurements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (measurement == null)
            {
                return NotFound();
            }

            return View(measurement);
        }

        // POST: Measurements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {            
            measurementRepo.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        private bool MeasurementExists(int id)
        {
            return _context.Measurements.Any(e => e.Id == id);
        }
    }
}
