using Syncfusion.XForms.ComboBox;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using SelectionChangedEventArgs = Syncfusion.XForms.ComboBox.SelectionChangedEventArgs;

namespace Parkner.Mobile.Behaviors
{
    [Preserve(AllMembers = true)]
    public class SelectedIndexBehavior : Behavior<SfComboBox>
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create
        (
            nameof(SelectedIndexBehavior.Command),
            typeof(ICommand),
            typeof(SelectedIndexBehavior)
        );

        public ICommand Command
        {
            get => (ICommand)this.GetValue(SelectedIndexBehavior.CommandProperty);
            set => this.SetValue(SelectedIndexBehavior.CommandProperty, value);
        }

        public SfComboBox ComboBox { get; private set; }

        protected override void OnAttachedTo(SfComboBox comboBox)
        {
            if (comboBox != null)
            {
                base.OnAttachedTo(comboBox);
                this.ComboBox = comboBox;
                comboBox.BindingContextChanged += this.OnBindingContextChanged;
                comboBox.SelectionChanged += this.ComboBox_SelectionChanged;
            }
        }

        protected override void OnDetachingFrom(SfComboBox comboBox)
        {
            if (comboBox != null)
            {
                base.OnDetachingFrom(comboBox);
                comboBox.BindingContextChanged -= this.OnBindingContextChanged;
                comboBox.SelectionChanged -= this.ComboBox_SelectionChanged;
                this.ComboBox = null;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.BindingContext = this.ComboBox.BindingContext;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.Command == null) return;

            if (this.Command.CanExecute(e.Value)) this.Command.Execute(e.Value);
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            this.OnBindingContextChanged();
        }
    }
}
