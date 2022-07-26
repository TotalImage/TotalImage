using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace TotalImage.UI.Controls;

public partial class PropertyRow : UserControl
{
    public static readonly DirectProperty<PropertyRow, string> LabelProperty
        = AvaloniaProperty.RegisterDirect<PropertyRow, string>(nameof(Label), o => o.Label, (o, v) => o.Label = v);

    public static readonly DirectProperty<PropertyRow, string> ValueProperty
        = AvaloniaProperty.RegisterDirect<PropertyRow, string>(nameof(Value), o => o.Value, (o, v) => o.Value = v);

    private string _label = "";
    private string _value = "";

    public string Label
    {
        get => _label;
        set => SetAndRaise(LabelProperty, ref _label, value);
    }

    public string Value
    {
        get => _value;
        set => SetAndRaise(ValueProperty, ref _value, value);
    }

    public PropertyRow()
    {
        InitializeComponent();
    }
}

