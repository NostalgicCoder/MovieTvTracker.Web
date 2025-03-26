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
        public List<FavoriteDecade> FavoriteMediaDecadeResults { get; set; }
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
        public string FilmTopTwoDecades { get; set; }
        public string FilmTopFiveYears { get; set; }
        public string FilmTopFiveGenres { get; set; }
        public string TvTopTwoDecades { get; set; }
        public string TvTopFiveYears { get; set; }
        public string TvTopFiveGenres { get; set; }
        public string FilmYearRange { get; set; }
        public string TvYearRange { get; set; }
        public string SelectedContentType { get; set; }
        public string SearchKeyword { get; set; }
        public string SearchYear { get; set; }
        public bool EnglishResultOnly { get; set; }

        public Media()
        {
            FavoriteActorResults = new List<ResultReturn>();
            FavoriteMediaDecadeResults = new List<FavoriteDecade>();

            FavoriteMediaDecadeResults.Add(new FavoriteDecade() { DecadeName = "50s", Count = 0 });
            FavoriteMediaDecadeResults.Add(new FavoriteDecade() { DecadeName = "60s", Count = 0 });
            FavoriteMediaDecadeResults.Add(new FavoriteDecade() { DecadeName = "70s", Count = 0 });
            FavoriteMediaDecadeResults.Add(new FavoriteDecade() { DecadeName = "80s", Count = 0 });
            FavoriteMediaDecadeResults.Add(new FavoriteDecade() { DecadeName = "90s", Count = 0 });
            FavoriteMediaDecadeResults.Add(new FavoriteDecade() { DecadeName = "2000s", Count = 0 });
            FavoriteMediaDecadeResults.Add(new FavoriteDecade() { DecadeName = "2010s", Count = 0 });
            FavoriteMediaDecadeResults.Add(new FavoriteDecade() { DecadeName = "2020s", Count = 0 });

            Stats = new Stats();
            EnglishResultOnly = true;
        }
    }
}