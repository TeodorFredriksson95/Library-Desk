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
    public partial class LoginPage : ContentPage
    {
        private Models.MetaStats _timeOnPage = null;

        //static property to ensure info is displayed based on user authority
        private static string _username;
        public static string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public LoginPage()
        {
            InitializeComponent();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _timeOnPage = new Models.MetaStats("login", "inloggningssidan");
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MainThread.BeginInvokeOnMainThread(async () => { await _timeOnPage.Finish(); _timeOnPage = null; });
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var userPassword = new List<User>();
            string userName = UsernameEntry.Text;

            Username = userName;

            using (var context = new LibraryDBContext())
            {
                userPassword = context.Users
                    .Where(x => x.Username == userName)
                    .Select(x => new User() { Password = x.Password, UserGroup = x.UserGroup })
                    .ToList();
                var usernameToCheck = context.Users.Where(x => x.Username == userName);
                if (usernameToCheck.Any())
                {
                    string password = PasswordEntry.Text;

                    //verify user password matches requirements of BCrypt hash
                    bool correctPassword = BCrypt.Net.BCrypt.Verify(
                        password,
                        userPassword[0].Password
                    );


                    //Change visual display based on authority of boss
                    if (correctPassword == true && userPassword[0].UserGroup == "chef")
                    {
                        Page pageToAdd = new LibrarianBossTabbedPage();
                        Page pageToAddFourth = new NotACustomerSearchPage();
                        Page pageToAddFifth = new NotACustomerProductPage();
                        Page pageToAddThird = new NotACustomerEProductsPage();

                        var homePage = new MainPage();

                        pageToAddThird.Title = "E-Media";
                        homePage.Title = $"{userName} - Inloggad";
                        pageToAdd.Title = "Bibliotekschef";
                        pageToAddFourth.Title = "Sök";
                        pageToAddFifth.Title = "Böcker & Filmer";

                        homePage.Children.RemoveAt(5);
                        homePage.Children.RemoveAt(4);
                        homePage.Children.RemoveAt(3);
                        homePage.Children.RemoveAt(2);
                        homePage.Children.RemoveAt(1);

                        pageToAdd.IconImageSource = "Icons/useradmin.png";
                        pageToAddFifth.IconImageSource = "Icons/book.png";
                        pageToAddThird.IconImageSource = "Icons/e-tablet.png";
                        pageToAddFourth.IconImageSource = "Icons/search.png";

                        homePage.Children.Insert(1, pageToAddFourth);
                        homePage.Children.Insert(2, pageToAddFifth);
                        homePage.Children.Insert(3, pageToAddThird);

                        homePage.Children.Add(pageToAdd);
                        await Navigation.PushAsync(homePage);

                        UsernameEntry.Text = "";
                        PasswordEntry.Text = "";

                    }

                    //Change visual display based on authority of librarian
                    else if (correctPassword == true && userPassword[0].UserGroup == "bibliotekarie")
                    {

                        Page pageToAdd = new LibrarianTabbedPage();
                        Page pageToAddFourth = new NotACustomerSearchPage();
                        Page pageToAddFifth = new NotACustomerProductPage();
                        Page pageToAddThird = new NotACustomerEProductsPage();

                        var homePage = new MainPage();

                        pageToAddThird.Title = "E-Media";
                        homePage.Title = $"{userName} - Inloggad";
                        pageToAdd.Title = "Bibliotekarie";
                        pageToAddFourth.Title = "Sök";
                        pageToAddFifth.Title = "Böcker & Filmer";

                        homePage.Children.RemoveAt(5);
                        homePage.Children.RemoveAt(4);
                        homePage.Children.RemoveAt(3);
                        homePage.Children.RemoveAt(2);
                        homePage.Children.RemoveAt(1);

                        homePage.Children.Insert(1, pageToAddFourth);
                        homePage.Children.Insert(2, pageToAddFifth);
                        homePage.Children.Insert(3, pageToAddThird);

                        pageToAdd.IconImageSource = "Icons/useradmin.png";
                        pageToAddFifth.IconImageSource = "Icons/book.png";
                        pageToAddThird.IconImageSource = "Icons/e-tablet.png";
                        pageToAddFourth.IconImageSource = "Icons/search.png";

                        homePage.Children.Add(pageToAdd);
                        await Navigation.PushAsync(homePage);

                        UsernameEntry.Text = "";
                        PasswordEntry.Text = "";
                    }

                    //Change visual display based on authority of user
                    else if (correctPassword == true && userPassword[0].UserGroup == "användare")
                    {
                       // Page pageToAdd = new UserAccountPage(userName);
                        Page pageToAddSecond = new UserBooksPage();
                        Page pageToAddThird = new UserSearchPage();
                        Page pageToAddFourth = new UserEProductsPage();
                        Page pageToAddFive = new TappedPageForUser();

                        var homePage = new MainPage();

                       // pageToAdd.IconImageSource = "shoppingcarticon.png";
                        pageToAddSecond.IconImageSource = "Icons/book.png";
                        pageToAddThird.IconImageSource = "Icons/search.png";
                        pageToAddFourth.IconImageSource = "Icons/e-tablet.png";
                        pageToAddFive.IconImageSource = "Icons/useronline.png";

                        homePage.Children.RemoveAt(5);
                        homePage.Children.RemoveAt(4);
                        homePage.Children.RemoveAt(3);
                        homePage.Children.RemoveAt(2);
                        homePage.Children.RemoveAt(1);
                        homePage.Children.Insert(1, pageToAddThird);
                        homePage.Children.Insert(2, pageToAddSecond);
                        homePage.Children.Insert(3, pageToAddFourth);
                        homePage.Children.Insert(4, pageToAddFive);


                        homePage.Children.Add(pageToAddFive);
                        await Navigation.PushAsync(homePage);

                        homePage.Title = $"{userName}";
                        homePage.IconImageSource = "Icons/logout-white.png"; 
                        pageToAddFive.Title = "Min profil";
                        pageToAddSecond.Title = "Böcker & Flmer";
                        pageToAddThird.Title = "Sök";
                        pageToAddFourth.Title = "E-Media";
                        pageToAddFive.Title = "Min profil";

                        Username = userName;

                        UsernameEntry.Text = "";
                        PasswordEntry.Text = "";
                    }

                    else
                        await DisplayAlert(
                            "Felaktigt Login",
                            "Användarnamn eller lösenord finns inte. Var vänlig försök igen",
                            "OK"
                        );
                }
                else
                {
                    await DisplayAlert(
                        "Felaktig Login",
                        "Användarnamn eller lösenord finns inte. Var vänlig försöker igen",
                        "OK");
                }


            }
        }


        //Not actively used for other than reference of hiding/showing (deleteing/creating instances of pages) tabs
        public void HideTab(int index)
        {
            TabbedPage theTabbedPage = App.Current.MainPage as TabbedPage;

            if (index < theTabbedPage.Children.Count())
            {
                theTabbedPage.Children.RemoveAt(index);
            }
        }

        public void AddTab(int index)
        {
            TabbedPage theTabbedPage = App.Current.MainPage as TabbedPage;

            var page = new LibrarianPage();

            if (theTabbedPage.Children.Contains(page))
            {
                theTabbedPage.Children.Insert(index, page);
            }


        }

        //on "register" click, navigate user to register page
        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            var tab = new MainPage();
            tab.CurrentPage = tab.Children[5];

            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(tab));

        }
    }
}

