using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BitCoin_Advisor.Business
{
    public class BLExchangeRate
    {

        public async Task<decimal> GetExchangeRate(string from, string To)
        {
            try
            {
                var address = String.Format("http://api.fixer.io/latest?base={0}&symbols={1}", from, To);

                var byteUrl = await ParseExchange(address);
                string regEx = string.Format("\"{0}\":([0-9]*\\.*[0-9]*)", To);
                Regex regex = new Regex(regEx);
                string result = System.Text.Encoding.UTF8.GetString(byteUrl, 0, byteUrl.Count() - 1);
                var matched = regex.Match(result);
                decimal UsdBrl = Convert.ToDecimal(matched.Groups[1].ToString(), new CultureInfo("en-US"));

                decimal plusIof = System.Math.Round(UsdBrl * 1.0038m, 2, MidpointRounding.AwayFromZero);

                return plusIof;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<byte[]> ParseExchange(string uri)
        {
            try
            {
                var httpClient = new HttpClient(); // Error CS0246: The type or namespace name `HttpClient' could not be found. Are you missing an assembly reference? (CS0246)
                return await httpClient.GetByteArrayAsync(uri);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
