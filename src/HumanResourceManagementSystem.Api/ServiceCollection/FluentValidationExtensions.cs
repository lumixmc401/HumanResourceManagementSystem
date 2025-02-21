using FluentValidation;
using FluentValidation.AspNetCore;
using HumanResourceManagementSystem.Service.Validators.Token;
using HumanResourceManagementSystem.Service.Validators.User;

namespace HumanResourceManagementSystem.Api.ServiceCollection
{
    public static class FluentValidationExtensions
    {
        public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableDataAnnotationsValidation = true;
            })
            .AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>()
            .AddValidatorsFromAssemblyContaining<LoginCredentialsDtoValidator>()
            .AddValidatorsFromAssemblyContaining<RefreshTokenRequestValidator>()
            .AddValidatorsFromAssemblyContaining<RevokeRefreshTokenRequestValidator>();
            return services;
        }
    }
}
