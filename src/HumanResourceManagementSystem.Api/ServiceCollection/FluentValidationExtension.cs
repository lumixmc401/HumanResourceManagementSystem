using FluentValidation;
using FluentValidation.AspNetCore;
using HumanResourceManagementSystem.Service.Validators.User;

namespace HumanResourceManagementSystem.Api.ServiceCollection
{
    public static class FluentValidationExtension
    {
        public static IServiceCollection AddApplicationValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation(config =>
            {
                config.DisableDataAnnotationsValidation = true;
            })
            .AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>()
            .AddValidatorsFromAssemblyContaining<VerifyUserDtoValidator>();
            return services;
        }
    }
}
