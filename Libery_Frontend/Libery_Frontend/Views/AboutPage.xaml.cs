using Libery_Frontend.Models;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    #region main page
    public partial class AboutPage : ContentPage
    {
        private MetaStats _timeOnPage = null;
        //populate label with formatting and info regarding opening hours
        public AboutPage()
        {
            InitializeComponent();


            timeLabel.Text =
                           "Öppettider \n\n\n" +
                           "MÅN 10:00-16:00 \n" +
                           "TIS 9:00-18:00 \n" +
                           "ONS 9:00-19:00 \n" +
                           "TOR 8:00-16:00 \n" +
                           "FRE 10:00-17:00 \n" +
                           "LÖR 11:00-15:00 \n" +
                           "SÖN 12:00-15:00";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _timeOnPage = new MetaStats("about", "Hemsidan");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainThread.BeginInvokeOnMainThread(async () => { await _timeOnPage.Finish(); _timeOnPage = null; });
        }


        //Logs out the current user and displays standard layout
        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
            var tab = new MainPage();
            tab.CurrentPage = tab.Children[0];
            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(tab));

        }


        #region library events
        //placeholder images with navigation readily available for redirection to specified events.
        //at this time, this navigation is not implemented due to lack of specifed library events
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await DisplayAlert("Bra klickat", "Du har klickat på en bild", "OK");
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var tab = new MainPage();
            tab.CurrentPage = tab.Children[2];

            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(tab));
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            await DisplayAlert("Bra klickat", "Du har klickat på en bild", "OK");
        }

        private async void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            await DisplayAlert("Bra klickat", "Du har klickat på en bild", "OK");
        }
        #endregion
    }
    #endregion
}