using Libery_Frontend.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Globalization;

namespace Libery_Frontend
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
            CultureInfo sweCulture = new CultureInfo("sv-SE");
            CultureInfo.DefaultThreadCurrentCulture = sweCulture;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
