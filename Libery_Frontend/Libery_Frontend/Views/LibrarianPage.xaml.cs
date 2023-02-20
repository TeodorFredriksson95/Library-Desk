using Libery_Frontend.SecondModels;
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
    public partial class LibrarianPage : ContentPage
    {
        public List<ProductCategory> Category;
        public List<Product> Products;
        public List<ProductType> ProdType;
        public List<Author> autName;
        public List<Director> dirName;
        public LibrarianPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();


            MainThread.BeginInvokeOnMainThread(async () =>
            {
                BooksListview.ItemsSource = await GetBooksAsync();
                E_bookslistview.ItemsSource = await GetE_BooksAsync();
                Movieslistview.ItemsSource = await GetMoviesAsync();
                E_Movieslistview.ItemsSource = await GetEMoviesAsync();
            });
        }

        #region populate lists
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
                            result[i].InfoConcat = String.Concat(result[i].Info.Substring(0, 60), "...");
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
                        }).Where(x => x.Type == "Film").ToList();


                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].InfoConcat = String.Concat(result[i].Info.Substring(0, 60), "...");
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
        public async Task<List<ProductModel>> GetE_BooksAsync()
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
                          //  DirectorID = p.DirectorId,
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
                           // DirectorID = p.DirectorID
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
                           // DirectorID = p.DirectorID,
                            AuthorName = pi.Firstname + " " + pi.Lastname
                        }).Where(x => x.Type == "E-Bok").ToList();



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
        public async Task<List<ProductModel>> GetEMoviesAsync()
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
                        }).Where(x => x.Type == "E-Film").ToList();


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
                        }).ToList();


                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i].InfoConcat = String.Concat(result[i].Info.Substring(0, 60), "...");
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
        
        //display specific info regarding the selected item
        public async Task<List<ProductModel>> GetProductsFullListAsync(ActivityIndicator indicator, string prodName, string prodType, string prodCat)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
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
                            ISBN = p.Isbn
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

            indicator.IsRunning = false;
            indicator.IsVisible = false;
            return taskResult;
        }

        #endregion
        #region change visual layout based on which listview item is clicked
        private async void BooksListview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            E_booksLVSL.IsVisible = false;
            E_MoviesLVSL.IsVisible = false;
            MoviesLVSL.IsVisible = false;

            ProductModel model = BooksListview.SelectedItem as ProductModel;
            SingleProdListview.ItemsSource = await GetProductsFullListAsync(ActivityIndicator, model.Name, model.Type, model.Category);

            Grid.SetColumnSpan(SingleProdFrame, 3);
            Grid.SetColumn(BooksLVSL, 3);
            SingleProdFrame.IsVisible = true;
        }

        private async void E_bookslistview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            BooksLVSL.IsVisible = false;
            E_MoviesLVSL.IsVisible = false;
            MoviesLVSL.IsVisible = false;

            ProductModel model = E_bookslistview.SelectedItem as ProductModel;
            SingleProdListview.ItemsSource = await GetProductsFullListAsync(ActivityIndicator, model.Name, model.Type, model.Category);

            Grid.SetColumnSpan(SingleProdFrame, 3);
            Grid.SetColumn(E_booksLVSL, 3);
            SingleProdFrame.IsVisible = true;
        }

        private async void Movieslistview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            BooksLVSL.IsVisible = false;
            E_booksLVSL.IsVisible = false;
            E_MoviesLVSL.IsVisible = false;

            ProductModel model = Movieslistview.SelectedItem as ProductModel;
            SingleProdListview.ItemsSource = await GetProductsFullListAsync(ActivityIndicator, model.Name, model.Type, model.Category);

            Grid.SetColumn(MoviesLVSL, 3);
            Grid.SetColumnSpan(SingleProdFrame, 3);
            SingleProdFrame.IsVisible = true;
        }

        private async void E_Movieslistview_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            BooksLVSL.IsVisible = false;
            E_booksLVSL.IsVisible = false;
            MoviesLVSL.IsVisible = false;

            ProductModel model = E_Movieslistview.SelectedItem as ProductModel;
            SingleProdListview.ItemsSource = await GetProductsFullListAsync(ActivityIndicator, model.Name, model.Type, model.Category);

            Grid.SetColumnSpan(SingleProdFrame, 3);
            Grid.SetColumn(E_MoviesLVSL, 3);
            SingleProdFrame.IsVisible = true;

        }
        #endregion

        //change visual layout and repopulate lists
        private async void BackToListButton_Clicked(object sender, EventArgs e)
        {

            E_MoviesLVSL.IsVisible = true;
            BooksLVSL.IsVisible = true;
            E_booksLVSL.IsVisible = true;
            MoviesLVSL.IsVisible = true;
            SingleProdFrame.IsVisible = false;

            Grid.SetColumn(BooksLVSL, 0);
            Grid.SetColumn(E_booksLVSL, 1);
            Grid.SetColumn(MoviesLVSL, 2);
            Grid.SetColumn(E_MoviesLVSL, 3);
            Grid.SetColumn(SingleProdFrame, 0);


            BooksListview.ItemsSource = await GetBooksAsync();
            E_bookslistview.ItemsSource = await GetE_BooksAsync();
            Movieslistview.ItemsSource = await GetMoviesAsync();
            E_Movieslistview.ItemsSource = await GetEMoviesAsync();

        }

        //function to delete selected item from listview. updates listviews after confirmation complete
        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {

            Button btn = sender as Button;
            ProductModel item = btn.BindingContext as ProductModel;

            try
            {


                using (var context = new LibraryDBContext())
                {
                    var removePost = context.Products.Where(x => x.Id == item.ProId).FirstOrDefault();
                    context.Products.Remove(removePost);
                    var svar = await DisplayAlert("Ta bort vald produkt", "Är du helt säker?", "Ja", "Nej");
                    if (svar == true)
                    {
                        context.SaveChanges();
                        E_MoviesLVSL.IsVisible = true;
                        BooksLVSL.IsVisible = true;
                        E_booksLVSL.IsVisible = true;
                        MoviesLVSL.IsVisible = true;
                        SingleProdFrame.IsVisible = false;


                        BooksListview.ItemsSource = await GetBooksAsync();
                        E_bookslistview.ItemsSource = await GetE_BooksAsync();
                        Movieslistview.ItemsSource = await GetMoviesAsync();
                        E_Movieslistview.ItemsSource = await GetEMoviesAsync();

                        Grid.SetColumn(BooksLVSL, 0);
                        Grid.SetColumn(E_booksLVSL, 1);
                        Grid.SetColumn(MoviesLVSL, 2);
                        Grid.SetColumn(E_MoviesLVSL, 3);
                        Grid.SetColumn(SingleProdFrame, 0);
                    }
                    else return;

                }
            }
            catch (Exception ex) { await DisplayAlert("Fel", "Denna bok är för tillfället lånad. Kan inte tas bort.", "OK"); }
        }


        //populate all lists based on search values
        private async void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var booklist = await GetBooksAsync();
            var filmlist = await GetMoviesAsync();
            var E_filmlist = await GetEMoviesAsync();
            var E_booklist = await GetE_BooksAsync();

            BooksListview.ItemsSource = booklist.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.AuthorName.ToLower().Contains(e.NewTextValue.ToLower()));
            E_bookslistview.ItemsSource = E_booklist.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.AuthorName.ToLower().Contains(e.NewTextValue.ToLower()));
            Movieslistview.ItemsSource = filmlist.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.AuthorName.ToLower().Contains(e.NewTextValue.ToLower()));
            E_Movieslistview.ItemsSource = E_filmlist.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()) || x.AuthorName.ToLower().Contains(e.NewTextValue.ToLower()));
        }
    }
}
