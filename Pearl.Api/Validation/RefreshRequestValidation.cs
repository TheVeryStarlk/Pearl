using FluentValidation;
using Pearl.Models.Requests;

namespace Pearl.Api.Validation;

public sealed class RefreshRequestValidation : AbstractValidator<RefreshRequest>
{
    public RefreshRequestValidation()
    {
        RuleFor(request => request.AccessToken).NotEmpty();
        RuleFor(request => request.RefreshToken).NotEmpty();
    }
}