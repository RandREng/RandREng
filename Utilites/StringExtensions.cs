using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace RandREng.Utility
{
    public static class StringExtensions
    {
        /// <summary>
        /// Trims string after checking for null so no exception is thrown
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string SafeTrim(this string theString)
        {
            return string.IsNullOrWhiteSpace(theString) ? "" : theString.Trim();
        }

        /// <summary>
        /// Trims string to max length after checking for null so no exception is thrown
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="MaxLength"></param>
        /// <returns></returns>
        public static string SafeTrimMax(this string theString, int MaxLength)
        {
            theString.SafeTrim();
            return theString.Substring(0, Math.Min(MaxLength, theString.Length));
        }

        /// <summary>
        /// Returns true if string is NOT null or empty / whitespace
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static bool IsValid(this string theString)
        {
            return false == string.IsNullOrWhiteSpace(theString);
        }

        /// <summary>
        /// Returns true if string is either Null or Whitespace        /// 
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string theString)
        {
            return string.IsNullOrWhiteSpace(theString);
        }

        /// <summary>
        /// Removes duplicate space characters from a string
        /// </summary>
        /// <param name="theString"></param>
        /// <returns></returns>
        public static string RemoveExtraSpaces(this string theString)
        {
            char[] delimiter = { ' ' };

            string[] split = theString.Split(delimiter, 100);

            string newString = split.Where(s => s.Length > 0).Aggregate("", (current, s) => current + s.Trim() + " ");

            return newString.Trim();

        }

        /// <summary>
        /// Return true if a string does not contain the argument string
        /// </summary>
        /// <param name="theString"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public static bool DoesNotContain(this string theString, string searchString)
        {
            return false == theString.Contains(searchString);
        }

        public static bool ContainsNumbers(this string theString)
        {
            return Regex.IsMatch(theString, @"\d");
        }

    }

}
