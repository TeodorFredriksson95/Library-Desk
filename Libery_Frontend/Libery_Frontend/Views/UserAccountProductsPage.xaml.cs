using Libery_Frontend.SecondModels;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//THIS PAGE IS NO LONGER ACTIVELY USED BUT IS USED AS
//REFERENCE FOR SOURCE CODE
#region

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserAccountProductsPage : ContentPage
    {
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<ShoppingCart> ShoppingCarts;

        public UserAccountProductsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(
                async () =>
                {
                    ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator);
                }
            );
        }

        public async Task<List<ProductModel>> GetProductsAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(
                () =>
                {
                    List<ProductModel> result = null;
                    try
                    {
                        using (var db = new LibraryDBContext())
                        {
                            Products = db.Products.ToList();
                            ProdType = db.ProductTypes.ToList();

                            result = Products
                                .Join(
                                    ProdType,
                                    p => p.ProductTypeId,
                                    pi => pi.Id,
                                    (p, pi) =>
                                        new ProductModel
                                        {
                                            Image = p.Image,
                                            Name = p.ProductName,
                                            Info = p.ProductInfo,
                                            Type = pi.Type,
                                            ProId = (int)p.Id
                                        }
                                )
                                .ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        // Display modal for error
                    }
                    return result;
                }
            );

            var taskResult = await databaseTask;

            indicator.IsRunning = false;
            indicator.IsVisible = false;

            return taskResult;
        }

        private async void BookProductButton_Clicked(object sender, EventArgs e)
        {
            ShoppingCart cart = new ShoppingCart();
            ProductModel item = ProductListView.SelectedItem as ProductModel;
            DateTime returnDate = DateTime.Now.AddDays(30);
            CultureInfo dateTimeLanguage = CultureInfo.GetCultureInfo("sv-SE");

            if (item != null)
            {
                MainThread.BeginInvokeOnMainThread(
                    async () =>
                    {
                        using (var context = new LibraryDBContext())
                        {
                            cart.ProductId = item.ProId;
                            cart.UserId = LoginPage.Username;
                            cart.DateBooked = DateTime.Now;
                            cart.ReturnDate = DateTime.Now.AddDays(30);

                            ShoppingCarts = context.ShoppingCarts
                                .Where(
                                    x => x.ProductId == item.ProId && x.UserId == LoginPage.Username
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

                                var typeOfProduct = item.Type;
                                await DisplayAlert(
                                    $"{typeOfProduct} lånad",
                                    $"{item.Name} är lånad.\nLämnas tillbaks senast {returnDate.ToString("dddd, MMMM dd, yyyy", dateTimeLanguage)}",
                                    "OK"
                                );
                            }
                        }
                        ProductListView.SelectedItem = null;
                    }
                );
            }
            else
                await DisplayAlert("Produkt ej vald", "Välj en produkt för att låna", "OK");
        }
    }
}
#endregion