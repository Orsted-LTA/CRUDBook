public class BookRepository : IBookRepository
{
    private readonly List<Book> _books = new();
    private int _nextId = 1;

    public async Task<List<Book>> GetAllBooksAsync()
    {
        return await Task.FromResult(_books);
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await Task.FromResult(_books.FirstOrDefault(b => b.Id == id));
    }

    public async Task<Book> AddBookAsync(Book book)
    {
        book.Id = _nextId++;
        _books.Add(book);
        return await Task.FromResult(book);
    }

    public async Task<Book> UpdateBookAsync(Book book)
    {
        var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
        if (existingBook == null)
            throw new ArgumentException("Sách không tồn tại");

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Description = book.Description;
        existingBook.Price = book.Price;
        existingBook.Image = book.Image;

        return await Task.FromResult(existingBook);
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book != null)
            _books.Remove(book);

        await Task.CompletedTask;
    }
}