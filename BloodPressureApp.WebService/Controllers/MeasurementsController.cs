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
        private readonly IMeasurementRepo measurementRepo;
        public MeasurementsController(IMeasurementRepo measurementRepo)
        {            
            this.measurementRepo = measurementRepo;
        }

        // GET: api/Measurements        
        [HttpGet]
        public List<Measurement> GetMeasurements()
        {            
            return measurementRepo.GetAll().ToList();           
        }

        // POST: api/Measurements
        [HttpPost]
        public Measurement PostMeasurement(Measurement measurement)
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
            return measurement;
        }

        // DELETE: api/Measurements/5        
        [HttpDelete("{id}")]
        public bool DeleteMeasurement(int id)
        {           
            var del = measurementRepo.Remove(id);
            if (del)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
