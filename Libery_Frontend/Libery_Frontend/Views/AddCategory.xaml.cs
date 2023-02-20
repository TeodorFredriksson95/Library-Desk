using Libery_Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

#region not actively used
//code structure implemented into AddProducttype.cs
//this page only used for code reference. this class can safely be removed
namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCategory : ContentPage
    {
        public List<ProductCategory> categories;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            using (var db = new Models.LibraryDBContext())
            {
                categories = db.ProductCategories.Select(x => new ProductCategory { Category=x.Category }).ToList();
                AllCategories.ItemsSource = categories;
            }
        }
        public AddCategory()
        {
            InitializeComponent();
        }

        private async void Addcategory_Clicked(object sender, EventArgs e)
        {
            using (var db = new Models.LibraryDBContext())
            {
                var newCategory = new ProductCategory
                {
                    Category = CategoriesEntry.Text,
                   
                };
                var svar = await DisplayAlert($"Vill du lägga till {CategoriesEntry.Text} ?", "Är du säker?", "Ja", "Nej");
                if (svar == true)
                {
                    db.Add(newCategory);
                    db.SaveChanges();
                    CategoriesEntry.Text = "";
                    
                    
                }
                else { }

                categories = db.ProductCategories.Select(x => new ProductCategory { Category = x.Category }).ToList();
                AllCategories.ItemsSource = categories;
            }
        }

        private void AllCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            var s = (Picker)sender;
            if (s.SelectedIndex == -1) return;

            ProductCategory author = (ProductCategory)s.SelectedItem;

            string personsAsString = s.Items[s.SelectedIndex];
            ProductCategory author2 = (ProductCategory)s.ItemsSource[s.SelectedIndex];

            CategoriesEntry.Text = personsAsString;
        }
    }
}
#endregion