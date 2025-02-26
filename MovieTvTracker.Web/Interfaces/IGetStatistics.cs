using MovieTvTracker.Web.Models;

namespace MovieTvTracker.Web.Interfaces
{
    public interface IGetStatistics
    {
        IMedia GetFilmYearsRangeAndGenres(IMedia media);
        IMedia GetTvGenres(IMedia media);
        IMedia GetQtyViewingStats(IMedia media);
        void GetTopYearsGenresDecades(IMedia media);
        void RecordFilmDecade(List<FavoriteFilmDecade> favoriteFilmDecadeColl, int year);
    }
}