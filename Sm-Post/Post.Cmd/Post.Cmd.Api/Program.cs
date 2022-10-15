using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Cmd.Api.Commands;
using Post.Cmd.Domain.Aggregates;
using Post.Cmd.Infrastructure.Config;
using Post.Cmd.Infrastructure.Dispatchers;
using Post.Cmd.Infrastructure.Handlers;
using Post.Cmd.Infrastructure.Repositories;
using Post.Cmd.Infrastructure.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbConfig>(builder.Configuration.GetSection(nameof(MongoDbConfig)));
builder.Services.AddScoped<IEventStoreRepository, EventStoreRepository>();
builder.Services.AddScoped<IEventStore, EventStore>();
builder.Services.AddScoped<IEventSourcingHandler<PostAggregate>, EventSourcingHandler>();
builder.Services.AddScoped<ICommandHandler, CommandHandler>();

// Register cmdhandler emethods
var cmdHandler = builder.Services.BuildServiceProvider().GetRequiredService<ICommandHandler>();
var dispather = new CommandDispatcher();
dispather.RegisterHandler<NewPostCommand>(cmdHandler.HandlerAsync);
dispather.RegisterHandler<EditMessageCommand>(cmdHandler.HandlerAsync);
dispather.RegisterHandler<LikePostCommand>(cmdHandler.HandlerAsync);
dispather.RegisterHandler<AddCommentCommand>(cmdHandler.HandlerAsync);
dispather.RegisterHandler<EditCommentCommand>(cmdHandler.HandlerAsync);
dispather.RegisterHandler<RemoveCommentCommand>(cmdHandler.HandlerAsync);
dispather.RegisterHandler<DeletePostCommand>(cmdHandler.HandlerAsync);
builder.Services.AddSingleton<ICommandDispatcher>(_ => dispather);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
