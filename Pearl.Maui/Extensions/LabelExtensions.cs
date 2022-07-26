using Plugin.ValidationRules;

namespace Pearl.Maui.Extensions;

public static class LabelExtensions
{
    public static void RegisterValidation<T>(this Label label, Entry entry, Validatable<T> validatable)
    {
        entry.PropertyChanged += (_, _) =>
        {
            if (validatable.Validate())
            {
                label.IsVisible = false;
                return;
            }

            label.Text = validatable.Error;
        };

        // Trigger the property change for initial activation.
        entry.Text = entry.Text;
        label.IsVisible = true;
    }
}