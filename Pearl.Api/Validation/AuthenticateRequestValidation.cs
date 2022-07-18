using FluentValidation;
using Pearl.Api.Models.Requests;

namespace Pearl.Api.Validation;

public sealed class AuthenticateRequestValidation : AbstractValidator<AuthenticateRequest>
{
    public AuthenticateRequestValidation()
    {
        RuleFor(request => request.Username).NotEmpty();
        RuleFor(request => request.Password).NotEmpty();
    }
}