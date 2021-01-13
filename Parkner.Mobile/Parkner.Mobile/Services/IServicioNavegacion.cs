using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Parkner.Mobile.Services
{
    public interface INavigationService
    {
        string CurrentPageKey { get; }

        void Configure(string pageKey, Type pageType);
        Task GoBack();
        Task NavigateModalAsync(string pageKey, bool animated = true);
        Task NavigateModalAsync(string pageKey, object parameter, bool animated = true);
        Task NavigateAsync(string pageKey, bool animated = true);
        Task NavigateAsync(string pageKey, object parameter, bool animated = true);
    }

    public class ViewNavigationService : INavigationService
    {
        private readonly Stack<NavigationPage> _navigationPageStack =
            new Stack<NavigationPage>();

        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private readonly object _sync = new object();
        private NavigationPage CurrentNavigationPage => this._navigationPageStack.Peek();

        public void Configure(string pageKey, Type pageType)
        {
            lock (this._sync)
            {
                if (this._pagesByKey.ContainsKey(pageKey))
                    this._pagesByKey[pageKey] = pageType;
                else
                    this._pagesByKey.Add(pageKey, pageType);
            }
        }

        public string CurrentPageKey
        {
            get
            {
                lock (this._sync)
                {
                    if (this.CurrentNavigationPage?.CurrentPage == null) return null;

                    Type pageType = this.CurrentNavigationPage.CurrentPage.GetType();

                    return this._pagesByKey.ContainsValue(pageType)
                        ? this._pagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        public async Task GoBack()
        {
            INavigation navigationStack = this.CurrentNavigationPage.Navigation;
            if (navigationStack.NavigationStack.Count > 1)
            {
                await this.CurrentNavigationPage.PopAsync();
                return;
            }

            if (this._navigationPageStack.Count > 1)
            {
                this._navigationPageStack.Pop();
                await this.CurrentNavigationPage.Navigation.PopModalAsync();
                return;
            }

            await this.CurrentNavigationPage.PopAsync();
        }

        public async Task NavigateModalAsync(string pageKey, bool animated = true)
        {
            await this.NavigateModalAsync(pageKey, null, animated);
        }

        public async Task NavigateModalAsync(string pageKey, object parameter, bool animated = true)
        {
            Page page = this.GetPage(pageKey, parameter);
            NavigationPage.SetHasNavigationBar(page, false);
            NavigationPage modalNavigationPage = new NavigationPage(page);
            await this.CurrentNavigationPage.Navigation.PushModalAsync(modalNavigationPage, animated);
            this._navigationPageStack.Push(modalNavigationPage);
        }

        public async Task NavigateAsync(string pageKey, bool animated = true)
        {
            await this.NavigateAsync(pageKey, null, animated);
        }

        public async Task NavigateAsync(string pageKey, object parameter, bool animated = true)
        {
            Page page = this.GetPage(pageKey, parameter);
            await this.CurrentNavigationPage.Navigation.PushAsync(page, animated);
        }

        public Page SetRootPage(string rootPageKey)
        {
            Page rootPage = this.GetPage(rootPageKey);
            this._navigationPageStack.Clear();
            NavigationPage mainPage = new NavigationPage(rootPage);
            this._navigationPageStack.Push(mainPage);
            return mainPage;
        }

        private Page GetPage(string pageKey, object parameter = null)
        {
            lock (this._sync)
            {
                if (!this._pagesByKey.ContainsKey(pageKey))
                    throw new ArgumentException(
                        $"No such page: {pageKey}. Did you forget to call NavigationService.Configure?");

                Type type = this._pagesByKey[pageKey];
                ConstructorInfo constructor;
                object[] parameters;

                if (parameter == null)
                {
                    constructor = type.GetTypeInfo()
                                      .DeclaredConstructors
                                      .FirstOrDefault(c => !c.GetParameters().Any());

                    parameters = new object[]
                        { };
                }
                else
                {
                    constructor = type.GetTypeInfo()
                                      .DeclaredConstructors
                                      .FirstOrDefault(
                                          c =>
                                          {
                                              ParameterInfo[] p = c.GetParameters();
                                              return (p.Length == 1)
                                                     && (p[0].ParameterType == parameter.GetType());
                                          });

                    parameters = new[]
                    {
                        parameter
                    };
                }

                if (constructor == null)
                    throw new InvalidOperationException(
                        "No suitable constructor found for page " + pageKey);

                Page page = constructor.Invoke(parameters) as Page;
                return page;
            }
        }
    }
}
