using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatsForUser : ContentPage
    {
        private Models.MetaStats _timeOnPage = null;

        public StatsForUser()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            _timeOnPage = new Models.MetaStats("userstats", "Statistiksida (användare)");
            base.OnAppearing();


        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainThread.BeginInvokeOnMainThread(async () => { await _timeOnPage.Finish(); _timeOnPage = null; });
        }

        #region Shows history of products for a user
        public async Task<List<TopProduct>> GetStatsforUser(ActivityIndicator indicator)
        {

            indicator.IsVisible = true;
            indicator.IsRunning = true;

            Task<List<TopProduct>> databaseTask = Task<List<TopProduct>>.Factory.StartNew(() =>
            {
                List<TopProduct> tops = null;
                {

                    using (var db = new Models.LibraryDBContext())
                    {
                        var username = LoginPage.Username;
                        var result = db.OrderDetails.Where(x => x.OrderId == username).ToList();

                        var rest = (from ob in result
                                    join prod in db.Products on ob.ProductId equals prod.Id
                                    select new TopProduct { ProductName = prod.ProductName, ReturnDate = ob.CustomerReturnBooked }).ToList();



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
        #endregion

       
        private async void UserStats_Clicked(object sender, EventArgs e)
        {

            StatsforUser.IsVisible = true;
            MainThread.BeginInvokeOnMainThread(async () => { StatsforUser.ItemsSource = await GetStatsforUser(ActivityIndicator); });
        }
        public class TopProduct
        {
            public int? ProductID { get; set; }
            public int orderCount { get; set; }
            public string ProductName { get; set; }
            public string ProductInfo { get; set; }
            public string Image { get; set; }


            public string Type { get; set; }
            public int? Category { get; set; }
            public string CategoryName { get; set; }
            public string OrderId { get; set; }

            public DateTime? DateBooked { get; set; }
            public DateTime? ReturnDate { get; set; }

        }

        
    }
}