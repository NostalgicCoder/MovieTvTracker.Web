using MovieTvTracker.Web.Class;
using MovieTvTracker.Web.Data;
using MovieTvTracker.Web.Interfaces;
using TmdbApi.Lib.Models;

namespace MovieTvTracker.Web.Models
{
    public class Media : IMedia
    {
        public PaginatedList<FavoriteActor> PaginatedFavoriteActorList { get; set; }
        public PaginatedList<WatchedMedia> PaginatedWatchedMediaList { get; set; }
        public List<ResultReturn> FavoriteActorResults { get; set; }
        public List<FavoriteFilmDecade> FavoriteFilmDecadeResults { get; set; }
        public ResultReturn TMDBData { get; set; }
        public WatchedMediaResults WatchedMediaResults { get; set; }
        public Stats Stats { get; set; }
        public Int32 FavoriteActorTotalCount { get; set; }
        public Int32 SelectedTMDBId { get; set; }
        public Int32 FilmsThisYear { get; set; }
        public Int32 TvThisYear { get; set; }
        public Int32 FilmsThisMonth { get; set; }
        public Int32 FilmsLastMonth { get; set; }
        public Int32 TvThisMonth { get; set; }
        public Int32 TvLastMonth { get; set; }
        public string TopTwoDecades { get; set; }
        public string TopFiveYears { get; set; }
        public string TopFiveGenres { get; set; }
        public string YearRange { get; set; }
        public string SelectedContentType { get; set; }
        public string Keyword { get; set; }
        public bool EnglishResultOnly { get; set; }

        public Media()
        {
            FavoriteActorResults = new List<ResultReturn>();
            FavoriteFilmDecadeResults = new List<FavoriteFilmDecade>();

            FavoriteFilmDecadeResults.Add(new FavoriteFilmDecade() { DecadeName = "50s", Count = 0 });
            FavoriteFilmDecadeResults.Add(new FavoriteFilmDecade() { DecadeName = "60s", Count = 0 });
            FavoriteFilmDecadeResults.Add(new FavoriteFilmDecade() { DecadeName = "70s", Count = 0 });
            FavoriteFilmDecadeResults.Add(new FavoriteFilmDecade() { DecadeName = "80s", Count = 0 });
            FavoriteFilmDecadeResults.Add(new FavoriteFilmDecade() { DecadeName = "90s", Count = 0 });
            FavoriteFilmDecadeResults.Add(new FavoriteFilmDecade() { DecadeName = "2000s", Count = 0 });
            FavoriteFilmDecadeResults.Add(new FavoriteFilmDecade() { DecadeName = "2010s", Count = 0 });
            FavoriteFilmDecadeResults.Add(new FavoriteFilmDecade() { DecadeName = "2020s", Count = 0 });

            Stats = new Stats();
            EnglishResultOnly = true;
        }
    }
}