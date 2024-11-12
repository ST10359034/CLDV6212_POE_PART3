using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ABC_Retail.Services; // Add this for the services

var builder = WebApplication.CreateBuilder(args);

// Add HTTP client for services that need it
builder.Services.AddHttpClient();

// Add controllers with views
builder.Services.AddControllersWithViews();

// Register the services for dependency injection
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<BlobService>();
builder.Services.AddScoped<OrderService>();     
builder.Services.AddScoped<ProductService>();   
builder.Services.AddScoped<TableService>();     
builder.Services.AddScoped<QueueService>();     
builder.Services.AddScoped<FileService>();      

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();




