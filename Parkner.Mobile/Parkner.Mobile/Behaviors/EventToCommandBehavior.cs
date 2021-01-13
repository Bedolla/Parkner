using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace Parkner.Mobile.Behaviors
{
    public class EventToCommandBehavior : BehaviorBase<VisualElement>
    {
        public static readonly BindableProperty EventNameProperty = BindableProperty.Create("EventName", typeof(string), typeof(EventToCommandBehavior), null, propertyChanged: EventToCommandBehavior.OnEventNameChanged);
        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(EventToCommandBehavior));
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create("CommandParameter", typeof(object), typeof(EventToCommandBehavior));
        public static readonly BindableProperty InputConverterProperty = BindableProperty.Create("Converter", typeof(IValueConverter), typeof(EventToCommandBehavior));
        private Delegate _eventHandler;

        public string EventName
        {
            get => (string)this.GetValue(EventToCommandBehavior.EventNameProperty);
            set => this.SetValue(EventToCommandBehavior.EventNameProperty, value);
        }

        public ICommand Command
        {
            get => (ICommand)this.GetValue(EventToCommandBehavior.CommandProperty);
            set => this.SetValue(EventToCommandBehavior.CommandProperty, value);
        }

        public object CommandParameter
        {
            get => this.GetValue(EventToCommandBehavior.CommandParameterProperty);
            set => this.SetValue(EventToCommandBehavior.CommandParameterProperty, value);
        }

        public IValueConverter Converter
        {
            get => (IValueConverter)this.GetValue(EventToCommandBehavior.InputConverterProperty);
            set => this.SetValue(EventToCommandBehavior.InputConverterProperty, value);
        }

        protected override void OnAttachedTo(VisualElement bindable)
        {
            base.OnAttachedTo(bindable);
            this.RegisterEvent(this.EventName);
        }

        protected override void OnDetachingFrom(VisualElement bindable)
        {
            this.DeregisterEvent(this.EventName);
            base.OnDetachingFrom(bindable);
        }

        private void RegisterEvent(string name)
        {
            if (String.IsNullOrWhiteSpace(name)) return;

            EventInfo eventInfo = this.AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null) throw new ArgumentException(String.Format("EventToCommandBehavior: Can't register the '{0}' event.", this.EventName));
            MethodInfo methodInfo = typeof(EventToCommandBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
            this._eventHandler = methodInfo.CreateDelegate(eventInfo.EventHandlerType, this);
            eventInfo.AddEventHandler(this.AssociatedObject, this._eventHandler);
        }

        private void DeregisterEvent(string name)
        {
            if (String.IsNullOrWhiteSpace(name)) return;

            if (this._eventHandler == null) return;
            EventInfo eventInfo = this.AssociatedObject.GetType().GetRuntimeEvent(name);
            if (eventInfo == null) throw new ArgumentException(String.Format("EventToCommandBehavior: Can't de-register the '{0}' event.", this.EventName));
            eventInfo.RemoveEventHandler(this.AssociatedObject, this._eventHandler);
            this._eventHandler = null;
        }

        private void OnEvent(object sender, object eventArgs)
        {
            if (this.Command == null) return;

            object resolvedParameter;
            if (this.CommandParameter != null)
                resolvedParameter = this.CommandParameter;
            else if (this.Converter != null)
                resolvedParameter = this.Converter.Convert(eventArgs, typeof(object), null, null);
            else
                resolvedParameter = eventArgs;

            if (this.Command.CanExecute(resolvedParameter)) this.Command.Execute(resolvedParameter);
        }

        private static void OnEventNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            EventToCommandBehavior behavior = (EventToCommandBehavior)bindable;
            if (behavior.AssociatedObject == null) return;

            string oldEventName = (string)oldValue;
            string newEventName = (string)newValue;

            behavior.DeregisterEvent(oldEventName);
            behavior.RegisterEvent(newEventName);
        }
    }
}
