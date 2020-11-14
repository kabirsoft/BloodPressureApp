using BloodPressureApp.Data.IRepositories;
using BloodPressureApp.Data.Models;
using BloodPressureApp.WebService.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace BloodPressureApp.UnitTest.BloodPressureApp.WebService
{
    [TestClass]
    public class MeasurementsTest
    {
        [TestMethod]
        public void TestGetMeasurements()
        {
            //arrange
            var MeasurementRepoMockClass = new Mock<IMeasurementRepo>();
            List<Measurement> getMeasurementObj = new List<Measurement>()
            {
                new Measurement{Id = 1, Systolic= 115, Diastolic= 78, Category="Normal", Date=DateTime.Now},
                new Measurement{Id = 2, Systolic= 121, Diastolic= 75, Category="Elevated", Date=DateTime.Now},
            };
            MeasurementRepoMockClass.Setup(x => x.GetAll()).Returns(getMeasurementObj);
            var measurementController = new MeasurementsController(MeasurementRepoMockClass.Object);

            //Act
            List<Measurement> result = measurementController.GetMeasurements();

            //assert
            Assert.AreEqual(result[0].Systolic, 115);
            Assert.AreEqual(result[1].Category, "Elevated");
        }
        [TestMethod]
        public void TestPostMeasurement()
        {
            //Arrange
            var MeasurementRepoMockClass = new Mock<IMeasurementRepo>();
            var measurement = new Measurement { Systolic= 131, Diastolic=85, Category= "Hypertension stage1" };
            MeasurementRepoMockClass.Setup(x => x.AddNew(measurement)).Returns(measurement);
            var measurementController = new MeasurementsController(MeasurementRepoMockClass.Object);

            //Act
            Measurement result = measurementController.PostMeasurement(measurement);

            //Assert
            Assert.AreEqual(result.Systolic, 131);
            Assert.AreEqual(result.Diastolic, 85);
            Assert.AreEqual(result.Category, "Hypertension stage1");
        }
        [TestMethod]
        public void TestDeleteMeasurement()
        {
            //Arrange
            var MeasurementRepoMockClass = new Mock<IMeasurementRepo>();
            var expected = true;
            MeasurementRepoMockClass.Setup(x => x.Remove(2)).Returns(expected);
            var measuresController = new MeasurementsController (MeasurementRepoMockClass.Object);

            //Act
            bool result = measuresController.DeleteMeasurement(2);

            //Assert
            Assert.AreEqual(result, true);

        }


    }
}
