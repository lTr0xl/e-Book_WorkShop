using e_Book.Interfaces;
using e_Book.Models;
using e_Book.Repository;
using e_Book.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace e_Book.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly IBookGenreRepository _bookGenreRepository;
        private readonly IPhotoService _photoService;
        private readonly IUserBooksRepository _userBooksRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IBookRepository bookRepository , IBookGenreRepository bookGenreRepository ,
            IPhotoService photoService, IUserBooksRepository userBooksRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _bookGenreRepository = bookGenreRepository;
            _photoService = photoService;
            _userBooksRepository = userBooksRepository;
            _webHostEnvironment = webHostEnvironment;
        }


        [HttpGet]
        public async Task<IActionResult> Index(BookViewModel bookVM)
        {
            IEnumerable<Author> authors = await _bookRepository.GetAllAuthors();
            IEnumerable<Genre> genres = await _bookRepository.GetAllGenres();
            IEnumerable<Book> books;
            if (bookVM.AuthorSearchId == 0 && bookVM.GenreSearchId == 0)
            {
                books = await _bookRepository.GetAll();
            }
            else if(bookVM.AuthorSearchId != 0 && bookVM.GenreSearchId == 0)
            {
                books = await _bookRepository.GetBooksByAuthorId(bookVM.AuthorSearchId);
            }
            else if (bookVM.AuthorSearchId == 0 && bookVM.GenreSearchId != 0)
            {
                books = await _bookRepository.GetBooksByGenreId(bookVM.GenreSearchId);
            }
            else
            {
                IEnumerable<Book> booksA = await _bookRepository.GetBooksByAuthorId(bookVM.AuthorSearchId);
                IEnumerable<Book> booksG = await _bookRepository.GetBooksByGenreId(bookVM.GenreSearchId);
                List<Book> booksAG = new List<Book>();
                foreach (var bookA in booksA)
                {
                    foreach (var bookG in booksG)
                    {
                        if (bookA.Id == bookG.Id)
                        {
                            booksAG.Add(bookA);
                            break;
                        }
                    }
                }
                books = booksAG;
            }

            if (bookVM.SearchByName != null)
            {
                IEnumerable<Book> booksN = await _bookRepository.GetBooksByName(bookVM.SearchByName);
                List<Book> booksAGN = new List<Book>();
                foreach (var book in books)
                {
                    foreach (var bookN in booksN)
                    {
                        if (bookN.Id == book.Id)
                        {
                            booksAGN.Add(book);
                            break;
                        }
                    }
                }
                books = booksAGN;
            }
            BookViewModel viewModel = new BookViewModel()
            {
                Books = books,
                Authors = authors,
                Genres = genres,
            };
            return View(viewModel);
        }
        public async Task<IActionResult> SearchByAuthorId(int id)
        {
            IEnumerable<Book> books = await _bookRepository.GetBooksByAuthorId(id);
            return View(books);
        }
        public async Task<IActionResult> SearchByGenreId(int id)
        {
            IEnumerable<Book> books = await _bookRepository.GetBooksByGenreId(id);
            return View(books);
        }
        public async Task<IActionResult> Details(int id)
        {
            Book book = await _bookRepository.GetByIdAsync(id);
            bool hasBook = false;
            if (User.Identity.IsAuthenticated)
            {
                hasBook = _userBooksRepository.HasBook(id);
            }
            if (book == null)
            {
                return View("Error");
            }
            DetailsViewModel detailsVM = new DetailsViewModel()
            {
                Book = book,
                HasBook = hasBook,
            };
            return View(detailsVM);
        }


        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddBook()
        {
            IEnumerable<Author> authors = await _bookRepository.GetAllAuthors();
            IEnumerable<Genre> genres = await _bookRepository.GetAllGenres();
            AddBookViewModel bVM = new AddBookViewModel()
            {
                Authors = authors,
                Genres = genres,
            };
            return View(bVM);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddBook(AddBookViewModel addBookVM)
        {
            if(!ModelState.IsValid)
            {
                IEnumerable<Author> authors = await _bookRepository.GetAllAuthors();
                addBookVM.Authors = authors;
                IEnumerable<Genre> genres = await _bookRepository.GetAllGenres();
                addBookVM.Genres = genres;
                ModelState.AddModelError("", "Failed to add book");
                return View(addBookVM);
            }

            var resultFront = await _photoService.AddPhotoAsync(addBookVM.FrontPage);
            if(addBookVM.DownloadPdf != null)
            {
                string folder = "Bookspdf/";
                addBookVM.DownlaodUrl = await UploadImage(folder, addBookVM.DownloadPdf);
            }

            var newBook = new Book()
            {
                Title=addBookVM.Title,
                YearPublished=addBookVM.YearPublished,
                NumPages=addBookVM.NumPages,
                Description=addBookVM.Description,
                Publisher=addBookVM.Publisher,
                FrontPage=resultFront.Url.ToString(),
                AuthorId=addBookVM.AuthorId,
                DownloadUrl = addBookVM.DownlaodUrl,
            };
            _bookRepository.Add(newBook);

            Book newLastBook = await _bookRepository.GetLastBook();
            foreach(var genreId in addBookVM.GenreIds)
            {
                BookGenre bookGenre = new BookGenre()
                {
                    BookId = newLastBook.Id,
                    GenreId = genreId,
                };
                _bookGenreRepository.Add(bookGenre);
            }
            return RedirectToAction("Index");
        }

        private async Task<string?> UploadImage(string folderPath, IFormFile file)
        {
            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);
            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
            return "/" + folderPath;
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            Book book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return View("Error");
            }
            return View(book);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBookConfirmed(int id)
        {
            Book book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return View("Error");
            }
            _bookRepository.Delete(book);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditBook(int id)
        {
            Book book = await _bookRepository.GetByIdAsyncNoTracking(id);
            if (book == null)
            {
                return View("Error");   
            }
            IEnumerable<Author> authors = await _bookRepository.GetAllAuthors();
            IEnumerable<Genre> genres = await _bookRepository.GetAllGenres();
            List<int> genreIds = new List<int>();
            foreach(var genre in book.BookGenres)
            {
                genreIds.Add(genre.GenreId);
            }
            EditBookViewModel bookVM = new EditBookViewModel()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                YearPublished = book.YearPublished,
                NumPages = book.NumPages,
                Publisher = book.Publisher,
                FrontPageURL = book.FrontPage,
                DownloadUrl = book.DownloadUrl,
                Authors = authors,
                Genres = genres,
                AuthorId = book.AuthorId,
                GenreIds = genreIds,
            };
            return View(bookVM);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditBook(int id,EditBookViewModel bookVM)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<Author> authors = await _bookRepository.GetAllAuthors();
                bookVM.Authors = authors;
                IEnumerable<Genre> genres = await _bookRepository.GetAllGenres();
                bookVM.Genres = genres;
                ModelState.AddModelError("", "Failed to edit book");
                return View(bookVM);
            }
            var book = await _bookRepository.GetByIdAsyncNoTracking(id);
            if (book != null)
            {
                try
                {
                    if (bookVM.FrontPage != null)
                    {
                        await _photoService.DeletePhotoAsync(book.FrontPage);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    IEnumerable<Author> authors = await _bookRepository.GetAllAuthors();
                    bookVM.Authors = authors;
                    IEnumerable<Genre> genres = await _bookRepository.GetAllGenres();
                    bookVM.Genres = genres;
                    return View(bookVM);
                }
                var photoResult = "";
                if(bookVM.FrontPage != null)
                {
                    photoResult = (await _photoService.AddPhotoAsync(bookVM.FrontPage)).Url.ToString();
                }
                else
                {
                    photoResult = book.FrontPage;
                }
                if (bookVM.DownloadPdf != null)
                {
                    string folder = "Bookspdf/";
                    bookVM.DownloadUrl = await UploadImage(folder, bookVM.DownloadPdf);
                }
                Book newBook = new Book()
                {
                    Id = bookVM.Id,
                    Title = bookVM.Title,
                    Description = bookVM.Description,
                    YearPublished = bookVM.YearPublished,
                    NumPages = bookVM.NumPages,
                    Publisher = bookVM.Publisher,
                    FrontPage = photoResult,
                    DownloadUrl = bookVM.DownloadUrl,
                    AuthorId = bookVM.AuthorId,
                };
                _bookRepository.Update(newBook);
                IEnumerable<BookGenre> bookGenres = await _bookGenreRepository.GetAll();
                foreach(var bg in bookGenres){
                    if(bg.BookId == id)
                    {
                        _bookGenreRepository.Delete(bg);
                    }
                }
                foreach (var genreId in bookVM.GenreIds)
                {
                    BookGenre bookGenre = new BookGenre()
                    {
                        BookId = book.Id,
                        GenreId = genreId,
                    };
                    _bookGenreRepository.Add(bookGenre);
                }

                return Redirect("/Book/Details/" + id);
            }
            else
            {
                IEnumerable<Author> authors = await _bookRepository.GetAllAuthors();
                bookVM.Authors = authors;
                IEnumerable<Genre> genres = await _bookRepository.GetAllGenres();
                bookVM.Genres = genres;
                return View(bookVM);
            }
        }
    }
}
