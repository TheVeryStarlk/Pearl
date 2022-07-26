using Plugin.ValidationRules;

namespace Pearl.Maui.Extensions;

public static class LabelExtensions
{
    public static void RegisterValidation<T>(this Label label, Entry entry, Validatable<T> validatable)
    {
        TriggerVisibility();

        entry.PropertyChanged += (_, _) => TriggerVisibility();

        void TriggerVisibility()
        {
            if (validatable.Validate())
            {
                label.IsVisible = false;
                return;
            }

            label.Text = validatable.Error;
            label.IsVisible = true;
        }
    }
}