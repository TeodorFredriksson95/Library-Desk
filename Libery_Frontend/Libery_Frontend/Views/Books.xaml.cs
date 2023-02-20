using Libery_Frontend.SecondModels;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#region same functionality as E-Media page
//code explanation and references available in E_MediaPage
//exact same code structure except for final selection from database
namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Books : ContentPage
    {
        private Models.MetaStats _timeOnPage = null;
        public List<ProductCategory> Category;
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<Author> autName;
        public List<Director> dirName;
        public Books()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _timeOnPage = new Models.MetaStats("products", "Produkt sidan");

            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(async () => { ProductListView.ItemsSource = await GetBooksAsync(); });
            MainThread.BeginInvokeOnMainThread(async () => { EbooksListview.ItemsSource = await GetMoviesAsync(); });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainThread.BeginInvokeOnMainThread(async () => { await _timeOnPage.Finish(); _timeOnPage = null; });
        }

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
                        }).Where(x=>x.Type == "Film").ToList();


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


        public async Task<List<ProductModel>> GetSingleBookView( string prodName, string prodType, string prodCat, string authorName)
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

        private async void BookProductButton_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert(
                "Inloggning krävs",
                "Du måste logga in för att kunna låna en produkt.\n Vill du logga in?",
                "Logga in",
                "Avbryt"
            );
            if (answer)
            {
                var tab = new MainPage();
                tab.CurrentPage = tab.Children[4];

                await Application.Current.MainPage.Navigation.PushModalAsync(
                    new NavigationPage(tab)
                );
            }
            else
                return;
        }

        private async void ProductListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            ProductModel model = ProductListView.SelectedItem as ProductModel;
            InspectProductListView.ItemsSource = await GetSingleBookView( model.Name, model.Type, model.Category, "Författare: " + model.AuthorName);

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
            InspectProductListView.ItemsSource = await GetSingleBookView( model.Name, model.Type, model.Category, "Regissör: " + model.AuthorName);

        }

        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var booklist = await GetBooksAsync();
            var filmlist = await GetMoviesAsync();

            ProductListView.ItemsSource = booklist.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.AuthorName.ToLower().Contains(e.NewTextValue.ToLower()));
            EbooksListview.ItemsSource = filmlist.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.AuthorName.ToLower().Contains(e.NewTextValue.ToLower()));
        }

    }
}
#endregion