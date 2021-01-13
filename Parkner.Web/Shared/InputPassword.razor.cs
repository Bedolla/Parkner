using Microsoft.AspNetCore.Components;
using System;

namespace Parkner.Web.Shared
{
    public partial class InputPassword
    {
        private string _value;

        private bool Mostrar { get; set; }

        [Parameter]
        public string PlaceHolder { get; set; }

        [Parameter]
        public string Id { get; set; }

        [Parameter]
        public string Value
        {
            get => this._value;
            set
            {
                if (this._value == value) return;
                this._value = value;
                this.ValueChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public EventCallback<string> ValueChanged { get; set; }
    }
}
