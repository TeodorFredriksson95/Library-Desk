using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserInfoPage : ContentPage
    {
        private Models.MetaStats _timeOnPage = null;
        public List<Models.User> users;
        public UserInfoPage()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _timeOnPage = new Models.MetaStats("userpage", "Kontosidan");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainThread.BeginInvokeOnMainThread(async () => { await _timeOnPage.Finish(); _timeOnPage = null; });
        }

        public async Task<List<UserInfo>> GetInfoAboutUser(ActivityIndicator indicator)
        {

            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<UserInfo>> databaseTask = Task<List<UserInfo>>.Factory.StartNew(() =>
            {
                List<UserInfo> result = null;
                {

                    using (var db = new Models.LibraryDBContext())
                    {

                        var username = LoginPage.Username;
                        var userinfo = db.Users.Where(x => x.Username == username).ToList();

                        var rest = (from ob in userinfo

                                    select new UserInfo { UserName = ob.Username, Firstname = ob.Firstname, Lastname = ob.Lastname, Email=ob.Email, PhoneNumber=ob.PhoneNumber }).ToList();
                        return rest;

                    }

                }
            }
            );

            var taskResult = await databaseTask;

            indicator.IsRunning = false;
            indicator.IsVisible = false;

            return taskResult;
        }

        public class UserInfo
        {
            public string UserName { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }    
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
        }

        private async void UserStats_Clicked(object sender, EventArgs e)
        {
            InfoUser.IsVisible = true;
            MainThread.BeginInvokeOnMainThread(async () => { InfoUser.ItemsSource = await GetInfoAboutUser(ActivityIndicator); });

        }
    }
}