namespace Pearl.Maui.Extensions;

public static class LabelExtensions
{
    public static void RegisterVisibilityToggler(this Label label)
    {
        label.PropertyChanged += (_, _) => label.IsVisible = !string.IsNullOrWhiteSpace(label.Text);
    }
}