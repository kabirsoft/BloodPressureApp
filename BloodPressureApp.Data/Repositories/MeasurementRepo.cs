using BloodPressureApp.Data.IRepositories;
using System;
using System.Collections.Generic;
using BloodPressureApp.Data;

using System.Text;
using BloodPressureApp.Data.Models;
using System.Linq;

namespace BloodPressureApp.Data.Repositories
{
    public class MeasurementRepo : IMeasurementRepo
    {
        private readonly ApplicationDbContext db;
        public MeasurementRepo(ApplicationDbContext db)
        {
            this.db = db;            
        }
        List<Models.Measurement> IMeasurementRepo.GetAll()
        {
            return  db.Measurements.ToList();
        }
        public Models.Measurement AddNew(Models.Measurement measurement)
        {
            db.Measurements.Add(measurement);
            db.SaveChanges();
            return measurement;
        }
        public bool Remove(int id)
        {
            var measurement = db.Measurements.Find(id);
            if(measurement == null)
            {
                return false;
            }
            db.Measurements.Remove(measurement);
            db.SaveChanges();
            return true;
        }      
    }
}
