using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace XProject.Web.Infrastructure.Utility
{
    public class TrueMoney
    {
        public static string sha256Generate(string data, string secretKey)
        {
            String rs = "";
            byte[] keyByte = Encoding.UTF8.GetBytes(secretKey);
            byte[] messageBytes = Encoding.UTF8.GetBytes(data);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                for (int i = 0; i < hashmessage.Length; i++)
                {
                    byte bit = hashmessage[i];
                    rs += bit.ToString("x2");
                }
            }
            return rs;
        }
        public static String generateSignature_Visa_1TransactionRequest(string access_key, string amount, string order_id, string order_info, string secret_key)
        {
            String result = "";
            String signature = "access_key=$access_key&amount=$amount&order_id=$order_id&order_info=$order_info";
            signature = signature.Replace("$access_key", access_key);
            signature = signature.Replace("$amount", amount);
            signature = signature.Replace("$order_id", order_id);
            signature = signature.Replace("$order_info", order_info);
            result = sha256Generate(signature, secret_key);
            return result;
        }
      }
}