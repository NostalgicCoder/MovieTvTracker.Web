using MovieTvTracker.Web.Models;

namespace MovieTvTracker.Web.Interfaces
{
    public interface IGetStatistics
    {
        IMedia GetFilmYearsRangeAndGenres(IMedia media);
        IMedia GetTvYearsRangeAndGenres(IMedia media);
        IMedia GetQtyViewingStats(IMedia media);
        void GetFilmTopYearsGenresDecades(IMedia media);
        void GetTvTopYearsGenresDecades(IMedia media);
        void RecordMediaDecade(List<FavoriteDecade> favoriteMediaDecadeResults, int year);
    }
}