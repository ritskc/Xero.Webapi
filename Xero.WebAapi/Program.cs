using Xero.WebAapi.IRepository;
using Xero.WebAapi.IServices;
using Xero.WebAapi.Repository;
using Xero.WebAapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPdfReader, PdfReader>();
builder.Services.AddSingleton<IFileParser, TextFileParser>();
builder.Services.AddSingleton<IInvoiceRepository, MockInvoiceRepository>();
builder.Services.AddSingleton<IQueueManager, QueueManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
