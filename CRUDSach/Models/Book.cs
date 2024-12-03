using System.ComponentModel.DataAnnotations;

public class Book
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

    public string? Image { get; set; }
}