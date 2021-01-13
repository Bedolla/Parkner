using Syncfusion.ListView.XForms.UWP;
using Syncfusion.SfDataGrid.XForms.UWP;
using Syncfusion.SfPullToRefresh.XForms.UWP;
using Syncfusion.XForms.UWP.PopupLayout;
using Windows.Services.Maps;
using Xamarin;

namespace Parkner.Mobile.UWP
{
    public sealed partial class MainPage
    {
        private const string Token = "TokenDeBingMaps";

        public MainPage()
        {
            this.InitializeComponent();

            SfPopupLayoutRenderer.Init();
            SfListViewRenderer.Init();
            SfPullToRefreshRenderer.Init();
            SfDataGridRenderer.Init();

            FormsMaps.Init(MainPage.Token);
            MapService.ServiceToken = MainPage.Token;

            this.LoadApplication(new Mobile.App());
        }
    }
}
