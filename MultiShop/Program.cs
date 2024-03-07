using System.Text.Unicode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//ben ekledim..//Süre 1 dk olarak belirlendi..sepete ekle 
builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromMinutes(1);
});

//BEN EKLEDÝM ALERT TÜRKCE KARAKTER SORUNU ÝÇÝN
builder.Services.AddWebEncoders(o =>
{
    o.TextEncoderSettings = new System.Text.Encodings.Web.TextEncoderSettings(UnicodeRanges.All);
});


//ben ekledim layout da session login görünümü için (logo altýnda email görünsün)
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();// ben ekledim........ sepete ekle, login giriþte session atýcam,order login kontrolu

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
