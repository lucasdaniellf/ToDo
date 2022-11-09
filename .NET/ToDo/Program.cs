using TaskManager.Model.ToDo;
using TaskManager.Repository;
using TaskManager.Repository.Classes;
using TaskManager.Repository.Interface;
using TaskManager.Services;
using TaskManager.Services.Hubs;
using TaskManager.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped(_ => new DbContext(builder.Configuration.GetConnectionString("SqliteConnectionString")));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepository<ToDo>, ToDoRepository>();
builder.Services.AddScoped<ToDoService>();

string policy = "MyPolicy";
builder.Services.AddCors(opt =>
{
    opt.AddPolicy(policy, p =>
    {
        p.AllowCredentials();
    });
});

builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.UseCors(policy);

app.UseEndpoints(endpoint =>
{
    endpoint.MapHub<ToDoHub>("/hub/todo");
});
app.Run();
