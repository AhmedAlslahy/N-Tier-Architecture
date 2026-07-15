using N_Tier.Application.Features.Admin.Message;
using N_Tier.Application.Features.Admin.Notification;
using N_Tier.Application.Features.Admin.Role;
using N_Tier.Application.Features.Admin.User;
using N_Tier.Application.Features.Auth;
using N_Tier.Application.Features.User.Message;
using N_Tier.Application.Features.User.Notification;
using N_Tier.Application.Features.User.User;
using N_Tier.Application.Features.User.UserSetting;
using N_Tier.Application.Helper.DTOs.Config;
using N_Tier.Application.Helper.Services.Implementation;
using N_Tier.Application.Helper.Services.Interfaces;
using N_Tier.Shared.Service;
using static N_Tier.Application.Features.Auth.Register;

namespace N_Tier.API;

public static class DependencyInjection
{
    //configuration
    public static WebApplicationBuilder AddProjectConfiguration(
  this WebApplicationBuilder builder)
    {
        builder.Configuration
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true)
            .AddJsonFile(
                $"appsettings.{builder.Environment.EnvironmentName}.json",
                optional: true)
            .AddEnvironmentVariables();

        return builder;
    }

    //Database
    public static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SarhneDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                configuration.GetConnectionString("ProjectConnection"));

            options.AddInterceptors(
                sp.GetRequiredService<AuditInterceptor>(),
                sp.GetRequiredService<UpdateAuditInterceptor>(),
                sp.GetRequiredService<SoftDeleteInterceptor>());
        });

        return services;
    }

    // Services
    public static IServiceCollection AddApplicationServices(
     this IServiceCollection services,
     IConfiguration configuration)
    {
        services.Configure<JwtInformations>(
                configuration.GetSection("JWTInformations"));
        services.Configure<EmailInformations>(
            configuration.GetSection("EmailInformations"));

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<UpdateAuditInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IEmailService, EmailService>();

        //Role
        services.AddScoped<GetAllRoles.GetAllRolesHandler>();
        services.AddScoped<DeleteRole.DeleteRoleHandler>();
        services.AddScoped<CreateRole.CreateRoleHandler>();
        services.AddScoped<AddAdminRole.AddAdminRoleHandler>();

        //User
        services.AddScoped<DeleteUser.DeleteUserHandler>();
        services.AddScoped<UpdateUser.UpdateUserHandler>();
        services.AddScoped<GetAllUsers.GetAllUsersHandler>();
        services.AddScoped<GetByLink.GetByLinkHandler>();

        //User Setting
        services.AddScoped<UpdateUserSetting.UpdateUserSettingHandler>();
        services.AddScoped<GetByLink.GetByLinkHandler>();

        //Auth
        services.AddScoped<Login.LoginHandler>();
        services.AddScoped<Register.RegisterHandler>();

        //Email
        services.AddScoped<SendConfirmEmailOTP.SendConfirmEmailOTPHandler>();
        services.AddScoped<SendForgetPasswordOTP.SendForgetPasswordOTPHandler>();
        services.AddScoped<ConfirmEmail.ConfirmEmailHandler>();
        services.AddScoped<ForgetPassword.ForgetPasswordHandler>();
        services.AddScoped<ResetPassword.ResetPasswordHandler>();

        //Notification
        services.AddScoped<SendNotification.SendNotificationHandler>();
        services.AddScoped<GetAllNotificationsByUserId.GetAllNotificationsByUserIdHandler>();
        services.AddScoped<GetNotificationById.GetNotificationByIdHandler>();
        services.AddScoped<UnreadCountNotificationByUserId.UnreadCountNotificationByUserIdHandler>();

        //Message
        services.AddScoped<SendMessage.SendMessageHandler>();
        services.AddScoped<GetAllMessageByUserId.GetAllMessageByUserIdHandler>();
        services.AddScoped<GetAllSenderMessageByUserId.GetAllSenderMessageByUserIdHandler>();
        services.AddScoped<GetAllMessageStarredByUserId.GetAllMessageStarredByUserIdHandler>();
        services.AddScoped<GetAllUnreadMessageByUserId.GetAllUnreadMessageByUserIdHandler>();
        services.AddScoped<GetMessageById.GetMessageByIdHandler>();
        services.AddScoped<SearchByWordOrUserName.SearchByWordOrUserNameHandler>();
        services.AddScoped<StarredMessageById.StarredMessageByIdHandler>();
        services.AddScoped<UnreadCountMessageByUserId.UnreadCountMessageByUserIdHandler>();

        return services;
    }

    //cors
    public static IServiceCollection AddCorsPolicy(
      this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("MyPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod();
            });
        });

        return services;
    }

    //validation
    public static IServiceCollection AddValidationServices(
        this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
                typeof(RegisterReq).Assembly)
               .AddFluentValidationAutoValidation();

        return services;
    }

    //JWT
    public static IServiceCollection AddJwtAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["JWTInformations:issuerIP"],

                    ValidateAudience = true,
                    ValidAudience = configuration["JWTInformations:audienceIP"],

                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(
                            configuration["JWTInformations:SecretKey"]!)),

                    ClockSkew = TimeSpan.Zero
                };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["jwt"];
                    return Task.CompletedTask;
                }
            };
        });

        return services;
    }

    ////IDentity
    //public static IServiceCollection AddIdentityServices(
    //    this IServiceCollection services)
    //{
    //    services.AddIdentity<User, IdentityRole>(options =>
    //    {
    //        options.Password.RequireDigit = true;
    //        options.Password.RequireLowercase = true;
    //        options.Password.RequireUppercase = true;
    //        options.Password.RequireNonAlphanumeric = false;
    //        options.Password.RequiredLength = 8;
    //    })
    //    .AddEntityFrameworkStores<SarhneDbContext>();

    //    return services;
    //}
}