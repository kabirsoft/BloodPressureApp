using BloodPressureApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodPressureApp.Data.IRepositories
{
    public interface IMeasurementRepo
    {
        List<Measurement> GetAll();
        Measurement AddNew(Measurement measurement);
        bool Remove(int id);
    }
}
