using EVS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Add session support
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Register ALL database services (using Scoped lifetime for database connections)
// Account & Admin Services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<AssignmentService>();
builder.Services.AddScoped<ExamService>();
builder.Services.AddScoped<ParentService>();
builder.Services.AddScoped<ClassroomService>();
builder.Services.AddScoped<AdmissionService>();

// Student, Parent, Teacher Services
builder.Services.AddScoped<AnnouncementService>();
builder.Services.AddScoped<AttendanceService>();
builder.Services.AddScoped<MessageService>();
builder.Services.AddScoped<TranscriptService>();
builder.Services.AddScoped<ScheduleService>();
builder.Services.AddScoped<GradeService>();

var app = builder.Build();

// Configure the HTTP request pipeline
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