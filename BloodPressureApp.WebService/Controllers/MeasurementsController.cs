using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BloodPressureApp.Data;
using BloodPressureApp.Data.Models;
using BloodPressureApp.Data.IRepositories;

namespace BloodPressureApp.WebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeasurementsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMeasurementRepo measurementRepo;

        public MeasurementsController(ApplicationDbContext context, IMeasurementRepo measurementRepo)
        {
            _context = context;
            this.measurementRepo = measurementRepo;
        }

        // GET: api/Measurements
        [HttpGet]
        public ActionResult<IEnumerable<Measurement>> GetMeasurements()
        {            
            return measurementRepo.GetAll().ToList();           
        }

        // GET: api/Measurements/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Measurement>> GetMeasurement(int id)
        {
            var measurement = await _context.Measurements.FindAsync(id);
            if (measurement == null)
            {
                return NotFound();
            }

            return measurement;
        }       

        // POST: api/Measurements
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<Measurement> PostMeasurement(Measurement measurement)
        {
            if (measurement.Systolic > 180 || measurement.Diastolic > 120)
            {
                measurement.Category = Category.HighpertensiveCrisis;
                measurement.Suggestion = Suggestion.HighpertensiveCrisis;
            }
            else if (measurement.Systolic >= 140 || measurement.Diastolic >= 90)
            {
                measurement.Category = Category.HypertensionStage2;
            }
            else if ((measurement.Systolic >= 130 && measurement.Systolic <= 139) || (measurement.Diastolic >= 80 && measurement.Diastolic <= 89))
            {
                measurement.Category = Category.HypertensionStage1;
            }
            else if ((measurement.Systolic >= 120 && measurement.Systolic <= 129) && (measurement.Diastolic < 80))
            {
                measurement.Category = Category.Elevated;
                measurement.Suggestion = Suggestion.Elevated;
            }
            else if (measurement.Systolic < 120 && measurement.Diastolic < 80)
            {
                measurement.Category = Category.Normal;
            }
            measurementRepo.AddNew(measurement);
            return CreatedAtAction("GetMeasurement", new { id = measurement.Id }, measurement);
        }

        // DELETE: api/Measurements/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Measurement>> DeleteMeasurement(int id)
        {
            var measurement = await _context.Measurements.FindAsync(id);
            if (measurement == null)
            {
                return NotFound();
            }

            //_context.Measurements.Remove(measurement);
            //await _context.SaveChangesAsync();

            measurementRepo.Remove(id);
            return measurement;
        }

        private bool MeasurementExists(int id)
        {
            return _context.Measurements.Any(e => e.Id == id);
        }
    }
}
