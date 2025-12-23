using EVS.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register database services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<AssignmentService>();
builder.Services.AddScoped<ExamService>();
builder.Services.AddScoped<ParentService>();
builder.Services.AddScoped<ClassroomService>();
builder.Services.AddScoped<AdmissionService>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Enable session middleware

app.UseAuthorization();

app.MapRazorPages();

app.Run();