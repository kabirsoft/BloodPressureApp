using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BloodPressureApp.Data.Models
{
    public class Measurement
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Systolic is required")]
        public int Systolic { get; set; }

        [Required(ErrorMessage = "Diastolic is required")]
        public int Diastolic { get; set; }
        public string Category { get; set; }      
        public string Suggestion { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }

    public class Category
    {
        public static string Normal = "Normal";
        public static string Elevated = "Elevated";
        public static string HypertensionStage1 = "Hypertension stage1";
        public static string HypertensionStage2 = "Hypertension stage2";
        public static string HighpertensiveCrisis = "Stage emergency";
    }
    public class Suggestion
    {
        public static string Elevated = "Life style need to change: diet control, decrease salt intake, weight control, regular exercise";
        public static string HighpertensiveCrisis = "Consult your doctor immediately";
    }
}
