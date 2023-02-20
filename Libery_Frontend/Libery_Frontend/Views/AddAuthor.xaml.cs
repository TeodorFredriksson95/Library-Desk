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
    public partial class AddAuthor : ContentPage
    {
        
        public List<AuthorName> authorNames;
        public AddAuthor()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            using (var db = new Models.LibraryDBContext())
            {
                authorNames = db.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname }).ToList();
                AllAuthor.ItemsSource = authorNames;
            }
        }
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
                if(svar == true)
                {
                    db.Add(newAuthor);
                    db.SaveChanges();
                    FirstNameEntry.Text = "";
                    LastNameEntry.Text = "";
                }
                else { }

                authorNames = db.Authors.Select(x => new AuthorName { Firstname = x.Firstname, Lastname = x.Lastname }).ToList();
                AllAuthor.ItemsSource = authorNames;
            }
          
        }

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
    }
}
#endregion