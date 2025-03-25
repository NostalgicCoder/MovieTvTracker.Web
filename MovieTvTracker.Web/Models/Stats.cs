namespace MovieTvTracker.Web.Models
{
    public class Stats
    {
        public List<ContentYear> FilmYears { get; set; }
        public List<ContentYear> TvYears { get; set; }
        public List<ContentGenre> FilmGenres { get; set; }
        public List<ContentGenre> TvGenres { get; set; }

        public Stats()
        {
            FilmYears = new List<ContentYear>();
            FilmGenres = new List<ContentGenre>();
            TvYears = new List<ContentYear>();
            TvGenres = new List<ContentGenre>();
        }
    }
}