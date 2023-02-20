using Libery_Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#region not actively used
//code structure implemented into AddProducttype.cs
//this page only used for code reference. this class can safely be removed
namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategoryPage : ContentPage
    {
        public List<Models.ProductCategory> categories;
        public AddCategoryPage()
        {

            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();


            using (var db = new Models.LibraryDBContext())
            {

                var category = db.ProductCategories.Select(x => new ProductCategory { Id = x.Id, Category = x.Category }).ToList();

                PickerCategory.ItemsSource = category;
            }

        }

        private async void AddButton_Clicked(object sender, EventArgs e)
        {
            var category = new ProductCategory();

            if (CategoryEntry.Text != null)
            {
                using (var db = new Models.LibraryDBContext())
                {
                    category.Category = CategoryEntry.Text;

                    db.Add(category);
                    db.SaveChanges();

                    await DisplayAlert("Kategori tillagd", $"Du har lagt till kategorin {category.Category}", "Gå vidare");
                    var categoryList = db.ProductCategories.Select(x => new ProductCategory { Id = x.Id, Category = x.Category }).ToList();

                    PickerCategory.ItemsSource = categoryList;
                }

            }
            else await DisplayAlert("Ingen vara tillagd", "Skriv in namnet på kategorin du vill lägga till", "OK");
        }
    }

}
#endregion
