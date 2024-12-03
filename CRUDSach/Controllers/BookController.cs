using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;

public class BookController : Controller
{
    private readonly IBookRepository _bookRepository;
    private readonly IWebHostEnvironment _hostEnvironment;

    public BookController(IBookRepository bookRepository, IWebHostEnvironment hostEnvironment)
    {
        _bookRepository = bookRepository;
        _hostEnvironment = hostEnvironment;
    }

    // Action Index - Hiển thị danh sách sách
    public async Task<IActionResult> Index()
    {
        var books = await _bookRepository.GetAllBooksAsync();
        return View(books);
    }

    // Action Details - Hiển thị chi tiết sách
    public async Task<IActionResult> Details(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();

        return View(book);
    }

    // Action Create - Thêm mới sách
    [HttpGet]
    public IActionResult Create()
    {
        return View(new BookViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BookViewModel bookViewModel)
    {
        if (!ModelState.IsValid)
            return View(bookViewModel);

        // Ưu tiên upload file trước
        if (bookViewModel.ImageFile != null)
        {
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + bookViewModel.ImageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await bookViewModel.ImageFile.CopyToAsync(fileStream);
            }

            bookViewModel.Image = $"/uploads/{uniqueFileName}";
        }
        // Nếu không upload file, sử dụng link
        else if (!string.IsNullOrEmpty(bookViewModel.ImageUrl))
        {
            bookViewModel.Image = bookViewModel.ImageUrl;
        }

        var book = bookViewModel.ToModel();
        await _bookRepository.AddBookAsync(book);
        return RedirectToAction(nameof(Index));
    }

    // Action Edit - Chỉnh sửa sách
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();

        return View(book.ToViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(BookViewModel bookViewModel)
    {
        if (!ModelState.IsValid)
            return View(bookViewModel);

        // Ưu tiên upload file
        if (bookViewModel.ImageFile != null)
        {
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + bookViewModel.ImageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await bookViewModel.ImageFile.CopyToAsync(fileStream);
            }

            // Xóa file cũ nếu tồn tại
            if (!string.IsNullOrEmpty(bookViewModel.Image) &&
                bookViewModel.Image.StartsWith("/uploads/"))
            {
                var oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, bookViewModel.Image.TrimStart('/'));
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            bookViewModel.Image = $"/uploads/{uniqueFileName}";
        }
        // Nếu không upload file, sử dụng link
        else if (!string.IsNullOrEmpty(bookViewModel.ImageUrl))
        {
            bookViewModel.Image = bookViewModel.ImageUrl;
        }

        var book = bookViewModel.ToModel();
        await _bookRepository.UpdateBookAsync(book);
        return RedirectToAction(nameof(Index));
    }
    // Action Delete - Xóa sách
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();

        return View(book);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();

        // Xóa file hình ảnh nếu tồn tại
        if (!string.IsNullOrEmpty(book.Image))
        {
            var filePath = Path.Combine(_hostEnvironment.WebRootPath, book.Image.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        await _bookRepository.DeleteBookAsync(id);
        return RedirectToAction(nameof(Index));
    }
}