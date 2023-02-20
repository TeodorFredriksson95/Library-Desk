using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Libery_Frontend.Models;
using System.Threading;
using Xamarin.Essentials;

#region not actively used
//code is not actively used other than code references. functionality has been updated and moved to different page
namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerAccountSearchPage : ContentPage
    {
        private Models.MetaStats _timeOnPage = null;
        private CancellationTokenSource _tokenSource;
        public CustomerAccountSearchPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _timeOnPage = new Models.MetaStats("search", "Söksidan");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainThread.BeginInvokeOnMainThread(async () => { await _timeOnPage.Finish(); _timeOnPage = null; });
        }

        public int i = 0;

        public async Task<IEnumerable<IGrouping<string, Product>>> SearchProductsAsync(string input)
        {
            Task<IEnumerable<IGrouping<string, Product>>> databaseTask = Task<IEnumerable<IGrouping<string, Product>>>.Factory.StartNew(() =>
            {
                IEnumerable<IGrouping<string, Product>> groupedResult = null;
                try
                {
                    using (var db = new Models.LibraryDBContext())
                    {
                        var query = from product in db.Products
                                    where product.ProductName.ToLower().Contains(input.ToLower())
                                    join prodType in db.ProductTypes on product.ProductType.Id equals prodType.Id

                                    select new
                                    {
                                        ProductType = prodType.Type,
                                        Product = product
                                    };

                        var grouped = from item in query.ToList()
                                      group item.Product by item.ProductType into g
                                      select g;
                        //select new GroupedProducts { ProductType = g.Key, Products = g.ToList() };

                        groupedResult = grouped;
                    }
                }

                catch (Exception ex)
                {
                    // Display modal for error
                }
                return groupedResult;
            }
            );

            var taskResult = await databaseTask;

            return taskResult;
        }

        public async Task Search(String input)
        {

            await Task.Delay(600);

            if (!input.Equals(SearchBarInput.Text))
            {
                return;
            }

            if (!string.IsNullOrEmpty(input))
            {
                SearchListView.BeginRefresh();
                ActivityIndicator.IsRunning = true;
                ActivityIndicator.IsVisible = true;

                var result = await SearchProductsAsync(input);
                SearchListView.ItemsSource = result ?? null;

                ActivityIndicator.IsVisible = false;
                ActivityIndicator.IsRunning = false;
                SearchListView.EndRefresh();
            }
            else
            {
                SearchListView.ItemsSource = null;
            }
        }

        private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = e.NewTextValue;
            await Search(input);

        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string input = SearchBarInput.Text;
            await Search(input);
        }

        private void BookProductButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}
#endregion