using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Namespace của project (thay thế bằng namespace của bạn)
namespace BookManagementApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Tạo WebApplicationBuilder
            var builder = WebApplication.CreateBuilder(args);

            // Cấu hình dịch vụ
            ConfigureServices(builder.Services);

            // Xây dựng ứng dụng
            var app = builder.Build();

            // Cấu hình middleware
            ConfigureMiddleware(app);

            // Chạy ứng dụng
            app.Run();
        }

        // Phương thức cấu hình các dịch vụ
        private static void ConfigureServices(IServiceCollection services)
        {
            // Đăng ký dịch vụ MVC
            services.AddControllersWithViews();

            // Đăng ký repository
            services.AddSingleton<IBookRepository, BookRepository>();

            // Thêm dịch vụ Authorization
            services.AddAuthorization();
        }

        // Phương thức cấu hình middleware
        private static void ConfigureMiddleware(WebApplication app)
        {
            // Môi trường phát triển
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            // Middleware tiêu chuẩn
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Authentication & Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            // Cấu hình route mặc định
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Book}/{action=Index}/{id?}");
        }
    }
}