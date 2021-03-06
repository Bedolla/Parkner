﻿using Parkner.Mobile.Models;
using Syncfusion.XForms.ComboBox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Parkner.Mobile.Controls
{
    [Preserve(AllMembers = true)]
    public class CountryComboBox : SfComboBox, INotifyPropertyChanged
    {
        #region Constructor
        public CountryComboBox()
        {
            this.BindingContext = this;

            this.Countries = new List<CountryModel>
            {
                new CountryModel {Country = "Australia", States = new[] {"Tasmania", "Victoria", "Queensland", "Northen Territory"}},
                new CountryModel {Country = "Brazil", States = new[] {"Bahia", "Ceara", "Goias", "Maranhao"}},
                new CountryModel {Country = "Canada", States = new[] {"Manitoba", "Ontario", "Quebec", "Yukon"}},
                new CountryModel {Country = "India", States = new[] {"Assam", "Gujarat", "Haryana", "Tamil Nadu"}},
                new CountryModel {Country = "USA", States = new[] {"California", "Florida", "New York", "Washington"}}
            };

            this.DataSource = this.Countries;
            this.SetBinding(SfComboBox.SelectedItemProperty, "Country", BindingMode.TwoWay);
            this.DisplayMemberPath = "Country";
            this.Watermark = "Select Country";
            this.VerticalOptions = LayoutOptions.CenterAndExpand;
        }
        #endregion

        #region Fields
        private object country;

        private string phoneNumberPlaceHolder = "Phone Number";

        private string phoneNumber;

        private string city;

        private object state;

        private string[] states;

        private string countryCode;
        #endregion

        #region Properties
        /// <summary>
        ///     Gets or sets the property that bounds with a ComboBox that gets the Country from user.
        /// </summary>
        public object Country
        {
            get => this.country;
            set
            {
                this.country = value;
                this.UpdateStateAndPhoneNumberFormat();
            }
        }

        /// <summary>
        ///     Gets or set the string property, that represents the phone number format based on country.
        /// </summary>
        public string PhoneNumberPlaceHolder
        {
            get => this.phoneNumberPlaceHolder;
            set
            {
                this.phoneNumberPlaceHolder = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or set the string property, that represents the phone number based on country.
        /// </summary>
        public string PhoneNumber
        {
            get => this.phoneNumber;
            set
            {
                this.phoneNumber = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the string property, which holds the contry code based on user input.
        /// </summary>
        public string CountryCode
        {
            get => this.countryCode;
            set
            {
                this.countryCode = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or set the string property, that represents the user given city.
        /// </summary>
        public string City
        {
            get => this.city;
            set
            {
                this.city = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the property that bounds with a ComboBox that gets the State from user.
        /// </summary>
        public object State
        {
            get => this.state;
            set
            {
                this.state = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets array collection that contains states based on country selection.
        /// </summary>
        public string[] States
        {
            get => this.states;
            set
            {
                this.states = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets string property that represents mask format for phone number.
        /// </summary>
        private string _mask = "";

        public string Mask
        {
            get => this._mask;
            set
            {
                this._mask = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the collection property, which contains the countries data.
        /// </summary>
        public List<CountryModel> Countries { get; set; }
        #endregion

        #region Methods
        /// <summary>
        ///     Method used to rest State and City and update PhoneNumber format.
        /// </summary>
        private void UpdateStateAndPhoneNumberFormat()
        {
            this.State = null;
            this.PhoneNumber = String.Empty;
            this.City = String.Empty;
            CountryModel countryModel = this.Country as CountryModel;
            this.States = countryModel.States;

            switch (countryModel.Country)
            {
                case "Australia":
                    this.PhoneNumberPlaceHolder = "e.g. X XXXX XXXX";
                    this.Mask = "(+61)X XXXX XXXX";
                    this.CountryCode = "(+61)";
                    break;
                case "Brazil":
                    this.PhoneNumberPlaceHolder = "e.g. XX XXXX XXXX";
                    this.Mask = "(+55)XX XXXX XXXX";
                    this.CountryCode = "(+55)";
                    break;
                case "Canada":
                    this.PhoneNumberPlaceHolder = "e.g. XXXXXXXXX";
                    this.Mask = "(+1)XXXXXXXXX";
                    this.CountryCode = "(+1)";
                    break;
                case "India":
                    this.PhoneNumberPlaceHolder = "e.g. XXXXX-XXXXX";
                    this.Mask = "(+91)XXXXX-XXXXX";
                    this.CountryCode = "(+91)";
                    break;
                case "USA":
                    this.PhoneNumberPlaceHolder = "e.g. XXX-XXX-XXX";
                    this.Mask = "(+1)XXX-XXX-XXX";
                    this.CountryCode = "(+1)";
                    break;
            }
        }
        
        #endregion
    }
}
