using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

public class BookViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tiêu đề sách là bắt buộc")]
    [StringLength(200, ErrorMessage = "Tiêu đề không được quá 200 ký tự")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Tác giả là bắt buộc")]
    [StringLength(100, ErrorMessage = "Tên tác giả không được quá 100 ký tự")]
    public string Author { get; set; }

    [StringLength(1000, ErrorMessage = "Mô tả không được quá 1000 ký tự")]
    public string Description { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Giá sách phải lớn hơn hoặc bằng 0")]
    public decimal Price { get; set; }

    // Cho phép nhập link hình ảnh
    [Url(ErrorMessage = "Đây phải là một URL hợp lệ")]
    [Display(Name = "Đường Dẫn Hình Ảnh")]
    public string? ImageUrl { get; set; }

    // Cho phép upload file
    [Display(Name = "Tải Hình Ảnh Từ Máy")]
    public IFormFile? ImageFile { get; set; }

    // Lưu trữ đường dẫn cuối cùng của hình ảnh
    public string? Image { get; set; }
}

public static class BookMappingExtensions
{
    public static BookViewModel ToViewModel(this Book book)
    {
        return new BookViewModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Description = book.Description,
            Price = book.Price,
            Image = book.Image,
            ImageUrl = book.Image // Nếu link được lưu trực tiếp
        };
    }

    public static Book ToModel(this BookViewModel viewModel)
    {
        return new Book
        {
            Id = viewModel.Id,
            Title = viewModel.Title,
            Author = viewModel.Author,
            Description = viewModel.Description,
            Price = viewModel.Price,
            Image = viewModel.Image ?? viewModel.ImageUrl
        };
    }
}