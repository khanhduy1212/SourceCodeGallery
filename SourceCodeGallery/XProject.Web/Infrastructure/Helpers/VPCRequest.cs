using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace XProject.Web.Infrastructure.Helpers
{
    public class VPCRequest
    {
        Uri _address;
        SortedList<String, String> _requestFields = new SortedList<String, String>(new VPCStringComparer());
        String _rawResponse;
        SortedList<String, String> _responseFields = new SortedList<String, String>(new VPCStringComparer());
        String _secureSecret;


        public VPCRequest(String URL)
        {
            _address = new Uri(URL);
        }

        public void SetSecureSecret(String secret)
        {
            _secureSecret = secret;
        }

        public void AddDigitalOrderField(String key, String value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                _requestFields.Add(key, value);
            }
        }

        public String GetResultField(String key, String defValue)
        {
            String value;
            if (_responseFields.TryGetValue(key, out value))
            {
                return value;
            }
            else
            {
                return defValue;
            }
        }

        public String GetResultField(String key)
        {
            return GetResultField(key, "");
        }

        private String GetRequestRaw()
        {
            StringBuilder data = new StringBuilder();
            foreach (KeyValuePair<string, string> kvp in _requestFields)
            {
                if (!String.IsNullOrEmpty(kvp.Value))
                {
                    data.Append(kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value) + "&");
                }
            }
            //remove trailing & from string
            if (data.Length > 0)
                data.Remove(data.Length - 1, 1);
            return data.ToString();
        }

        public string GetTxnResponseCode()
        {
            return GetResultField("vpc_TxnResponseCode");
        }

        //_____________________________________________________________________________________________________
        // Three-Party order transaction processing

        public String Create3PartyQueryString()
        {
            StringBuilder url = new StringBuilder();
            //Payment Server URL
            url.Append(_address);
            url.Append("?");
            //Create URL Encoded request string from request fields 
            url.Append(GetRequestRaw());
            //Hash the request fields
            url.Append("&vpc_SecureHash=");
            url.Append(CreateSHA256Signature(true));
            return url.ToString();
        }

        public string Process3PartyResponse(System.Collections.Specialized.NameValueCollection nameValueCollection)
        {
            foreach (string item in nameValueCollection)
            {
                if (!item.Equals("vpc_SecureHash") && !item.Equals("vpc_SecureHashType"))
                {
                    _responseFields.Add(item, nameValueCollection[item]);
                }

            }

            if (!nameValueCollection["vpc_TxnResponseCode"].Equals("0") && !String.IsNullOrEmpty(nameValueCollection["vpc_Message"]))
            {
                if (!String.IsNullOrEmpty(nameValueCollection["vpc_SecureHash"]))
                {
                    if (!CreateSHA256Signature(false).Equals(nameValueCollection["vpc_SecureHash"]))
                    {
                        return "INVALIDATED";
                    }
                    return "CORRECTED";
                }
                return "CORRECTED";
            }

            if (String.IsNullOrEmpty(nameValueCollection["vpc_SecureHash"]))
            {
                return "INVALIDATED";//no sercurehash response
            }
            if (!CreateSHA256Signature(false).Equals(nameValueCollection["vpc_SecureHash"]))
            {
                return "INVALIDATED";
            }
            return "CORRECTED";
        }

        //_____________________________________________________________________________________________________

        private class VPCStringComparer : IComparer<String>
        {
            /*
             <summary>Customised Compare Class</summary>
             <remarks>
             <para>
             The Virtual Payment Client need to use an Ordinal comparison to Sort on 
             the field names to create the SHA256 Signature for validation of the message. 
             This class provides a Compare method that is used to allow the sorted list 
             to be ordered using an Ordinal comparison.
             </para>
             </remarks>
             */

            public int Compare(String a, String b)
            {
                /*
                 <summary>Compare method using Ordinal comparison</summary>
                 <param name="a">The first string in the comparison.</param>
                 <param name="b">The second string in the comparison.</param>
                 <returns>An int containing the result of the comparison.</returns>
                 */

                // Return if we are comparing the same object or one of the 
                // objects is null, since we don't need to go any further.
                if (a == b) return 0;
                if (a == null) return -1;
                if (b == null) return 1;

                // Ensure we have string to compare
                string sa = a as string;
                string sb = b as string;

                // Get the CompareInfo object to use for comparing
                System.Globalization.CompareInfo myComparer = System.Globalization.CompareInfo.GetCompareInfo("en-US");
                if (sa != null && sb != null)
                {
                    // Compare using an Ordinal Comparison.
                    return myComparer.Compare(sa, sb, System.Globalization.CompareOptions.Ordinal);
                }
                throw new ArgumentException("a and b should be strings.");
            }
        }

        //______________________________________________________________________________
        // SHA256 Hash Code

        public string CreateSHA256Signature(bool useRequest)
        {
            // Hex Decode the Secure Secret for use in using the HMACSHA256 hasher
            // hex decoding eliminates this source of error as it is independent of the character encoding
            // hex decoding is precise in converting to a byte array and is the preferred form for representing binary values as hex strings. 
            byte[] convertedHash = new byte[_secureSecret.Length / 2];
            for (int i = 0; i < _secureSecret.Length / 2; i++)
            {
                convertedHash[i] = (byte)Int32.Parse(_secureSecret.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
            }

            // Build string from collection in preperation to be hashed
            StringBuilder sb = new StringBuilder();
            SortedList<String, String> list = (useRequest ? _requestFields : _responseFields);
            foreach (KeyValuePair<string, string> kvp in list)
            {
                if (kvp.Key.StartsWith("vpc_") || kvp.Key.StartsWith("user_"))
                    sb.Append(kvp.Key + "=" + kvp.Value + "&");
            }
            // remove trailing & from string
            if (sb.Length > 0)
                sb.Remove(sb.Length - 1, 1);

            // Create secureHash on string
            string hexHash = "";
            using (HMACSHA256 hasher = new HMACSHA256(convertedHash))
            {
                byte[] hashValue = hasher.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
                foreach (byte b in hashValue)
                {
                    hexHash += b.ToString("X2");
                }
            }
            return hexHash;
        }

        public static string PaymentOnePay(string vpc_Version,string vpc_Command, string vpc_Merchant
            , string vpc_AccessCode
            , string vpc_MerchTxnRef
            , string vpc_OrderInfo
            , string vpc_Amount
            , string vpc_ReturnURL, string vpc_TicketNo
            , string vpc_SHIP_Street01=null
            , string vpc_SHIP_Provice = null
            , string vpc_SHIP_City = null
            , string vpc_SHIP_Country = null
            , string vpc_Customer_Phone = null
            , string vpc_Customer_Email = null
            , string vpc_Customer_Id = null

        )
        {
            string SECURE_SECRET = ConfigurationManager.AppSettings["SECURE_SECRET"];//"6D0870CDE5F24F34F3915FB0045120DB"
            // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
            VPCRequest conn = new VPCRequest(ConfigurationManager.AppSettings["UrlOnePay"]);
            conn.SetSecureSecret(SECURE_SECRET);
            // Add the Digital Order Fields for the functionality you wish to use
            // Core Transaction Fields
            conn.AddDigitalOrderField("AgainLink", "http://OnePay.vn");
            conn.AddDigitalOrderField("Title", "OnePay paygate");
            conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
            conn.AddDigitalOrderField("vpc_Version", vpc_Version);
            conn.AddDigitalOrderField("vpc_Command", vpc_Command);
            conn.AddDigitalOrderField("vpc_Merchant", vpc_Merchant);
            conn.AddDigitalOrderField("vpc_AccessCode", vpc_AccessCode);
            conn.AddDigitalOrderField("vpc_MerchTxnRef", vpc_MerchTxnRef);
            conn.AddDigitalOrderField("vpc_OrderInfo", vpc_OrderInfo);
            conn.AddDigitalOrderField("vpc_Amount", vpc_Amount);
            conn.AddDigitalOrderField("vpc_ReturnURL", vpc_ReturnURL);
            // Thong tin them ve khach hang. De trong neu khong co thong tin
            conn.AddDigitalOrderField("vpc_SHIP_Street01", vpc_SHIP_Street01);
            conn.AddDigitalOrderField("vpc_SHIP_Provice", vpc_SHIP_Provice);
            conn.AddDigitalOrderField("vpc_SHIP_City", vpc_SHIP_City);
            conn.AddDigitalOrderField("vpc_SHIP_Country", vpc_SHIP_Country);
            conn.AddDigitalOrderField("vpc_Customer_Phone", vpc_Customer_Phone);
            conn.AddDigitalOrderField("vpc_Customer_Email", vpc_Customer_Email);
            conn.AddDigitalOrderField("vpc_Customer_Id", vpc_Customer_Id);
            // Dia chi IP cua khach hang
            conn.AddDigitalOrderField("vpc_TicketNo", vpc_TicketNo);
            // Chuyen huong trinh duyet sang cong thanh toan
            String url = conn.Create3PartyQueryString();
            return url;
        }
    }
}