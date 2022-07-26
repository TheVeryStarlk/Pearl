using Plugin.ValidationRules.Interfaces;

namespace Pearl.Maui.Services;

public sealed class ValidationService
{
    public bool ValidateAll(params IValidity[] validities)
    {
        var success = true;
        foreach (var validity in validities)
        {
            if (!validity.Validate())
            {
                success = false;
            }
        }

        return success;
    }
}