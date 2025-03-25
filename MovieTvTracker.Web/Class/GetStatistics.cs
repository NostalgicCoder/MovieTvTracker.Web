using MovieTvTracker.Web.Interfaces;
using MovieTvTracker.Web.Models;
using TmdbApi.Lib.Models;

namespace MovieTvTracker.Web.Class
{
    public class GetStatistics : IGetStatistics
    {
        /// <summary>
        /// Loop through all watched media film results, aquire data on the year, year range, decade, genre for consumption by the view at a later stage
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IMedia GetFilmYearsRangeAndGenres(IMedia media)
        {
            try
            {
                foreach (WatchedMediaItem item in media.WatchedMediaResults.WatchedFilms)
                {
                    DateTime releaseYear = Convert.ToDateTime(item.ResultReturn.FilmIdResult.release_date);

                    RecordMediaDecade(media.FavoriteMediaDecadeResults, releaseYear.Year);

                    if (media.Stats.FilmYears.Any(x => x.Year == releaseYear.Year))
                    {
                        media.Stats.FilmYears.Where(x => x.Year == releaseYear.Year).Select(x => x.Qty = x.Qty + 1).ToList();
                    }
                    else
                    {
                        media.Stats.FilmYears.Add(new ContentYear() { Year = releaseYear.Year, Qty = 1 });
                    }

                    foreach (Genre genre in item.ResultReturn.FilmIdResult.genres)
                    {
                        if (media.Stats.FilmGenres.Any(x => x.Genre == genre.name))
                        {
                            media.Stats.FilmGenres.Where(x => x.Genre == genre.name).Select(x => x.Qty = x.Qty + 1).ToList();
                        }
                        else
                        {
                            media.Stats.FilmGenres.Add(new ContentGenre() { Genre = genre.name, Qty = 1 });
                        }
                    }
                }

                media.Stats.FilmYears = media.Stats.FilmYears.OrderBy(x => x.Year).ToList();
                media.Stats.FilmGenres = media.Stats.FilmGenres.OrderByDescending(x => x.Qty).ToList();
                media.FilmYearRange = string.Format("{0} > {1}", media.Stats.FilmYears.First().Year.ToString(), media.Stats.FilmYears.Last().Year.ToString());

                GetFilmTopYearsGenresDecades(media);
            }
            catch (Exception ex)
            {
                throw new Exception("GetFilmYearsRangeAndGenres", ex);
            }

            return media;
        }

        /// <summary>
        /// Loop through all watched media TV results, aquire data on the year, year range, decade, genre for consumption by the view at a later stage
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IMedia GetTvYearsRangeAndGenres(IMedia media)
        {
            try
            {
                foreach (WatchedMediaItem item in media.WatchedMediaResults.WatchedTV)
                {
                    DateTime releaseYear = Convert.ToDateTime(item.ResultReturn.TvIdResult.first_air_date);

                    RecordMediaDecade(media.FavoriteMediaDecadeResults, releaseYear.Year);

                    if (media.Stats.TvYears.Any(x => x.Year == releaseYear.Year))
                    {
                        media.Stats.TvYears.Where(x => x.Year == releaseYear.Year).Select(x => x.Qty = x.Qty + 1).ToList();
                    }
                    else
                    {
                        media.Stats.TvYears.Add(new ContentYear() { Year = releaseYear.Year, Qty = 1 });
                    }

                    foreach (Genre genre in item.ResultReturn.TvIdResult.genres)
                    {
                        if (media.Stats.TvGenres.Any(x => x.Genre == genre.name))
                        {
                            media.Stats.TvGenres.Where(x => x.Genre == genre.name).Select(x => x.Qty = x.Qty + 1).ToList();
                        }
                        else
                        {
                            media.Stats.TvGenres.Add(new ContentGenre() { Genre = genre.name, Qty = 1 });
                        }
                    }
                }

                media.Stats.TvYears = media.Stats.TvYears.OrderBy(x => x.Year).ToList();
                media.Stats.TvGenres = media.Stats.TvGenres.OrderByDescending(x => x.Qty).ToList();
                media.TvYearRange = string.Format("{0} > {1}", media.Stats.TvYears.First().Year.ToString(), media.Stats.TvYears.Last().Year.ToString());

                GetTvTopYearsGenresDecades(media);
            }
            catch(Exception ex)
            {
                throw new Exception("GetTvYearsRangeAndGenres", ex);
            }

            return media;
        }

        /// <summary>
        /// Perform queries on the watched media data to aquire stats (figures) on number of films, tv watched for specific periods of time.  Save data back to the 'media' object.
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        public IMedia GetQtyViewingStats(IMedia media)
        {
            media.FilmsLastMonth = 0;
            media.TvLastMonth = 0;

            if (DateTime.Now.Month == 1)
            {
                media.FilmsLastMonth = media.WatchedMediaResults.WatchedFilms.Where(item => item.WatchedMedia.LastWatched.Month == 12 && item.WatchedMedia.LastWatched.Year == (DateTime.Now.Year - 1)).Count();
            }
            else
            {
                media.FilmsLastMonth = media.WatchedMediaResults.WatchedFilms.Where(item => item.WatchedMedia.LastWatched.Month == (DateTime.Now.Month - 1)).Count();
            }

            if (DateTime.Now.Month == 1)
            {
                media.TvLastMonth = media.WatchedMediaResults.WatchedTV.Where(item => item.WatchedMedia.LastWatched.Month == 12 && item.WatchedMedia.LastWatched.Year == (DateTime.Now.Year - 1)).Count();
            }
            else
            {
                media.TvLastMonth = media.WatchedMediaResults.WatchedTV.Where(item => item.WatchedMedia.LastWatched.Month == (DateTime.Now.Month - 1)).Count();
            }

            media.FilmsThisYear = media.WatchedMediaResults.WatchedFilms.Where(item => item.WatchedMedia.LastWatched.Year == DateTime.Now.Year).Count();
            media.TvThisYear = media.WatchedMediaResults.WatchedTV.Where(item => item.WatchedMedia.LastWatched.Year == DateTime.Now.Year).Count();
            media.FilmsThisMonth = media.WatchedMediaResults.WatchedFilms.Where(item => item.WatchedMedia.LastWatched.Month == DateTime.Now.Month).Count();
            media.TvThisMonth = media.WatchedMediaResults.WatchedTV.Where(item => item.WatchedMedia.LastWatched.Month == DateTime.Now.Month).Count();

            return media;
        }

        /// <summary>
        /// Turn the collected film result data into string representations for display within the relevant view, filter out genres not wanted for display due to being very common.
        /// </summary>
        /// <param name="media"></param>
        public void GetFilmTopYearsGenresDecades(IMedia media)
        {
            media.FilmTopFiveYears = string.Join(", ", media.Stats.FilmYears.OrderByDescending(x => x.Qty).Take(5).Select(x => x.Year));

            media.FilmTopFiveGenres = string.Join(", ", media.Stats.FilmGenres.Where(x => x.Genre != "Drama" && x.Genre != "Thriller").OrderByDescending(x => x.Qty).Take(5).Select(x => x.Genre));

            media.FilmTopTwoDecades = string.Join(", ", media.FavoriteMediaDecadeResults.OrderByDescending(x => x.Count).Take(2).Select(x => x.DecadeName));
        }

        /// <summary>
        /// Turn the collected TV result data into string representations for display within the relevant view, filter out genres not wanted for display due to being very common.
        /// </summary>
        /// <param name="media"></param>
        public void GetTvTopYearsGenresDecades(IMedia media)
        {
            media.TvTopFiveYears = string.Join(", ", media.Stats.TvYears.OrderByDescending(x => x.Qty).Take(5).Select(x => x.Year));

            media.TvTopFiveGenres = string.Join(", ", media.Stats.TvGenres.Where(x => x.Genre != "Drama" && x.Genre != "Thriller").OrderByDescending(x => x.Qty).Take(5).Select(x => x.Genre));

            media.TvTopTwoDecades = string.Join(", ", media.FavoriteMediaDecadeResults.OrderByDescending(x => x.Count).Take(2).Select(x => x.DecadeName));
        }

        /// <summary>
        /// Decide what decade each year result fits into and count the quantity of results for each
        /// </summary>
        /// <param name="favoriteMediaDecadeResults"></param>
        /// <param name="year"></param>
        public void RecordMediaDecade(List<FavoriteDecade> favoriteMediaDecadeResults, int year)
        {
            if (year >= 1950 && year <= 1959)
            {
                favoriteMediaDecadeResults.Where(w => w.DecadeName == "50s").ToList().ForEach(x => x.Count = x.Count + 1);
            }
            else if (year >= 1960 && year <= 1969)
            {
                favoriteMediaDecadeResults.Where(w => w.DecadeName == "60s").ToList().ForEach(x => x.Count = x.Count + 1);
            }
            else if (year >= 1970 && year <= 1979)
            {
                favoriteMediaDecadeResults.Where(w => w.DecadeName == "70s").ToList().ForEach(x => x.Count = x.Count + 1);
            }
            else if (year >= 1980 && year <= 1989)
            {
                favoriteMediaDecadeResults.Where(w => w.DecadeName == "80s").ToList().ForEach(x => x.Count = x.Count + 1);
            }
            else if (year >= 1990 && year <= 1999)
            {
                favoriteMediaDecadeResults.Where(w => w.DecadeName == "90s").ToList().ForEach(x => x.Count = x.Count + 1);
            }
            else if (year >= 2000 && year <= 2009)
            {
                favoriteMediaDecadeResults.Where(w => w.DecadeName == "2000s").ToList().ForEach(x => x.Count = x.Count + 1);
            }
            else if (year >= 2010 && year <= 2019)
            {
                favoriteMediaDecadeResults.Where(w => w.DecadeName == "2010s").ToList().ForEach(x => x.Count = x.Count + 1);
            }
            else if (year >= 2020 && year <= 2029)
            {
                favoriteMediaDecadeResults.Where(w => w.DecadeName == "2020s").ToList().ForEach(x => x.Count = x.Count + 1);
            }
        }
    }
}