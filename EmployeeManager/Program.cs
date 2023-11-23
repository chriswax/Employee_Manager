using EmployeeManager.DAL;
using EmployeeManager.Repositories.Abstract;
using EmployeeManager.Repositories.Implementation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.AddDbContext<DBContext>(options => options
.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));

//======Configurations for JWT ========///
//For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DBContext>()
    .AddDefaultTokenProviders();


//Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;    ///this was not in second reference
})    //adding jwt bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;  //can work without this
    options.RequireHttpsMetadata = false;  //can work without this
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        //ValidateIssuerSigningKey = true,
        //ValidAudience = ConfigurationManager.AppSetting["JWTAuth:ValidAudienceURL"],
        //ValidIssuer = ConfigurationManager.AppSetting["JWTAuth:ValidIssuerURL"],
        //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["JWTAuth:Secret"]))  //change t oconfiguration

        ValidAudience = "https://localhost:44395",
        ValidIssuer = "http://localhost:5004",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("3A152D7602D247A19F84558D41969369"))

    };
});


//======Configurations for JWT ========///


builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();   //enable single domain "http://localhost:4000" ---Multiple domain "http://localhost:400", "http://localhost:3000", Any Domain ..use "*"
}));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //app.UseSwagger();
    //app.UseSwaggerUI(options =>
    //{
    //    options.SwaggerEndpoint("/swagger/V1/swagger.json", "Product WebAPI");
    //});
   }
app.UseCors("corspolicy");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();