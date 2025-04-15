using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlazeInspect.Models
{
    public class Question
    {
        //question
        public string Q_SerialNumber { get; set; }
        public int Product_ID { get; set; }
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string first_opt { get; set; }
        public string second_opt { get; set; }
        public string ResponseText { get; set; }
        public string CommentText { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public int[] Q_ResponseID { get; set; }
        public string[] Response { get; set; }
        public string[] Comment { get; set; }

        public Array checkbox { get; set; }

    }
}