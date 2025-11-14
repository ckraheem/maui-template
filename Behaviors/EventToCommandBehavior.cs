using System.Reflection;
using System.Windows.Input;

namespace MauiTemplate.Behaviors;

/// <summary>
/// Behavior that converts any event to a command
/// </summary>
public class EventToCommandBehavior : Behavior<View>
{
    public static readonly BindableProperty EventNameProperty =
        BindableProperty.Create(nameof(EventName), typeof(string), typeof(EventToCommandBehavior));

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EventToCommandBehavior));

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(EventToCommandBehavior));

    public string EventName
    {
        get => (string)GetValue(EventNameProperty);
        set => SetValue(EventNameProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    private Delegate? _eventHandler;

    protected override void OnAttachedTo(View bindable)
    {
        base.OnAttachedTo(bindable);

        if (string.IsNullOrWhiteSpace(EventName))
            return;

        var eventInfo = bindable.GetType().GetRuntimeEvent(EventName);
        if (eventInfo == null)
            return;

        var methodInfo = typeof(EventToCommandBehavior).GetTypeInfo()
            .GetDeclaredMethod(nameof(OnEvent));

        _eventHandler = methodInfo!.CreateDelegate(eventInfo.EventHandlerType!, this);
        eventInfo.AddEventHandler(bindable, _eventHandler);
    }

    protected override void OnDetachingFrom(View bindable)
    {
        if (_eventHandler != null && !string.IsNullOrWhiteSpace(EventName))
        {
            var eventInfo = bindable.GetType().GetRuntimeEvent(EventName);
            eventInfo?.RemoveEventHandler(bindable, _eventHandler);
        }

        base.OnDetachingFrom(bindable);
    }

    private void OnEvent(object? sender, object? eventArgs)
    {
        if (Command?.CanExecute(CommandParameter) == true)
        {
            Command.Execute(CommandParameter);
        }
    }
}
