using Syncfusion.ListView.XForms.UWP;
using Syncfusion.SfAutoComplete.XForms.UWP;
using Syncfusion.SfBarcode.XForms.UWP;
using Syncfusion.SfBusyIndicator.XForms.UWP;
using Syncfusion.SfCalendar.XForms.UWP;
using Syncfusion.SfChart.XForms.UWP;
using Syncfusion.SfDataGrid.XForms.UWP;
using Syncfusion.SfImageEditor.XForms.UWP;
using Syncfusion.SfMaps.XForms.UWP;
using Syncfusion.SfNavigationDrawer.XForms.UWP;
using Syncfusion.SfNumericTextBox.XForms.UWP;
using Syncfusion.SfNumericUpDown.XForms.UWP;
using Syncfusion.SfPicker.XForms.UWP;
using Syncfusion.SfPullToRefresh.XForms.UWP;
using Syncfusion.SfRating.XForms.UWP;
using Syncfusion.SfRotator.XForms.UWP;
using Syncfusion.XForms.UWP.Accordion;
using Syncfusion.XForms.UWP.BadgeView;
using Syncfusion.XForms.UWP.Border;
using Syncfusion.XForms.UWP.Buttons;
using Syncfusion.XForms.UWP.Cards;
using Syncfusion.XForms.UWP.ComboBox;
using Syncfusion.XForms.UWP.EffectsView;
using Syncfusion.XForms.UWP.Expander;
using Syncfusion.XForms.UWP.MaskedEdit;
using Syncfusion.XForms.UWP.PopupLayout;
using Syncfusion.XForms.UWP.Shimmer;
using Syncfusion.XForms.UWP.TabView;
using Syncfusion.XForms.UWP.TextInputLayout;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using Xamarin.Forms;
using Application = Windows.UI.Xaml.Application;
using Frame = Windows.UI.Xaml.Controls.Frame;
using UnhandledExceptionEventArgs = Windows.UI.Xaml.UnhandledExceptionEventArgs;

namespace Parkner.Mobile.UWP
{
    public sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            this.UnhandledException += this.OnUnhandledException;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            if (!(Window.Current.Content is Frame rootFrame))
            {
                rootFrame = new Frame();
                rootFrame.NavigationFailed += this.OnNavigationFailed;

                List<Assembly> assembliesToInclude = new List<Assembly>
                {
                    typeof(SfShimmerRenderer).GetTypeInfo().Assembly,
                    typeof(SfEffectsViewRenderer).GetTypeInfo().Assembly,
                    //typeof(SfChatRenderer).GetTypeInfo().Assembly,
                    //typeof(SfRichTextEditorRenderer).GetTypeInfo().Assembly,
                    typeof(SfAccordionRenderer).GetTypeInfo().Assembly,
                    typeof(SfExpanderRenderer).GetTypeInfo().Assembly,
                    typeof(SfCardLayoutRenderer).GetTypeInfo().Assembly,
                    typeof(SfCardViewRenderer).GetTypeInfo().Assembly,
                    typeof(SfBadgeViewRenderer).GetTypeInfo().Assembly,
                    //typeof(SfParallaxViewRenderer).GetTypeInfo().Assembly,
                    typeof(SfBorderRenderer).GetTypeInfo().Assembly,
                    typeof(SfButtonRenderer).GetTypeInfo().Assembly,
                    //typeof(SfTreeViewRenderer).GetTypeInfo().Assembly,
                    typeof(SfTextInputLayoutRenderer).GetTypeInfo().Assembly,
                    typeof(SfSegmentedControlRenderer).GetTypeInfo().Assembly,
                    typeof(SfComboBoxRenderer).GetTypeInfo().Assembly,
                    typeof(SfRadioButtonRenderer).GetTypeInfo().Assembly,
                    typeof(SfCheckBoxRenderer).GetTypeInfo().Assembly,
                    typeof(SfTabViewRenderer).GetTypeInfo().Assembly,
                    //typeof(SfDiagramRenderer).GetTypeInfo().Assembly,
                    typeof(SfPopupLayoutRenderer).GetTypeInfo().Assembly,
                    typeof(SfMaskedEditRenderer).GetTypeInfo().Assembly,
                    //typeof(SfDataFormRenderer).GetTypeInfo().Assembly,
                    typeof(SfBarcodeRenderer).GetTypeInfo().Assembly,
                    //typeof(SfSparklineRenderer).GetTypeInfo().Assembly,
                    //typeof(SfRangeNavigatorRenderer).GetTypeInfo().Assembly,
                    //typeof(SfKanbanRenderer).GetTypeInfo().Assembly,
                    typeof(SfListViewRenderer).GetTypeInfo().Assembly,
                    typeof(SfPullToRefreshRenderer).GetTypeInfo().Assembly,
                    //typeof(SfTreeMapRenderer).GetTypeInfo().Assembly,
                    typeof(SfMapsRenderer).GetTypeInfo().Assembly,
                    typeof(SfRatingRenderer).GetTypeInfo().Assembly,
                    //typeof(SfRadialMenuRenderer).GetTypeInfo().Assembly,
                    typeof(SfNumericUpDownRenderer).GetTypeInfo().Assembly,
                    typeof(SfNumericTextBoxRenderer).GetTypeInfo().Assembly,
                    typeof(SfNavigationDrawerRenderer).GetTypeInfo().Assembly,
                    typeof(SfBusyIndicatorRenderer).GetTypeInfo().Assembly,
                    typeof(SfAutoCompleteRenderer).GetTypeInfo().Assembly,
                    typeof(SfRotatorRenderer).GetTypeInfo().Assembly,
                    //typeof(SfCarouselRenderer).GetTypeInfo().Assembly,
                    typeof(SfCalendarRenderer).GetTypeInfo().Assembly,
                    //typeof(SfCircularProgressBarRenderer).GetTypeInfo().Assembly,
                    //typeof(SfLinearProgressRenderer).GetTypeInfo().Assembly,
                    //typeof(SfLinearGaugeRenderer).GetTypeInfo().Assembly,
                    //typeof(SfDigitalGaugeRenderer).GetTypeInfo().Assembly,
                    //typeof(SfGaugeRenderer).GetTypeInfo().Assembly,
                    //typeof(SfScheduleRenderer).GetTypeInfo().Assembly,
                    //typeof(SfRangeSliderRenderer).GetTypeInfo().Assembly,
                    //typeof(SfPdfDocumentViewRenderer).GetTypeInfo().Assembly,
                    typeof(SfPickerRenderer).GetTypeInfo().Assembly,
                    typeof(SfDataGridRenderer).GetTypeInfo().Assembly,
                    typeof(SfImageEditorRenderer).GetTypeInfo().Assembly,
                    //typeof(SfSunburstChartRenderer).GetTypeInfo().Assembly,
                    typeof(SfChartRenderer).GetTypeInfo().Assembly
                };

                Forms.SetFlags("CollectionView_Experimental");

                Forms.Init(e, assembliesToInclude);

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null) rootFrame.Navigate(typeof(MainPage), e.Arguments);

            Window.Current.Activate();
        }

        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

            // Save application state and stop any background activity

            deferral.Complete();
        }

        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.Message);
        }
    }
}
