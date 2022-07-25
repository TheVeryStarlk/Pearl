using Plugin.ValidationRules.Interfaces;

namespace Pearl.Maui.Services;

public sealed class ValidationService
{
    public bool ValidateAll(params IValidity[] validities)
    {
        var failed = true;
        foreach (var validity in validities)
        {
            if (!validity.Validate())
            {
                failed = true;
            }
        }

        return failed;
    }
}