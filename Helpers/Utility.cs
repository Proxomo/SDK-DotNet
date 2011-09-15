using System;
using System.Web;
using System.Text;

namespace Proxomo
{

    internal class Utility
    {

        internal static double GenerateTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToUInt64(ts.TotalSeconds);
        }

        internal static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        internal static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        internal static string FormatQueryString(string address = "", string latitude = "", string longitude = "", string q = "", string category = "", double radius = 25, LocationSearchScope scope = LocationSearchScope.All, int maxResults = 25, string personID = "")
        {
            StringBuilder sbuilder = new StringBuilder();

            sbuilder.Append("?");

            if (!(string.IsNullOrWhiteSpace(address)))
            {
                sbuilder.AppendFormat("&address={0}", HttpUtility.UrlEncode(address));
            }

            if (!(string.IsNullOrWhiteSpace(latitude)))
            {
                sbuilder.AppendFormat("&latitude={0}", HttpUtility.UrlEncode(latitude));
            }

            if (!(string.IsNullOrWhiteSpace(longitude)))
            {
                sbuilder.AppendFormat("&longitude={0}", HttpUtility.UrlEncode(longitude));
            }

            if (!(string.IsNullOrWhiteSpace(q)))
            {
                sbuilder.AppendFormat("&q={0}", HttpUtility.UrlEncode(q));
            }

            if (!(string.IsNullOrWhiteSpace(category)))
            {
                sbuilder.AppendFormat("&category={0}", HttpUtility.UrlEncode(category));
            }

            if (radius > 0)
            {
                sbuilder.AppendFormat("&radius={0}", Convert.ToInt16(radius));
            }

            if (scope > 0)
            {
                sbuilder.AppendFormat("&scope={0}", Convert.ToInt16(scope));
            }

            if (maxResults > 0)
            {
                sbuilder.AppendFormat("&maxresults={0}", Convert.ToInt16(maxResults));
            }

            if (!(string.IsNullOrWhiteSpace(personID)))
            {
                sbuilder.AppendFormat("&personid={0}", HttpUtility.UrlEncode(personID));
            }

            if (sbuilder.Length > 1)
            {
                return sbuilder.ToString().Replace("?&", "?");
            }
            else
            {
                sbuilder = null;
                return string.Empty;
            }

        }

    }

}
