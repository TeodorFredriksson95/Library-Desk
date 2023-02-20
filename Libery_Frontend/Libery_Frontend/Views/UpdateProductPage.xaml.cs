using Libery_Frontend.SecondModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//lol
namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateProductPage : ContentPage
    {
        public List<Product> Products;
        public List<ProductCategory> Category;
        public List<ProductType> ProdType;
        public List<AuthorName> aut;
        public List<Director> dir;
        public ProductModel pModel;
        public UpdateProductPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Load products asynchronously
            MainThread.BeginInvokeOnMainThread(async () =>
            {

                //populate pickers with producttypes and productcategories respectively from database
                using (var db = new LibraryDBContext())
                {
                    ProductTypePicker.ItemsSource = db.ProductTypes.Select(x => new ProductType { Id = x.Id, Type = x.Type }).ToList();
                    CategoryIDPicker.ItemsSource = db.ProductCategories.Select(x => new ProductCategory { Id = x.Id, Category = x.Category }).ToList();
                }
                ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator);
            });
        }


        //Returns every product available
        //Result is joined with producttypes
        public async Task<List<ProductModel>> GetProductsAsync(ActivityIndicator indicator)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
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

                        result = Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.ProductName,
                            Info = p.ProductInfo,
                            Type = pi.Type,
                            ProId = p.Id,
                            Pages = p.BookPages,
                            AuthorID = p.AuthorId,
                            DirectorID = p.DirectorId,
                            CategoryID = p.CategoryId,
                            ReleaseDate = p.ReleaseDate,
                            UnitPrice = p.Price,
                            ISBN = p.Isbn,
                            IsBookable = p.IsBookable,
                        }).ToList();

                        result = result.Join(Category, pi => pi.CategoryID, p => p.Id, (p, pi) =>
                        new ProductModel
                        {
                            Image = p.Image,
                            Name = p.Name,
                            Info = p.Info,
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

        //GET SINGLE PRODUCT INFO FUNCTION
        //When an item is selected from the list, this function is called in order to dynamically display a full-info section
        //of that specific item.
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

        #region populate entry info based on selected item
        private async void ProductListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Product prod;
            Product dirProd;
            ProductCategory cat;
            ProductType prodType;

            //define what type of listview has been selected
            ProductModel model = ProductListView.SelectedItem as ProductModel;
            
            //populate second listview with more specific info concerning selected item
            SecondProductListView.ItemsSource = await GetProductsFullListAsync(ActivityIndicator, model.Name, model.Type, model.Category);
            
            //change visual elements
            AllProdsFrame.IsVisible = false;
            SingleProdFrame.IsVisible = true;

            //info that populates second listview
            ISBNEntry.Text = model.ISBN;
            PriceEntry.Text = model.UnitPrice.ToString();
            AmountOfPagesEntry.Text = model.Pages.ToString();
            ImageURLEntry.Text = model.Image;
            DescriptionEntry.Text = model.Info;

            using (var db = new LibraryDBContext())
            {
                prod = db.Products.Where(x => x.AuthorId == model.AuthorID).FirstOrDefault();
                dirProd = db.Products.Where(x => x.DirectorId == model.DirectorID).FirstOrDefault();
                cat = db.ProductCategories.Where(x => x.Id == model.CategoryID).FirstOrDefault();
                prodType = db.ProductTypes.Where(x => x.Type == model.Type).FirstOrDefault();
                aut = db.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname, AuthorId = x.Id }).ToList();
                dir = db.Directors.Select(x => new Director { Firstname = x.Firstname, Lastname = x.Lastname, Id = x.Id }).ToList();

            }


            //automatically set type picker to the index where selected item matches property
            var prodTypePicker = ProductTypePicker;

            for (int i = 0; i < ProductTypePicker.Items.Count; i++)
            {
                string pType = prodTypePicker.Items[i].ToString();
                if (pType.ToLower() == prodType.Type.ToLower())
                {
                    ProductTypePicker.SelectedIndex = i;
                }
            }


            //automatically set author/director picker to the index where selected item matches property
            //determine wether picker is connected to Director table or Author table
            if (model.Type.ToLower() == "bok" || model.Type.ToLower() == "e-bok")
            {
                AuthorIDPicker.ItemsSource = aut;
                AuthorLab.Text = "Författare";
                var authorIDPicker = AuthorIDPicker;

                for (int i = 0; i < AuthorIDPicker.Items.Count; i++)
                {
                    string authorID = authorIDPicker.Items[i].ToString();
                    string[] autID = authorID.Split('(', ')', ' ');
                    if (Convert.ToInt32(autID[3]) == prod.AuthorId)
                    {
                        AuthorIDPicker.SelectedIndex = i;
                    }
                }
            }
            if (model.Type.ToLower() == "film" || model.Type.ToLower() == "e-film")
            {
                AuthorIDPicker.ItemsSource = dir;
                AuthorLab.Text = "Regissör";

                var authorIDPicker = AuthorIDPicker;

                for (int i = 0; i < AuthorIDPicker.Items.Count; i++)
                {
                    string authorID = authorIDPicker.Items[i].ToString();
                    string[] autID = authorID.Split('(', ')', ' ');
                    if (Convert.ToInt32(autID[3]) == dirProd.DirectorId)
                    {
                        AuthorIDPicker.SelectedIndex = i;
                    }
                }

            }



            //automatically set category picker to the index where selected item matches property
            var categoryIDPicker = CategoryIDPicker;

            for (int i = 0; i < CategoryIDPicker.Items.Count; i++)
            {
                string category = categoryIDPicker.Items[i].ToString();
                if (category.ToLower() == cat.Category.ToLower())
                {
                    CategoryIDPicker.SelectedIndex = i;
                }
            }

            //Check for null references. If value is not null, populate datepicker
            if (prod.ReleaseDate != null)
            {
                RealeseDatePicker.Date = (DateTime)prod.ReleaseDate;
            }

            if (model.IsBookable != null)
            {
                BookableCBX.IsChecked = model.IsBookable.Value;
            }
        }
        #endregion

        //Change visual layout
        private void EditButton_Clicked(object sender, EventArgs e)
        {
            EntryView.IsVisible = true;
        }


        //Change visual layout and re-populate first listview
        private async void BackToListButton_Clicked(object sender, EventArgs e)
        {
            ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator);
            AllProdsFrame.IsVisible = true;
            DefaultFrameText.IsVisible = true;
            SingleProdFrame.IsVisible = false;
            EntryView.IsVisible = false;

        }


        //Updates the details of selected itema
        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            string[] authorname = new string[5];
            if (AuthorIDPicker.SelectedItem != null)
            {
                authorname = AuthorIDPicker.SelectedItem.ToString().Split(' ');
            }
            else
            {
                await DisplayAlert("Ingen författare/regissör vald", "Välj en författare eller regissör", "OK");
                return;
            }

            var prodToRecieve = ProductListView.SelectedItem as ProductModel;
                string prodType = ProductTypePicker.SelectedItem.ToString();
                var cat = CategoryIDPicker.SelectedItem.ToString();
                ProductCategory catID;
                ProductType prodID;
                Product prodToUpdate;
            try
            {
                using (var db = new LibraryDBContext())
                {
                    prodID = db.ProductTypes.Where(x => x.Type == prodType).FirstOrDefault();
                    catID = db.ProductCategories.Where(x => x.Category == cat).FirstOrDefault();

                    prodToUpdate = db.Products.Single(x => x.Id == prodToRecieve.ProId);
                    prodToUpdate.ProductInfo = DescriptionEntry.Text;
                    prodToUpdate.Isbn = ISBNEntry.Text;
                    prodToUpdate.BookPages = Convert.ToInt32(AmountOfPagesEntry.Text);
                    prodToUpdate.Price = Convert.ToInt32(PriceEntry.Text);
                    prodToUpdate.ProductTypeId = prodID.Id;
                    prodToUpdate.CategoryId = catID.Id;
                    prodToUpdate.ReleaseDate = RealeseDatePicker.Date;

                    if (authorname[1].Contains("."))
                    {
                        prodToUpdate.AuthorId = Convert.ToInt32(authorname[3]);
                        prodToUpdate.DirectorId = null;
                    }
                    else
                    {
                        prodToUpdate.DirectorId = Convert.ToInt32(authorname[3]);
                        prodToUpdate.AuthorId = null;
                    }

                    if (BookableCBX.IsChecked)
                    {
                        prodToUpdate.IsBookable = true;
                    }
                    else prodToUpdate.IsBookable = false;

                    db.SaveChanges();

                    await DisplayAlert("Uppdaterad", $"Produkten har uppdaterats!", "OK");
                }
                ProductListView.ItemsSource = await GetProductsAsync(ActivityIndicator);
                SecondProductListView.ItemsSource = await GetProductsFullListAsync(ActivityIndicator, prodToUpdate.ProductName, prodID.Type, catID.Category);

            }
            catch (Exception ex)
            {
                await DisplayAlert("Felaktiga fält", "Alla fält måste fyllas i för att kunna uppdateras." +
                                                      "\nVar vänlig försök igen", "OK");
            }
        }

        //change visual elements
        private void DescriptionEntry_Focused(object sender, FocusEventArgs e)
        {
            DescriptionEntry.BackgroundColor = Color.Beige;
        }

        private void DescriptionEntry_Unfocused(object sender, FocusEventArgs e)
        {
            DescriptionEntry.BackgroundColor = Color.Wheat;
        }


        //Change picker values based on the selection of a different picker
        private void ProductTypePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = (Picker)sender;
            if (s.SelectedIndex == -1) return;

            ProductType producttype = (ProductType)s.SelectedItem;

            string typeSelected = s.Items[s.SelectedIndex];


            //If product type of selected item is book or e-book, change the pickers itemssource
            if (producttype.Type.ToLower() == "bok" || producttype.Type.ToLower() == "e-bok")
            {
                using (var db = new LibraryDBContext())
                {
                    aut = db.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname, AuthorId = x.Id }).ToList();
                }
                AuthorIDPicker.ItemsSource = aut;
                AuthorLab.Text = "Författare";
            }


            //if product type of selected item is movie or e-movie, change the pickers itemssource
            if (producttype.Type.ToLower() == "film" || producttype.Type.ToLower() == "e-film")
            {
                using (var db = new LibraryDBContext())
                {
                    dir = db.Directors.Select(x => new Director { Firstname = x.Firstname, Lastname = x.Lastname, Id = x.Id }).ToList();
                }
                AuthorIDPicker.ItemsSource = dir;
                AuthorLab.Text = "Regissör";
            }

        }

    }
}