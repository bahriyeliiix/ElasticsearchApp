using Business.Service;
using Microsoft.AspNetCore.Mvc;

namespace ElasticsearchApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly ElasticSearchService _elasticSearchService;

        public BooksController(ElasticSearchService elasticSearchService)
        {
            _elasticSearchService = elasticSearchService;
        }

        public IActionResult Index()
        {
            var result = _elasticSearchService.GetAllBooks();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Book book)
        {
            book.Id = Guid.NewGuid().ToString();
            _elasticSearchService.IndexDocument(book);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(string id)
        {
            var book = _elasticSearchService.GetBook(id);
            return View(book);
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var book = _elasticSearchService.GetBook(id);
            if (book != null)
            {
                _elasticSearchService.DeleteBook(id); 
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string id)
        {
            var book = _elasticSearchService.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        public IActionResult Edit(string id)
        {
            var book = _elasticSearchService.GetBook(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _elasticSearchService.IndexDocument(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

    }
}
