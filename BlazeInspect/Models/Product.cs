using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlazeInspect.Models
{
    public class Product
    {
        public int Extinguisher_ID { get; set; }
        public string SerialNumber { get; set; }
        public string inspector_name { get; set; }
        public int Product_name { get; set; }
        public string Location_ { get; set; }
        public string Type { get; set; }
        public string Capacity { get; set; }
        public string Manufacturer { get; set; }
        public string Date_of_Manufacture { get; set; }
        public string Last_Inspection_Date { get; set; }
        public string Next_Inspection_Date { get; set; }
        public string Maintenance_Schedule { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Date { get; set; }
        public string time { get; set; }
        public Array checkbox { get; set; }

        //index
        public int totalproduct { get; set; }
        public int totalinspector { get; set; }
        public int pendinginspection { get; set; }

        public int red { get; set; }
        public int green { get; set; }
        public int orange { get; set; }

        public int todayinspection { get; set; }
        public int yesterdayinspection { get; set; }
        public int weeklyinspection { get; set; }
        public int monthlyinspection { get; set; }
        //index


    }
}