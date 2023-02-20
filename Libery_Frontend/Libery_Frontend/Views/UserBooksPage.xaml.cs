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

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserBooksPage : ContentPage
    {
        private Models.MetaStats _timeOnPage = null;
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<ShoppingCart> ShoppingCarts;
        public List<ProductCategory> Category;
        public List<Author> autName;
        public List<Director> dirName;

        public UserBooksPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _timeOnPage = new Models.MetaStats("products", "Produkt sidan");
            // Load products asynchronously

            // Load two separate listviews with books and movies respectively
            MainThread.BeginInvokeOnMainThread(async () => { ProductListView.ItemsSource = await GetBooksAsync(); });
            MainThread.BeginInvokeOnMainThread(async () => { EbooksListview.ItemsSource = await GetMoviesAsync(); });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainThread.BeginInvokeOnMainThread(async () => { await _timeOnPage.Finish(); _timeOnPage = null; });
        }

        //GET BOOKS FUNCTIONS
        public async Task<List<ProductModel>> GetBooksAsync()
        {

            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                List<ProductModel> catRes = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        Category = db.ProductCategories.ToList();
                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();
                        autName = db.Authors.ToList();

                        //Products table joined with Producttypes table
                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.ProductName,
                            Info = p.ProductInfo,
                            Type = pi.Type,
                            ProId = p.Id,
                            InfoConcat = p.ProductInfo,
                            Pages = p.BookPages,
                            AuthorID = p.AuthorId,
                            DirectorID = p.DirectorId,
                            CategoryID = p.CategoryId,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.Price,
                            ISBN = p.Isbn,
                            IsBookable = p.IsBookable
                        }).ToList();

                        //Result of products + producttype join extended with second join.
                        //Result joined with Category table
                        result = result.Join(Category, pi => pi.CategoryID, p => p.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.Name,
                            Info = p.Info,
                            InfoConcat = p.Info,
                            Type = p.Type,
                            ProId = p.ProId,
                            Pages = p.Pages,
                            AuthorID = p.AuthorID,
                            CategoryID = p.CategoryID,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.UnitPrice,
                            ISBN = p.ISBN,
                            IsBookable = p.IsBookable,
                            Category = pi.Category,
                            DirectorID = p.DirectorID
                        }).ToList();


                        //Result of previous joins extended with third join.
                        //Result joined with Authors table
                        result = result.Join(autName, pi => pi.AuthorID, p => p.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.Name,
                            Info = p.Info,
                            InfoConcat = p.Info,
                            Type = p.Type,
                            ProId = p.ProId,
                            Pages = p.Pages,
                            AuthorID = p.AuthorID,
                            CategoryID = p.CategoryID,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.UnitPrice,
                            ISBN = p.ISBN,
                            IsBookable = p.IsBookable,
                            Category = p.Category,
                            DirectorID = p.DirectorID,
                            AuthorName = pi.Firstname + " " + pi.Lastname
                        }).Where(x => x.Type == "Bok").ToList();

                        //For visual purposes, reduce string length of item description to 60.
                        //Substring is saved into a separate variable so the full description can still be accessed.
                        for (int i = 0; i < result.Count; i++)
                        {
                            if (result[i].Info != null && result[i].Info.Length > 60)
                            {
                                result[i].InfoConcat = String.Concat(result[i].Info.Substring(0, 60), "...");
                            }
                        }
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
            return taskResult;
        }


        //GET MOVIES FUNCTIONS
        #region
        public async Task<List<ProductModel>> GetMoviesAsync()
        {

            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                List<ProductModel> catRes = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {
                        Category = db.ProductCategories.ToList();
                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();
                        dirName = db.Directors.ToList();

                        //Products table joined with Producttypes table
                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.ProductName,
                            Info = p.ProductInfo,
                            Type = pi.Type,
                            ProId = p.Id,
                            InfoConcat = p.ProductInfo,
                            Pages = p.BookPages,
                            AuthorID = p.AuthorId,
                            DirectorID = p.DirectorId,
                            CategoryID = p.CategoryId,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.Price,
                            ISBN = p.Isbn,
                            IsBookable = p.IsBookable
                        }).ToList();

                        //Result of products + producttype join extended with second join.
                        //Result joined with Category table
                        result = result.Join(Category, pi => pi.CategoryID, p => p.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.Name,
                            Info = p.Info,
                            InfoConcat = p.Info,
                            Type = p.Type,
                            ProId = p.ProId,
                            Pages = p.Pages,
                            CategoryID = p.CategoryID,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.UnitPrice,
                            ISBN = p.ISBN,
                            IsBookable = p.IsBookable,
                            Category = pi.Category,
                            DirectorID = p.DirectorID
                        }).ToList();


                        //Result of previous joins extended with third join.
                        //Result joined with Director table
                        result = result.Join(dirName, pi => pi.DirectorID, p => p.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.Name,
                            Info = p.Info,
                            InfoConcat = p.Info,
                            Type = p.Type,
                            ProId = p.ProId,
                            Pages = p.Pages,
                            CategoryID = p.CategoryID,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.UnitPrice,
                            ISBN = p.ISBN,
                            IsBookable = p.IsBookable,
                            Category = p.Category,
                            DirectorID = p.DirectorID,
                            AuthorName = pi.Firstname + " " + pi.Lastname
                        }).Where(x => x.Type == "Film").ToList();


                        //For visual purposes, reduce string length of item description to 60.
                        //Substring is saved into a separate variable so the full description can still be accessed.
                        for (int i = 0; i < result.Count; i++)
                        {
                            if (result[i].Info != null && result[i].Info.Length > 60)
                            {
                                result[i].InfoConcat = String.Concat(result[i].Info.Substring(0, 60), "...");
                            }
                        }
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
            return taskResult;
        }
        #endregion


        //GET SINGLE PRODUCT INFO FUNCTION
        #region

        //When an item is selected from the list, this function is called in order to dynamically display a full-info section
        //of that specific item.
        public async Task<List<ProductModel>> GetBooksFullListAsync(string prodName, string prodType, string prodCat, string authorName)
        {
            Task<List<ProductModel>> databaseTask = Task<List<ProductModel>>.Factory.StartNew(() =>
            {
                List<ProductModel> result = null;
                try
                {
                    using (var db = new LibraryDBContext())
                    {

                        Products = db.Products.ToList();
                        ProdType = db.ProductTypes.ToList();
                        Category = db.ProductCategories.ToList();

                        var pCat = Category.Where(x => x.Id == Products.FirstOrDefault().CategoryId).ToList();

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) =>
                        new ProductModel
                        {
                            ProId = p.Id,
                            Image = p.Image,
                            Name = p.ProductName,
                            Info = p.ProductInfo,
                            Type = pi.Type,
                            UnitPrice = p.Price,
                            AuthorID = p.AuthorId,
                            DirectorID = p.DirectorId,
                            ReleaseDate = p.ReleaseDate,
                            IsBookable = p.IsBookable,
                            Category = prodCat,
                            ISBN = p.Isbn,
                            AuthorName = authorName
                        }).Where(x => x.Name == prodName && x.Type == prodType).ToList();
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
            return taskResult;
        }

        #endregion

        //BOOK PRODUCT FUNCTIONS
        #region
        private async void BookProductButton_Clicked(object sender, EventArgs e)
        {
            InspectProductListView.SelectedItem = null;

            ShoppingCart cart = new ShoppingCart();
            DateTime returnDate = DateTime.Now.AddDays(30);
            DateTime bookedDate = DateTime.Now;
            CultureInfo dateTimeLanguage = CultureInfo.GetCultureInfo("sv-SE");


            Button btn = sender as Button;
            ProductModel item = btn.BindingContext as ProductModel;

            if (item.IsBookable != true)
            {
                await DisplayAlert("Går ej att boka", "Denna produkt går för tillfället inte att boka", "OK");
                return;
            }

           else if (item != null)
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
                            await DisplayAlert($"{typeOfProduct} bokad",
                                $"{item.Name} är bokad.\nLämnas tillbaks senast {returnDate.ToString("dddd, MMMM dd, yyyy", dateTimeLanguage)}", "OK");
                        }
                    }
                });
            }

  

            else
                await DisplayAlert("Produkt ej vald", "Välj en produkt för att boka", "OK");


        }
        #endregion


        //Show detailed product info based on item selected
        #region

        private async void ProductListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            ProductModel model = ProductListView.SelectedItem as ProductModel;
            InspectProductListView.ItemsSource = await GetBooksFullListAsync(model.Name, model.Type, model.Category, "Författare: " + model.AuthorName);

            if (InspectProductListView.Opacity == 0)
            {
                await Task.WhenAll(InspectProductListView.FadeTo(1, 1000));
            }

        }

        private async void MovieButtons_Clicked(object sender, EventArgs e)
        {
            EbooksListview.Opacity = 0;
            EbooksListview.IsVisible = true;

            ProductListView.ItemsSource = await GetBooksAsync();
            await Task.WhenAll(EbooksListview.FadeTo(1, 1000), ProductListView.FadeTo(0, 500));

            ProductListView.IsVisible = false;
        }

        private async void BooksButton_Clicked(object sender, EventArgs e)
        {
            ProductListView.Opacity = 0;
            ProductListView.IsVisible = true;

            ProductListView.ItemsSource = await GetBooksAsync();
            await Task.WhenAll(ProductListView.FadeTo(1, 1000), EbooksListview.FadeTo(0, 500));

            EbooksListview.IsVisible = false;
        }

        private async void EbooksListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (InspectProductListView.Opacity == 0)
            {
                await Task.WhenAll(InspectProductListView.FadeTo(1, 1000));
            }


            ProductModel model = EbooksListview.SelectedItem as ProductModel;
            
            InspectProductListView.ItemsSource = await GetBooksFullListAsync(model.Name, model.Type, model.Category, "Regissör: " + model.AuthorName);

        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var booklist = await GetBooksAsync();
            var filmlist = await GetMoviesAsync();

            ProductListView.ItemsSource = booklist.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.AuthorName.ToLower().Contains(e.NewTextValue.ToLower()));
            EbooksListview.ItemsSource = filmlist.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.AuthorName.ToLower().Contains(e.NewTextValue.ToLower()));
        }
        #endregion
    }
}
