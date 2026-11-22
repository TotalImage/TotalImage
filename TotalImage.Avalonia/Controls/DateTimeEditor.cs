using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Layout;

namespace TotalImage.Avalonia.Controls;

/// <summary>
/// Edits a date with an optional time of day: a calendar date picker and, when <see cref="Format"/>
/// contains a time, a time picker with seconds. Shows nothing while <see cref="Format"/> is
/// <c>null</c> (the date is not set).
/// </summary>
public sealed class DateTimeEditor : UserControl
{
    public static readonly StyledProperty<DateTime> ValueProperty =
        AvaloniaProperty.Register<DateTimeEditor, DateTime>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public static readonly StyledProperty<string?> FormatProperty =
        AvaloniaProperty.Register<DateTimeEditor, string?>(nameof(Format));

    private readonly CalendarDatePicker datePicker = new()
    {
        SelectedDateFormat = CalendarDatePickerFormat.Custom,
        CustomDateFormatString = "yyyy-MM-dd",
        Watermark = " ",
        IsTodayHighlighted = true,
        MinWidth = 140
    };

    private readonly TimePicker timePicker = new()
    {
        ClockIdentifier = "24HourClock",
        UseSeconds = true,
        MinWidth = 0
    };

    private bool updating;

    // Whether the time is edited too, from the last format that was set: an unset date has none.
    private bool hasTime;

    public DateTimeEditor()
    {
        Content = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 4,
            Children = { datePicker, timePicker }
        };

        datePicker.SelectedDateChanged += (_, _) => UpdateValue();
        timePicker.SelectedTimeChanged += (_, _) => UpdateValue();
        UpdatePickers();
    }

    /// <summary>
    /// The date and time.
    /// </summary>
    public DateTime Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// The custom date format pattern of the value, or <c>null</c> to show nothing. The time picker
    /// is shown when it contains hours.
    /// </summary>
    public string? Format
    {
        get => GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == ValueProperty || change.Property == FormatProperty)
            UpdatePickers();
    }

    private void UpdatePickers()
    {
        updating = true;
        try
        {
            if (Format is not null)
                hasTime = Format.Contains('H');
            timePicker.IsVisible = hasTime;
            // An unset date shows nothing, not the time picker's placeholders.
            timePicker.Opacity = Format is null ? 0 : 1;
            datePicker.SelectedDate = Format is null ? null : Value.Date;
            timePicker.SelectedTime = Format is null ? null : Value.TimeOfDay;
        }
        finally
        {
            updating = false;
        }
    }

    private void UpdateValue()
    {
        if (updating || Format is null)
            return;

        if (datePicker.SelectedDate is not { } date)
        {
            // A date can't be cleared here; that is what the check box next to it is for.
            UpdatePickers();
            return;
        }

        var time = hasTime ? timePicker.SelectedTime ?? Value.TimeOfDay : Value.TimeOfDay;
        Value = date.Date + new TimeSpan(time.Hours, time.Minutes, time.Seconds);
    }
}
