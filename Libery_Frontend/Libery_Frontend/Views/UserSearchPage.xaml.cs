using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using System.Globalization;
using Xamarin.Essentials;
using Libery_Frontend.SecondModels;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserSearchPage : ContentPage
    {
        private Models.MetaStats _timeOnPage = null;
        private CancellationTokenSource _tokenSource;
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<ShoppingCart> ShoppingCarts;

        public UserSearchPage()
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

        //SEARCH FUNCTIONS
        #region

        public int i = 0;

        //Query available search items
        public async Task<IEnumerable<IGrouping<string, Product>>> SearchProductsAsync(string input)
        {
            Task<IEnumerable<IGrouping<string, Product>>> databaseTask = Task<IEnumerable<IGrouping<string, Product>>>.Factory.StartNew(
                () =>
                {
                    IEnumerable<IGrouping<string, Product>> groupedResult = null;
                    try
                    {
                        using (var db = new LibraryDBContext())
                        {
                            var query =
                                from product in db.Products
                                where product.ProductName.ToLower().Contains(input.ToLower())
                                join prodType in db.ProductTypes
                                    on product.ProductType.Id equals prodType.Id
                                select new { ProductType = prodType.Type, Product = product };

                            var grouped =
                                from item in query.ToList()
                                group item.Product by item.ProductType into g
                                select g;

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

        //Display items from search query that matches search input
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

        //asynchronously check for changes in the search field and display results corresponding to matching items
        private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = e.NewTextValue;
            await Search(input);
        }

        // same functionality as above but only issued when the search icon is actually clicked
        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            string input = SearchBarInput.Text;
            await Search(input);
        }
        #endregion


        //BOOK PRODUCT FUNCTIONS
        #region

        //If item is bookable (true/false) and book button is clicked, insert info from the buttons binding context into Shoppingcart table
        //and connect it to the user currently logged in (Database)
        //
        //If user is not logged in, prompt user to login before booking item. If admin is logged in, no button is showed.
        private async void BookProductButton_Clicked_1(object sender, EventArgs e)
        {
            ShoppingCart cart = new ShoppingCart();
            DateTime returnDate = DateTime.Now.AddDays(30);
            CultureInfo dateTimeLanguage = CultureInfo.GetCultureInfo("sv-SE");

            Button btn = sender as Button;
            Product item = btn.BindingContext as Product;

            if (item != null)
            {
                MainThread.BeginInvokeOnMainThread(
                    async () =>
                    {
                        using (var context = new LibraryDBContext())
                        {
                            ProdType = context.ProductTypes.ToList();
                            Products = context.Products.ToList();

                            cart.ProductId = item.Id;
                            cart.UserId = LoginPage.Username;
                            cart.DateBooked = DateTime.Now;
                            cart.ReturnDate = DateTime.Now.AddDays(30);

                            ShoppingCarts = context.ShoppingCarts
                                .Where(
                                    x => x.ProductId == item.Id && x.UserId == LoginPage.Username
                                )
                                .ToList();

                            if (ShoppingCarts.Any())
                            {
                                await DisplayAlert(
                                    "Redan lånad",
                                    "Du har redan lånat denna produkt",
                                    "OK"
                                );
                            }
                            else
                            {
                                context.Add(cart);
                                context.SaveChanges();

                                var typeOfProduct = context.ProductTypes
                                    .Where(x => x.Id == item.ProductTypeId)
                                    .FirstOrDefault();
                                var ProdTypeName = new ProductType { Type = typeOfProduct.Type };

                                await DisplayAlert(
                                    $"{ProdTypeName.Type} lånad",
                                    $"{item.ProductName} är lånad.\nLämnas tillbaks senast {returnDate.ToString("dddd, MMMM dd, yyyy", dateTimeLanguage)}",
                                    "OK"
                                );
                            }
                        }
                        SearchListView.SelectedItem = null;
                    }
                );
            }
            else
                await DisplayAlert("Produkt ej vald", "Välj en produkt för att låna", "OK");
        }
        #endregion
    }
}
