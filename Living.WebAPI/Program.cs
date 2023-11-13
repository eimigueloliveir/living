using FluentValidation;
using Living.Application.UseCases.Posts.Create;
using Living.Application.Validation;
using Living.WebAPI.Extensions;
using System.Text.Json;

namespace Living.WebAPI;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
            options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMediatR(configuration =>
        {
            //configuration.AddBehavior(typeof(ValidationBehavior<>));
            configuration.RegisterServicesFromAssemblyContaining(typeof(CreatePostCommand));
        });

        builder.Services.AddValidatorsFromAssemblyContaining<CreatePostValidator>();

        builder.Services.ConfigureFluentMigrator(builder.Configuration);

        builder.Services.AddAuthentication().AddBearerToken();
        builder.Services.AddAuthorization();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UpdateDatabase();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
