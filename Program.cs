// MiniPKI Project - RPLM 2024
// ---------------------------
// * 24 feb 2024 : codename [ vlady@cazador ] in memorian dear friend
// * 27 feb 2024 : mejoras en filtros object
// * 29 feb 2024 : agregar el administrador de arhivos cert
// * 04 mar 2024 : agregado X509Generador para creacion de CertiDigitales 
// * 04 mar 2024 : finalización y publicación
//
// using MiniPki.Models;  // noEF

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<MyData>();  // noEF
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".SuperMemory.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{   
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
