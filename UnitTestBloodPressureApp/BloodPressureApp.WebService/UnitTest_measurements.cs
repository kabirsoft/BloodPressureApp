using BloodPressureApp.Data.IRepositories;
using BloodPressureApp.Data.Models;
using BloodPressureApp.WebService.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestBloodPressureApp.BloodPressureApp.WebService
{
    [TestClass]
    public class UnitTest_measurements
    {
        public void TestGetMeasurements()
        {
            //arrange
            var MeasurementRepoMockClass = new Mock<IMeasurementRepo>();
            List<Measurement> getMeasurementObj = new List<Measurement>()
            {
                new Measurement{Id = 1, Systolic= 75, Diastolic= 110, Category="Normal", Date=DateTime.Now},
                new Measurement{Id = 2, Systolic= 121, Diastolic= 75, Category="Elevated", Date=DateTime.Now},
            };
            MeasurementRepoMockClass.Setup(x => x.GetAll()).Returns(getMeasurementObj);
            var measurementController = new MeasurementsController(MeasurementRepoMockClass.Object);

            //Act
            List<Measurement> result = measurementController.GetMeasurements();

            //assert
            Assert.AreEqual(result[0].Systolic, 75);
            Assert.AreEqual(result[1].Category, "Elevated");
        }
    }
}
