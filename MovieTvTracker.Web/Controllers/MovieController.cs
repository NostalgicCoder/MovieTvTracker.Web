using Microsoft.AspNetCore.Mvc;
using MovieTvTracker.Web.Models;
using MovieTvTracker.Web.Data;
using TmdbApi.Lib.Interfaces;
using MovieTvTracker.Web.Class;
using MovieTvTracker.Web.Interfaces;
using TmdbApi.Lib.Class;
using Microsoft.EntityFrameworkCore;
using TmdbApi.Lib.Enum;

namespace MovieTvTracker.Web.Controllers
{
    public class MovieController : Controller
    {
        private ITmdb _tmdb;

        private ErrorViewModel _errorViewModel;

        private readonly ApplicationDbContext _db;

        private int _pageSize = 25;

        /// <summary>
        /// Constructor
        /// </summary>
        public MovieController(ApplicationDbContext db, ITmdb tmdb)
        {
            _db = db;
            _tmdb = tmdb;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string keyword, bool EnglishResultOnly)
        {
            IMedia media = new Media();

            media.EnglishResultOnly = EnglishResultOnly;

            if(ModelState.IsValid)
            {
                media.TMDBData = _tmdb.SearchForFilmTvPerson(keyword);
             
                if(media.EnglishResultOnly)
                {
                    media.TMDBData.FilmResults.results = media.TMDBData.FilmResults.results.Where(item => item.original_language == Language.English).ToArray();
                    media.TMDBData.TvResults.results = media.TMDBData.TvResults.results.Where(item => item.original_language == Language.English).ToArray();
                }

                return View(media);
            }

            return RedirectToAction("MoviesNowPlaying", "Movie");
        }

        /// <summary>
        /// Gets the current movies that are playing and being showing in UK cinemas that are English language
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult MoviesNowPlaying()
        {
            IMedia media = new Media();

            media.TMDBData = _tmdb.MoviesNowPlaying();

            media.TMDBData.MoviesNowPlaying.results = media.TMDBData.MoviesNowPlaying.results.Where(item => item.original_language == Language.English).ToArray();

            return View(media);
        }

        /// <summary>
        /// Process all results in the 'WatchedMedia' table that match film content then get data from TMDB API on each of them and generate stats.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<IActionResult> GetMyFilmStats()
        {
            try
            {
                IMedia media = new Media();
                IGetStatistics stats = new GetStatistics();

                media.WatchedMediaResults = new WatchedMediaResults();

                foreach (WatchedMedia item in await _db.WatchedMedia.Where(x => x.ContentType == "Film").ToListAsync())
                {
                    media.WatchedMediaResults.WatchedFilms.Add(new WatchedMediaItem
                    {
                        ResultReturn = _tmdb.SearchForFilmAndCreditsById(item.TMDBId),
                        WatchedMedia = item
                    });
                }

                media = stats.GetFilmYearsRangeAndGenres(media);
                media = stats.GetQtyViewingStats(media);

                return View(media);
            }
            catch(Exception ex)
            {
                throw new Exception("GetMyFilmStats", ex);
            }
        }

        /// <summary>
        /// Process all results in the 'WatchedMedia' table that match TV content then get data from TMDB API on each of them and generate stats.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<IActionResult> GetMyTvStats()
        {
            try
            {
                IMedia media = new Media();
                IGetStatistics stats = new GetStatistics();

                media.WatchedMediaResults = new WatchedMediaResults();

                foreach (WatchedMedia item in await _db.WatchedMedia.Where(x => x.ContentType == "TV").ToListAsync())
                {
                    media.WatchedMediaResults.WatchedTV.Add(new WatchedMediaItem
                    {
                        ResultReturn = _tmdb.SearchForTvAndCreditsById(item.TMDBId),
                        WatchedMedia = item
                    });
                }

                media = stats.GetTvGenres(media);
                media = stats.GetQtyViewingStats(media);

                return View(media);
            }
            catch (Exception ex)
            {
                throw new Exception("GetMyTvStats", ex);
            }
        }

        /// <summary>
        /// Process all results in the 'WatchedMedia' table that match film content, paginate the content to fit the chosen pageSize and maintain performance.  Filter results by a keyword provided by the user
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IActionResult> GetWatchedMediaFilm(int? pageNumber, Media media)
        {
            try
            {
                IGetStatistics stats = new GetStatistics();

                media.WatchedMediaResults = new WatchedMediaResults();

                if (!string.IsNullOrEmpty(media.Keyword))
                {
                    // Aquire all the film TMDB ID values that match the provided keyword value
                    List<Int32> matchedKeywordResults = _tmdb.ConvertIdToTitleAndCheckForKeywordMatch(_db.WatchedMedia.Where(x => x.ContentType == "Film").Select(x => x.TMDBId).ToList(), media.Keyword, Caller.Film);

                    // Only paginate through the keyword matched TMDB ID values
                    media.PaginatedWatchedMediaList = await PaginatedList<WatchedMedia>.CreateAsync(_db.WatchedMedia.Where(x => matchedKeywordResults.Contains(x.TMDBId)).OrderByDescending(x => x.LastWatched), pageNumber ?? 1, _pageSize);
                }
                else
                {
                    media.PaginatedWatchedMediaList = await PaginatedList<WatchedMedia>.CreateAsync(_db.WatchedMedia.Where(x => x.ContentType == "Film").OrderByDescending(x => x.LastWatched), pageNumber ?? 1, _pageSize);
                }

                foreach (WatchedMedia item in media.PaginatedWatchedMediaList)
                {
                    media.WatchedMediaResults.WatchedFilms.Add(new WatchedMediaItem
                    {
                        ResultReturn = _tmdb.SearchForFilmAndCreditsById(item.TMDBId),
                        WatchedMedia = item
                    });
                }

                return View(media);
            }
            catch(Exception ex)
            {
                throw new Exception("GetWatchedMediaFilm", ex);
            }
        }

        /// <summary>
        /// Process all results in the 'WatchedMedia' table that match TV content, paginate the content to fit the chosen pageSize and maintain performance.
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<IActionResult> GetWatchedMediaTv(int? pageNumber)
        {
            try
            {
                IMedia media = new Media();
                IGetStatistics stats = new GetStatistics();

                media.WatchedMediaResults = new WatchedMediaResults();
                media.PaginatedWatchedMediaList = await PaginatedList<WatchedMedia>.CreateAsync(_db.WatchedMedia.Where(x => x.ContentType == "TV").OrderByDescending(x => x.LastWatched), pageNumber ?? 1, _pageSize);

                foreach (WatchedMedia item in media.PaginatedWatchedMediaList)
                {
                    media.WatchedMediaResults.WatchedTV.Add(new WatchedMediaItem
                    {
                        ResultReturn = _tmdb.SearchForTvAndCreditsById(item.TMDBId),
                        WatchedMedia = item
                    });
                }

                return View(media);
            }
            catch(Exception ex)
            {
                throw new Exception("GetWatchedMediaTv", ex);
            }
        }

        /// <summary>
        /// Aquire favorite actor results from the database, run those results through TMDB API to get information and then feed that to the model that supplies the view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFavoriteActors(int? pageNumber)
        {
            IMedia media = new Media();

            media.FavoriteActorTotalCount = await _db.FavoriteActor.CountAsync(); ;
            media.PaginatedFavoriteActorList = await PaginatedList<FavoriteActor>.CreateAsync(_db.FavoriteActor, pageNumber ?? 1, _pageSize);

            try
            {
                foreach (FavoriteActor item in media.PaginatedFavoriteActorList)
                {
                    try
                    {
                        media.FavoriteActorResults.Add(_tmdb.SearchForPersonAndCreditsById(item.TMDBId));
                    }
                    catch (Exception ex)
                    {
                        /*
                         Catch per result errors, so if one '.add' fails the whole thing doesnt stop
                        TODO: Add error handling
                         */
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetFavoriteActors", ex);
            }

            return View(media);
        }

        /// <summary>
        /// Check if the selected TMDB ID exists in the database, if a result already exists update minimal req fields otherwise create a brand new 'WatchedMedia' entity result in the database.
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsWatched(Media media)
        {
            try
            {
                WatchedMedia watchedMedia = await _db.WatchedMedia.Where(x => x.TMDBId == media.SelectedTMDBId).FirstOrDefaultAsync();

                if (watchedMedia == null)
                {
                    watchedMedia = new WatchedMedia();

                    watchedMedia.TMDBId = media.SelectedTMDBId;
                    watchedMedia.ContentType = media.SelectedContentType;
                    watchedMedia.WatchedQty = 1;
                    watchedMedia.FirstWatched = DateTime.Now;
                    watchedMedia.LastWatched = DateTime.Now;

                    await _db.WatchedMedia.AddAsync(watchedMedia);
                }
                else
                {
                    watchedMedia.LastWatched = DateTime.Now;
                    watchedMedia.WatchedQty = watchedMedia.WatchedQty + 1;

                    _db.WatchedMedia.Update(watchedMedia);
                }

                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("MarkAsWatched", ex);
            }

            return RedirectToAction("MoviesNowPlaying", "Movie");
        }

        /// <summary>
        /// Check if the selected TMDB ID exists in the database, if a result doesnt already exist then create a 'FavoriteActor' entity and add it to the database
        /// </summary>
        /// <param name="media"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkFavoriteActor(Media media)
        {
            try
            {
                FavoriteActor favoriteActor = await _db.FavoriteActor.Where(x => x.TMDBId == media.TMDBData.PersonIdResult.id).FirstOrDefaultAsync();

                if (favoriteActor == null)
                {
                    favoriteActor = new FavoriteActor()
                    {
                        TMDBId = media.TMDBData.PersonIdResult.id
                    };

                    await _db.FavoriteActor.AddAsync(favoriteActor);
                    await _db.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("MarkFavoriteActor", ex);
            }

            return RedirectToAction("MoviesNowPlaying", "Movie");
        }

        /// <summary>
        /// Return the user selected film item from TMDB API and display in the view.  If no result can be found display the error view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserFilmSelectionById(int id)
        {
            IMedia media = new Media();

            media.SelectedTMDBId = id;
            media.TMDBData = _tmdb.SearchForFilmAndCreditsById(id);

            if (media.TMDBData.FilmIdResult != null && media.TMDBData.CreditsByFilmId != null)
            {
                return View(media);
            }
            else
            {
                _errorViewModel = new ErrorViewModel();
                _errorViewModel.Error = "FilmIdResult or CreditsByFilmId has been returned NULL.";

                return View("Error", _errorViewModel);
            }
        }

        /// <summary>
        /// Return the user selected TV item from TMDB API and display in the view.  If no result can be found display the error view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserTVSelectionById(int id)
        {
            IMedia media = new Media();

            media.SelectedTMDBId = id;
            media.TMDBData = _tmdb.SearchForTvAndCreditsById(id);

            if (media.TMDBData.TvIdResult != null)
            {
                return View(media);
            }
            else
            {
                _errorViewModel = new ErrorViewModel();
                _errorViewModel.Error = "TVIdResult has been returned NULL.";

                return View("Error", _errorViewModel);
            }
        }

        /// <summary>
        /// Return the user selected person from TMDB API and display in the view.  If no result can be found display the error view.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetUserPersonSelectionById(int id)
        {
            IMedia media = new Media();

            media.SelectedTMDBId = id;
            media.TMDBData = _tmdb.SearchForPersonAndCreditsById(id);

            if (media.TMDBData.PersonIdResult != null)
            {
                return View(media);
            }
            else
            {
                _errorViewModel = new ErrorViewModel();
                _errorViewModel.Error = "PersonIdResult has been returned NULL.";

                return View("Error", _errorViewModel);
            }
        }

        /// <summary>
        /// Find and delete the user selected favorite actor from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> DeleteFavoriteActor(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            FavoriteActor obj = await _db.FavoriteActor.Where(x => x.TMDBId == id).FirstAsync();

            if (obj == null)
            {
                return NotFound();
            }

            try
            {
                _db.FavoriteActor.Remove(obj);
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _errorViewModel = new ErrorViewModel();
                _errorViewModel.Error = "Unable to delete favorite actor result from the database.";

                return View("Error", _errorViewModel);
            }

            return RedirectToAction("GetFavoriteActors", "Movie");
        }
    }
}