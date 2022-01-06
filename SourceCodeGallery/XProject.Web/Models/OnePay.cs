using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XProject.Web.Models
{
    public class OnePay
    {
        public string vpc_Version{get;set;}
        public string vpc_Command{get;set;}
        public string vpc_Merchant {get;set;}
        public string vpc_AccessCode {get;set;}
        public string vpc_MerchTxnRef{get;set;}
        public string vpc_OrderInfo{get;set;}
        public string vpc_Amount {get;set;}
        public string vpc_ReturnURL{get;set;}
        public string vpc_TicketNo{get;set;}
        public string vpc_SHIP_Street01 {get;set;}
        public string vpc_SHIP_Provice {get;set;}
        public string vpc_SHIP_City {get;set;}
        public string vpc_SHIP_Country {get;set;}
        public string vpc_Customer_Phone {get;set;}
        public string vpc_Customer_Email {get;set;}
        public string vpc_Customer_Id { get; set; }
        public string message { get; set; }
        public int statusTran { get; set; }


    }
}