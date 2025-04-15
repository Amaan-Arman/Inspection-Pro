using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlazeInspect.Models
{
    public class Chat
    {
        //question
        public int user_id { get; set; }
        public int sender_user_id { get; set; }
        public int receiver_user_id { get; set; }
        public string user_name { get; set; }
        public string user_img { get; set; }
        public string messege_title { get; set; }
        public string messege_text { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string day { get; set; }
        public string isread { get; set; }
        public string login_type { get; set; }
        public int Chat_id { get; set; }

        public string attachmentread { get; set; }
        public HttpPostedFileBase[] attachment { get; set; }

    }
}