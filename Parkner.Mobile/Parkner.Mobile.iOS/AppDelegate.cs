using Foundation;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfAutoComplete.XForms.iOS;
using Syncfusion.SfBarcode.XForms.iOS;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using Syncfusion.SfCalendar.XForms.iOS;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.SfDataGrid.XForms.iOS;
using Syncfusion.SfImageEditor.XForms.iOS;
using Syncfusion.SfMaps.XForms.iOS;
using Syncfusion.SfNavigationDrawer.XForms.iOS;
using Syncfusion.SfNumericTextBox.XForms.iOS;
using Syncfusion.SfNumericUpDown.XForms.iOS;
using Syncfusion.SfPicker.XForms.iOS;
using Syncfusion.SfRating.XForms.iOS;
using Syncfusion.SfRotator.XForms.iOS;
using Syncfusion.XForms.iOS.Accordion;
using Syncfusion.XForms.iOS.BadgeView;
using Syncfusion.XForms.iOS.Border;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.Cards;
using Syncfusion.XForms.iOS.ComboBox;
using Syncfusion.XForms.iOS.Core;
using Syncfusion.XForms.iOS.EffectsView;
using Syncfusion.XForms.iOS.Expander;
using Syncfusion.XForms.iOS.MaskedEdit;
using Syncfusion.XForms.iOS.PopupLayout;
using Syncfusion.XForms.iOS.Shimmer;
using Syncfusion.XForms.iOS.TabView;
using Syncfusion.XForms.iOS.TextInputLayout;
using Syncfusion.XForms.Pickers.iOS;
using UIKit;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Parkner.Mobile.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.SetFlags("CollectionView_Experimental");
            Forms.Init();
            FormsMaps.Init();

            //SfSignaturePadRenderer.Init();
            SfChartRenderer.Init();
            //SfSunburstChartRenderer.Init();
            SfImageEditorRenderer.Init();
            SfDataGridRenderer.Init();
            SfPickerRenderer.Init();
            //SfPdfDocumentViewRenderer.Init();
            //SfRangeSliderRenderer.Init();
            //SfScheduleRenderer.Init();
            //SfGaugeRenderer.Init();
            //SfDigitalGaugeRenderer.Init();
            //SfLinearGaugeRenderer.Init();
            //SfLinearProgressBarRenderer.Init();
            //SfCircularProgressBarRenderer.Init();
            SfCalendarRenderer.Init();
            //SfCarouselRenderer.Init();
            SfRotatorRenderer.Init();
            SfAutoCompleteRenderer.Init();
            SfBusyIndicatorRenderer.Init();
            SfNavigationDrawerRenderer.Init();
            SfNumericTextBoxRenderer.Init();
            SfNumericUpDownRenderer.Init();
            //SfRadialMenuRenderer.Init();
            SfRatingRenderer.Init();
            SfMapsRenderer.Init();
            //SfTreeMapRenderer.Init();
            //SfPullToRefreshRenderer.Init();
            SfListViewRenderer.Init();
            //SfKanbanRenderer.Init();
            //SfRangeNavigatorRenderer.Init();
            //SfSparklineRenderer.Init();
            SfBarcodeRenderer.Init();
            //SfDataFormRenderer.Init();
            SfMaskedEditRenderer.Init();
            SfPopupLayoutRenderer.Init();
            //SfDiagramRenderer.Init();
            SfTabViewRenderer.Init();
            SfCheckBoxRenderer.Init();
            SfRadioButtonRenderer.Init();
            SfSegmentedControlRenderer.Init();
            SfComboBoxRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            //SfTreeViewRenderer.Init();
            SfButtonRenderer.Init();
            SfBorderRenderer.Init();
            //SfParallaxViewRenderer.Init();
            SfBadgeViewRenderer.Init();
            SfExpanderRenderer.Init();
            SfCardViewRenderer.Init();
            SfCardLayoutRenderer.Init();
            SfAccordionRenderer.Init();
            SfSwitchRenderer.Init();
            //SfRichTextEditorRenderer.Init();
            SfEffectsViewRenderer.Init();
            SfShimmerRenderer.Init();
            SfAvatarViewRenderer.Init();
            SfTimePickerRenderer.Init();
            SfDatePickerRenderer.Init();
            //SfChatRenderer.Init();

            this.LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
