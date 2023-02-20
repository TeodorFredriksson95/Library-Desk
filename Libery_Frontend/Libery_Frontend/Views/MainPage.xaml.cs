using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {
            InitializeComponent();

        }
        async void LoggaUt(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            
            await Navigation.PopAsync();
            var tab = new MainPage();
            tab.CurrentPage = tab.Children[0];
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(tab));
        }
    }
}