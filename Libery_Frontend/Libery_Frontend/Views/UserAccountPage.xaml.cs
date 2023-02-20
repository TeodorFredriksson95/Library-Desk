using Libery_Frontend.SecondModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserAccountPage : ContentPage
    {
        private Models.MetaStats _timeOnPage = null;
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<User> Users;
        public List<ShoppingCart> ShoppingCarts;

        public UserAccountPage(string username)
        {
            InitializeComponent();
            BindingContext = username;
        }

        public UserAccountPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _timeOnPage = new Models.MetaStats("userinfo", "Konto infosidan");
            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(
                async () =>
                {
                    ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator);
                    ListViewEmptyLabel.Text = await GetUserProductList();
                }
            );
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainThread.BeginInvokeOnMainThread(async () => { await _timeOnPage.Finish(); _timeOnPage = null; });
        }


        //Code to change label text depending on wether or not the user has an empty shopping cart or not.
        public async Task<string> GetUserProductList()
        {
            List<shoppingCartTestModel> userCart = null;

            userCart = await GetProductsAsync(ActivityIndicator);

            if (userCart.Count == 0)
                return $"Du har inga lånade produkter";
            else
                return $"Dina lånade produkter";
        }


        //Returns a list of products connected to the specific logged in user
        public async Task<List<shoppingCartTestModel>> GetProductsAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            CultureInfo dateTimeLanguage = CultureInfo.GetCultureInfo("sv-SE");

            Task<List<shoppingCartTestModel>> databaseTask = Task<
                List<shoppingCartTestModel>
            >.Factory.StartNew(
                () =>
                {
                    List<shoppingCartTestModel> cartResultone = null;
                    using (var db = new LibraryDBContext())
                    {
                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();
                        Users = db.Users.ToList();
                        ShoppingCarts = db.ShoppingCarts.ToList();


                    cartResultone = ShoppingCarts.Join(Products, p => p.ProductId, pi => pi.Id, (p, pi) => new shoppingCartTestModel
                    {
                        AccountName = p?.UserId,
                        Image = pi.Image ?? "Ingen bild",
                        ID = p?.Id,
                        UnitPrice = pi.Price,
                        ProductID = pi?.Id,
                        AuthorID = pi?.AuthorId,
                        ISBN = pi.Isbn ?? "Inget ISBN",
                        ProductTypeID = pi?.ProductTypeId,
                        ProductName = pi.ProductName ?? "Ingen titel",
                        CategoryID = pi?.CategoryId,
                        ProductInfo = pi.ProductInfo,
                        ReleaseDate = pi?.ReleaseDate,
                        DateBooked = p?.DateBooked, 
                        ReturnDate = (DateTime)p.ReturnDate 
                    }).Where(x => x.AccountName == LoginPage.Username).ToList();

                    cartResultone = cartResultone.Join(ProdType, p => p.ProductTypeID, pi => pi.Id, (p, pi) => new shoppingCartTestModel
                    {
                        AccountName = p.AccountName,
                        Image = p.Image,
                        UnitPrice = p?.UnitPrice,
                        ID = p?.ID,
                        ProductID = p?.ProductID,
                        AuthorID = p?.AuthorID,
                        ISBN = p.ISBN,
                        ProductTypeID = p?.ProductTypeID,
                        ProductName = p.ProductName,
                        CategoryID = p?.CategoryID,
                        ProductInfo = p.ProductInfo,
                        ReleaseDate = p?.ReleaseDate,
                        DateBooked = p?.DateBooked,
                        ReturnDate = p?.ReturnDate,
                        prodType = pi.Type
                    }).ToList();


                }

                    return cartResultone;
                }
            );

            var taskResult = await databaseTask;

            indicator.IsRunning = false;
            indicator.IsVisible = false;

            return taskResult;
        }

        #region Method to return a product
        //Method to insert product and user info into orders table (database) once the user has atively returned an item
        private async void InsertIntoOrderButton_Clicked(object sender, EventArgs e)
        {
            ShoppingCart cartToRemove;
            OrderDetail cart = new OrderDetail();

            Button btn = sender as Button;
            shoppingCartTestModel item = btn.BindingContext as shoppingCartTestModel;

            if (item != null)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    using (var context = new LibraryDBContext())
                    {

                        cart.ProductId = item.ProductID;
                        cart.OrderId = LoginPage.Username;
                        cart.UnitPrice = item.UnitPrice;
                        cart.CustomerDateBooked = item.DateBooked;
                        cart.CustomerReturnBooked = DateTime.Now;

                            var orderList = context.Users
                                .Where(x => x.Username == LoginPage.Username)
                                .FirstOrDefault();

                            var order = new Order
                            {
                                CustomerUsername = LoginPage.Username,
                                CustomerId = orderList.Id,
                                CustomerAddress = orderList.Address,
                                CustomerPostalCode = orderList.PostalCode,
                                CustomerCity = orderList.City
                            };

                            cartToRemove = context.ShoppingCarts
                                .Where(x => x.Id == item.ID)
                                .FirstOrDefault();

                            context.Remove(cartToRemove);
                            context.Add(order);
                            context.Add(cart);
                            context.SaveChanges();

                            ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator);
                            ListViewEmptyLabel.Text = await GetUserProductList();
                        }
                        ProductListView.SelectedItem = null;
                    }
                );

                var typeOfProduct = item.prodType;
                await DisplayAlert($"{typeOfProduct} återlämnad",
                    $"{item.ProductName} är återlämnad.\nTack!", "Gå vidare");


            }
            else
                await DisplayAlert(
                    "Produkt ej vald",
                    "Välj en produkt för att lämna tillbaka",
                    "OK"
                );
        }
        #endregion

        #region visual elements
        //Below methods dictate which elements become visible to user and which elements
        //that shall remain hidden. The method is based on tap gestures on phones/tablets
        //and click motions on laptops/desktops
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            baba1.IsVisible = true;
            baba2.IsVisible = true;
            baba3.IsVisible = true;
            baba4.IsVisible = true;
            baba5.IsVisible = true;
            baba6.IsVisible = true;

            trollpic.IsVisible = false;
            lolpic.IsVisible = true;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            baba1.IsVisible = false;
            baba2.IsVisible = false;
            baba3.IsVisible = false;
            baba4.IsVisible = false;
            baba5.IsVisible = false;
            baba6.IsVisible = false;

            trollpic.IsVisible = true;
            lolpic.IsVisible = false;
        }
        #endregion
        #region Method to extend returndate on a product
        private async void ExtendButton_Clicked(object sender, EventArgs e)
        {
            ShoppingCart cart = new ShoppingCart();


            Button btn = sender as Button;
            shoppingCartTestModel item = btn.BindingContext as shoppingCartTestModel;
            

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                using (var context = new LibraryDBContext())
                {

                    cart.ProductId = item.ProductID;
                    cart.UserId = LoginPage.Username;
                    cart.DateBooked = item.DateBooked;
                    cart.ReturnDate = item.ReturnDate;

                    cart = context.ShoppingCarts.Where(x => x.UserId == LoginPage.Username && x.ProductId == item.ProductID).ToList().FirstOrDefault();

                    cart.ReturnDate = item.ReturnDate.Value.AddDays(30);
                  

                    context.Update(cart);
                    context.SaveChanges();

                    var typeOfProduct = item.ProductName;
                    await DisplayAlert($"{typeOfProduct} förlängd",
                        $"{item.ProductName} är förlängd.\nLämnas tillbaks senast {cart.ReturnDate}", "OK");

                    ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator);
                }
            });

        }
    }
    #endregion


    //Class to model a shoppingcart
    public class shoppingCartTestModel
    {
        public int? ID { get; set; } = default;
        public string Image { get; set; } = default;
        public double? UnitPrice { get; set; }
        public int? AuthorID { get; set; } = default;
        public string ISBN { get; set; } = default;
        public int? ProductTypeID { get; set; } = default;
        public int? CategoryID { get; set; } = default;
        public string ProductName { get; set; } = default;
        public string ProductInfo { get; set; } = default;
        public DateTime? ReleaseDate { get; set; } = default;
        public DateTime? DateBooked { get; set; } = default;
        public DateTime? ReturnDate { get; set; } = default;
        public string UserID { get; set; } = default;
        public int? ProductID { get; set; } = default;
        public string AccountName { get; set; }
        public string prodType { get; set; }
    }

    //Class to model a shoppingcart based on logged in user
    public class UserShoppingCartModel
    {
        public string OrderID { get; set; }
        public int ProductId { get; set; }
        public double UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
