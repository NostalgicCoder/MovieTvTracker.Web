using System.Text.RegularExpressions;

namespace MovieTvTracker.Web.Class
{
    public class PerformValidation
    {
        /// <summary>
        /// Check if 'searchYear' corresponds to a valid year value for a movie/tv result
        /// </summary>
        /// <param name="searchYear"></param>
        /// <returns></returns>
        public bool ValidateSearchYear(string searchYear)
        {
            if(searchYear == null)
            {
                return false;
            }

            if (Regex.IsMatch(searchYear, @"^[1-2][09][0-9][0-9]$") && searchYear != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}