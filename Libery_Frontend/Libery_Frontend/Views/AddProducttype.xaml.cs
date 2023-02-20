using Libery_Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProduct : ContentPage
    {
        public List<ProductType> productTypes;
        public List<Models.ProductCategory> categories;
        public List<AuthorName> authorNames;
        public List<Director> dirNames;
        protected override async void OnAppearing()
        {

            //populate pickers based on author/director/product type/product category
            base.OnAppearing();
            using (var db = new Models.LibraryDBContext())
            {
                productTypes = db.ProductTypes.Select(x => new ProductType { Type = x.Type }).ToList();
                Allproducttype.ItemsSource = productTypes;

                var category = db.ProductCategories.Select(x => new ProductCategory { Id = x.Id, Category = x.Category }).ToList();
                PickerCategory.ItemsSource = category;

                authorNames = db.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname }).ToList();
                AllAuthor.ItemsSource = authorNames;

                dirNames = db.Directors.Select(x => new Director { Firstname = x.Firstname, Lastname = x.Lastname }).ToList();
                AllDirector.ItemsSource = dirNames;

            }
        }
        public AddProduct()
        {
            InitializeComponent();
        }

        //Add product type to database
        private async void AddProducttype_Clicked(object sender, EventArgs e)
        {
            using (var db = new Models.LibraryDBContext())
            {
                var newCategory = new ProductType
                {
                    Type = TypeEntry.Text,

                };
                var svar = await DisplayAlert($"Vill du lägga till '{TypeEntry.Text}' ?", "Är du säker?", "Ja", "Nej");
                if (svar == true)
                {
                    if (TypeEntry.Text != null)
                    {
                        db.Add(newCategory);
                        db.SaveChanges();

                        await DisplayAlert("Produkttyp tillagd", $"'{TypeEntry.Text}' har lagts till i listan", "OK");

                    }

                    else await DisplayAlert("Ingen vara tillagd", "Skriv in namnet på produkttypen du vill lägga till", "OK");


                }
                else return;

                TypeEntry.Text = "";
                Allproducttype.ItemsSource = db.ProductTypes.Select(x => new ProductType { Type = x.Type }).ToList();
            }
        }

        //Add author to database
        private async void AddAuthor_Clicked(object sender, EventArgs e)
        {
            using (var db = new Models.LibraryDBContext())
            {
                var newAuthor = new Author
                {
                    Firstname = FirstNameEntry.Text,
                    Lastname = LastNameEntry.Text,
                };
                var svar = await DisplayAlert($"Vill du lägga till {FirstNameEntry.Text} {LastNameEntry.Text}?", "Är du säker?", "Ja", "Nej");
                if (svar == true)
                {
                    if (FirstNameEntry.Text != null && LastNameEntry.Text != null)
                    {
                        db.Add(newAuthor);
                        db.SaveChanges();
                        await DisplayAlert("Författare tillagd", $" Författare '{FirstNameEntry.Text} {LastNameEntry.Text}' har lagts till i listan", "OK");

                    }
                    else await DisplayAlert("Fält saknas", "Vänligen fyll i alla fält som gäller författare", "OK");
                }
                else return;

                FirstNameEntry.Text = "";
                LastNameEntry.Text = "";
                AllAuthor.ItemsSource = db.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname }).ToList();
            }

        }

        //select already available author
        private void AllAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = (Picker)sender;
            if (s.SelectedIndex == -1) return;

            AuthorName author = (AuthorName)s.SelectedItem;

            string personsAsString = s.Items[s.SelectedIndex];
            AuthorName author2 = (AuthorName)s.ItemsSource[s.SelectedIndex];

            FirstNameEntry.Text = personsAsString;

            if (personsAsString.Contains(' '))
            {
                var split = personsAsString.Split(' ');
                FirstNameEntry.Text = split[0];
                LastNameEntry.Text = split[1];
            }
        }

        //remove product type from database
        private async void RemoveProducttype_Clicked(object sender, EventArgs e)
        {

            ProductType item = Allproducttype.SelectedItem as ProductType;
            string typeToBeRemoved = null;
            try
            {

                if (item != null)
                {
                    typeToBeRemoved = item.Type;
                    var svar = await DisplayAlert($"Vill du ta bort '{typeToBeRemoved}' ?", "Är du säker?", "Ja", "Nej");

                    if (svar == true)
                    {
                        using (var context = new Models.LibraryDBContext())
                        {
                            var removeType = context.ProductTypes.SingleOrDefault(x => x.Type == item.Type);
                            context.ProductTypes.Remove(removeType);
                            context.SaveChanges();

                            Allproducttype.ItemsSource = context.ProductTypes.Select(x => new ProductType { Type = x.Type }).ToList();
                            await DisplayAlert($"Produkttyp borttagen", $"Produkttyp '{typeToBeRemoved}' har tagits bort", "OK");
                        }
                    }
                    else return;
                }

                else await DisplayAlert($"Ingen produkttyp vald", $"Var vänlig välj en produkttyp att ta bort från listan", "OK");
            }
            catch (Exception ex) { await DisplayAlert($"Fel", $"Kunde inte ta bort {typeToBeRemoved} från listan", "OK"); }
        }

        //Add product category to database
        private async void AddCategoryButton_Clicked(object sender, EventArgs e)
        {
            using (var db = new Models.LibraryDBContext())
            {
                var newCategory = new ProductCategory
                {
                    Category = CategoryEntry.Text,

                };
                var svar = await DisplayAlert($"Vill du lägga till '{CategoryEntry.Text}' ?", "Är du säker?", "Ja", "Nej");
                if (svar == true)
                {
                    if (CategoryEntry.Text != null)
                    {
                        db.Add(newCategory);
                        db.SaveChanges();

                        await DisplayAlert("Produktkategori tillagd", $"'{CategoryEntry.Text}' har lagts till i listan", "OK");

                    }

                    else await DisplayAlert("Ingen kategori tillagd", "Skriv in namnet på produktkategorin du vill lägga till", "OK");


                }
                else return;

                CategoryEntry.Text = "";
                PickerCategory.ItemsSource = db.ProductTypes.Select(x => new ProductType { Type = x.Type }).ToList();
            }
        }

        //Remove product category from database
        private async void RemoveCategoryButton_Clicked(object sender, EventArgs e)
        {
            ProductCategory item = PickerCategory.SelectedItem as ProductCategory;
            string typeToBeRemoved = null;
            try
            {
                if (item != null)
                {
                    typeToBeRemoved = item.Category;
                    var svar = await DisplayAlert($"Vill du ta bort '{typeToBeRemoved}' ?", "Är du säker?", "Ja", "Nej");

                    if (svar == true)
                    {

                        using (var context = new Models.LibraryDBContext())
                        {
                            var removeType = context.ProductCategories.SingleOrDefault(x => x.Category == item.Category);
                            context.ProductCategories.Remove(removeType);
                            context.SaveChanges();

                            PickerCategory.ItemsSource = context.ProductCategories.Select(x => new ProductType { Type = x.Category }).ToList();
                            await DisplayAlert($"Produktkategori borttagen", $"Produktkategori '{typeToBeRemoved}' har tagits bort", "OK");
                        }
                    }

                    else return;

                }
                else await DisplayAlert($"Ingen produktkategori vald", $"Var vänlig välj en produktkategori att ta bort från listan", "OK");
            }
            catch (Exception ex) { await DisplayAlert($"Fel", $"Kunde inte ta bort {typeToBeRemoved} från listan", "OK"); }
        }

        //Remove author from database
        private async void RemoveAuthorButton_Clicked(object sender, EventArgs e)
        {
            AuthorName item = AllAuthor.SelectedItem as AuthorName;
            string typeToBeRemoved = null;
            try
            {
                if (item != null)
                {
                    typeToBeRemoved = item.Firstname + " " + item.Lastname;
                    var svar = await DisplayAlert($"Vill du ta bort '{typeToBeRemoved}' ?", "Är du säker?", "Ja", "Nej");

                    if (svar == true)
                    {

                        using (var context = new Models.LibraryDBContext())
                        {
                            var removeType = context.Authors.FirstOrDefault(x => x.Firstname == item.Firstname && x.Lastname == item.Lastname);
                            context.Authors.Remove(removeType);
                            context.SaveChanges();

                            AllAuthor.ItemsSource = context.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname }).ToList();
                            await DisplayAlert($"Författare borttagen", $"Författare '{typeToBeRemoved}' har tagits bort", "OK");
                        }
                    }

                    else return;

                }
                else await DisplayAlert($"Ingen författare vald", $"Var vänlig välj en författare att ta bort från listan", "OK");
            }
            catch (Exception ex) { await DisplayAlert($"Fel", $"Kunde inte ta bort {typeToBeRemoved} från listan", "OK"); }
        }

        //Remove director from database
        private async void RemoveDirectorButton_Clicked(object sender, EventArgs e)
        {
            {
                Director item = AllDirector.SelectedItem as Director;
                string typeToBeRemoved = null;
                try
                {
                    if (item != null)
                    {
                        typeToBeRemoved = item.Firstname + " " + item.Lastname;
                        var svar = await DisplayAlert($"Vill du ta bort '{typeToBeRemoved}' ?", "Är du säker?", "Ja", "Nej");

                        if (svar == true)
                        {

                            using (var context = new Models.LibraryDBContext())
                            {
                                var removeType = context.Directors.FirstOrDefault(x => x.Firstname == item.Firstname && x.Lastname == item.Lastname);
                                context.Directors.Remove(removeType);
                                context.SaveChanges();

                                AllDirector.ItemsSource = context.Directors.Select(x => new Director { Firstname = x.Firstname, Lastname = x.Lastname }).ToList();
                                await DisplayAlert($"Regissör borttagen", $"Regissör '{typeToBeRemoved}' har tagits bort", "OK");
                            }
                        }

                        else return;

                    }
                    else await DisplayAlert($"Ingen regissör vald", $"Var vänlig välj en regissör att ta bort från listan", "OK");
                }
                catch (Exception ex) { await DisplayAlert($"Fel", $"Kunde inte ta bort {typeToBeRemoved} från listan", "OK"); }
            }
        }

        //Add director to database
        private async void AddDirectorButton_Clicked(object sender, EventArgs e)
        {
            
                using (var db = new Models.LibraryDBContext())
                {
                    var newAuthor = new Director
                    {
                        Firstname = dirFirstNameEntry.Text,
                        Lastname = dirLastNameEntry.Text,
                    };
                    var svar = await DisplayAlert($"Vill du lägga till '{dirFirstNameEntry.Text} {dirLastNameEntry.Text}'?", "Är du säker?", "Ja", "Nej");
                    if (svar == true)
                    {
                        if (dirFirstNameEntry.Text != null && dirLastNameEntry.Text != null)
                        {
                            db.Add(newAuthor);
                            db.SaveChanges();
                            await DisplayAlert("Regissör tillagd", $" Regissör '{dirFirstNameEntry.Text} {dirLastNameEntry.Text}' har lagts till i listan", "OK");

                        }
                        else await DisplayAlert("Fält saknas", "Vänligen fyll i alla fält som gäller regissörer", "OK");
                    }
                    else return;

                    dirFirstNameEntry.Text = "";
                    dirLastNameEntry.Text = "";
                    AllDirector.ItemsSource = db.Directors.Select(x => new Director { Firstname = x.Firstname, Lastname = x.Lastname }).ToList();
                }

            
        }
    }
}