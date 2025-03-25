using MovieTvTracker.Web.Class;
using MovieTvTracker.Web.Data;
using MovieTvTracker.Web.Models;
using TmdbApi.Lib.Models;

namespace MovieTvTracker.Web.Interfaces
{
    public interface IMedia
    {
        PaginatedList<FavoriteActor> PaginatedFavoriteActorList { get; set; }
        PaginatedList<WatchedMedia> PaginatedWatchedMediaList { get; set; }
        List<ResultReturn> FavoriteActorResults { get; set; }
        List<FavoriteDecade> FavoriteMediaDecadeResults { get; set; }
        ResultReturn TMDBData { get; set; }
        WatchedMediaResults WatchedMediaResults { get; set; }
        Stats Stats { get; set; }
        Int32 FavoriteActorTotalCount { get; set; }
        Int32 SelectedTMDBId { get; set; }
        Int32 FilmsThisYear { get; set; }
        Int32 TvThisYear { get; set; }
        Int32 FilmsThisMonth { get; set; }
        Int32 FilmsLastMonth { get; set; }
        Int32 TvThisMonth { get; set; }
        Int32 TvLastMonth { get; set; }
        string FilmTopTwoDecades { get; set; }
        string FilmTopFiveYears { get; set; }
        string FilmTopFiveGenres { get; set; }
        string TvTopTwoDecades { get; set; }
        string TvTopFiveYears { get; set; }
        string TvTopFiveGenres { get; set; }
        string FilmYearRange { get; set; }
        string TvYearRange { get; set; }
        string SelectedContentType { get; set; }
        string Keyword { get; set; }
        bool EnglishResultOnly { get; set; }
    }
}