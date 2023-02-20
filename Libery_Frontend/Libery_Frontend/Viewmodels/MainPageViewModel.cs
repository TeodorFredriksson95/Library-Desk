//using System;
//using System.Collections.Generic;
//using System.Text;
//using Xamarin.Forms;
//using Xamarin.Essentials;
//using System.Collections.ObjectModel;
//using Libery_Frontend.Views;
//using Libery_Frontend.Models;
//using System.Windows.Input;
//using System.Threading.Tasks;
//using System.Linq;

//namespace Libery_Frontend.Viewmodels
//{
//    public class MainPageViewModel: BindableObject
//    {

//        public List<Models.Product> Products;
//        public List<Models.ProductType> ProdType;
//        ActivityIndicator indicator;
//        private ObservableCollection<ProductModel> _collectionList;
//        public ObservableCollection<ProductModel> CollectionList
//        {
//            get { return _collectionList; }
//            set
//            {
//                _collectionList = value;
//                OnPropertyChanged();
//            }

//        }

//        public ICommand ShowButton { get; }

//        public MainPageViewModel()
//        {
//            CollectionList = new ObservableCollection<ProductModel>();
//            MainThread.BeginInvokeOnMainThread(async () => { CollectionList = await GetProductsAsync(indicator); });
//            ShowButton = new Command(DisplayButton);
//        }

//        public async Task<ObservableCollection<ProductModel>> GetProductsAsync(ActivityIndicator indicator)
//        {
//            indicator.IsVisible = true;
//            indicator.IsRunning = true;
//            Task<ObservableCollection<ProductModel>> databaseTask = Task<ObservableCollection<ProductModel>>.Factory.StartNew(() =>
//            {
//                ObservableCollection<ProductModel> result = null;
//                try
//                {
//                    using (var db = new Models.LibraryDBContext())
//                    {

//                        Products = db.Products.ToList();
//                        ProdType = db.ProductTypes.ToList();

//                        result = (ObservableCollection<ProductModel>)Products.Join(ProdType, p => p.ProductTypeId, pi => pi.Id, (p, pi) => new ProductModel { Image = p.Image, Name = p.ProductName, Info = p.ProductInfo, Type = pi.Type });
//                    }
//                }

//                catch (Exception ex)
//                {
//                    // Display modal for error
//                }
//                return result;
//            }
//            );

//            var taskResult = await databaseTask;

//            indicator.IsRunning = false;
//            indicator.IsVisible = false;

//            return taskResult;
//        }

//        private void DisplayButton(object obj)
//        {
//            var homePage = new MainPage();
//            if (homePage.loginPage == 2)
//            {
//                Button btn = (Button)obj
//            }
//        }
//    }
//}
