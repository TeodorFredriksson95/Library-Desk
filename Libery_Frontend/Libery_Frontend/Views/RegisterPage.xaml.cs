using Libery_Frontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        private Models.MetaStats _timeOnPage = null;
        User user = new User();
        public RegisterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _timeOnPage = new Models.MetaStats("register", "Registreringssida");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainThread.BeginInvokeOnMainThread(async () => { await _timeOnPage.Finish(); _timeOnPage = null; });
        }

        #region Registration
        private async void RegisterButton_Clicked(object sender, System.EventArgs e)
        {

            //Check availability for selected username. If username already exists, prompt user to select a different one.
            using (var context = new LibraryDBContext())
            {
                var checkUsernameAvailability = context.Users.Where(x => x.Username == UsernameEntry.Text);
                if (checkUsernameAvailability.Any())
                {
                    await DisplayAlert("Upptaget användarnamn", "Användarnamnet upptaget. Var vänlig välj ett annat.", "OK");
                    return;
                }

                else if (ConfirmPasswordEntry.Text != PasswordEntry.Text)
                {
                    await DisplayAlert("Felaktigt lösenord", "Lösenord matchar inte. Försök igen", "OK");
                    return;
                }



                else
                {
                    try
                    {
                        user.Username = UsernameEntry.Text;
                        user.Password = PasswordEntry.Text;

                        //encrypt password via BCrypt hash method
                        user.Password = BCrypt.Net.BCrypt.HashPassword(PasswordEntry.Text, 10);

                        user.Firstname = FirstnameEntry.Text ?? FirstnameEntry.Placeholder;
                        user.Lastname = LastnameEntry.Text ?? LastnameEntry.Placeholder;
                        user.PhoneNumber = PhonenumberEntry.Text ?? PhonenumberEntry.Placeholder;
                        user.City = CityEntry.Text ?? CityEntry.Placeholder;
                        user.Address = AddressEntry.Text ?? AddressEntry.Placeholder;
                        user.PostalCode = PostcodeEntry.Text ?? PostcodeEntry.Placeholder;
                        user.Email = EmailEntry.Text ?? EmailEntry.Placeholder;

                        user.UserGroup = "användare";

                        context.Users.Add(user);
                        context.SaveChanges();

                        await DisplayAlert("Registrerad", "Registrering klar", "OK");
                    }

                    //fail safes for inadequate entry information
                    catch (ArgumentNullException)
                    {
                        await DisplayAlert($"Felaktig info",
                                            $"Du måste fylla i alla fält",
                                            "OK");
                    }

                    catch (InvalidOperationException)
                    {
                        await DisplayAlert($"Felaktigt användarnamn",
                                           $"Du måste fylla ange ett användarnamn",
                                           "OK");
                    }
                }


            }
        }
        #endregion
    }
}