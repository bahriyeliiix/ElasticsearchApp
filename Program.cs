using Business.Service;

var builder = WebApplication.CreateBuilder(args);

// ElasticsearchService'i ekleyin
builder.Services.AddSingleton<ElasticSearchService>();

// Diğer servisleri ekleyin
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Elasticsearch indeksi oluşturma
using (var scope = app.Services.CreateScope())
{
    var elasticsearchService = scope.ServiceProvider.GetRequiredService<ElasticSearchService>();
    elasticsearchService.CreateIndexIfNotExists();
}

// Middleware'leri ekleyin
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
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
