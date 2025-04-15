using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlazeInspect.Models
{
    public class login
    {
        //question
        public int user_credential_id { get; set; }
        public string user_name { get; set; }
        public string user_img { get; set; }
        public string email { get; set; }
        public string user_mobileNo { get; set; }
        public string login_id { get; set; }
        public int login_type_id { get; set; }
        public string login_type { get; set; }
        public string password { get; set; }
        public string NewPassword { get; set; }
        
        public HttpPostedFileBase emp_pic { get; set; }
        public Array checkbox { get; set; }
        public string date { get; set; }
        public string Set_inspection_date { get; set; }

        public int inspector_id { get; set; }
        public string inspector_name { get; set; }

        //Rights
        public int module_id { get; set; }
        public string module_name { get; set; }
        public int can_read { get; set; }
        public int can_create { get; set; }
        public int can_delete { get; set; }
        public int can_update { get; set; }
        public int can_print { get; set; }
        public int can_report { get; set; }


    }
}