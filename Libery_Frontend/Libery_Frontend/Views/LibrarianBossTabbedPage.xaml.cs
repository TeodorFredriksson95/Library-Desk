using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Libery_Frontend.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LibrarianBossTabbedPage : TabbedPage
    {
        public LibrarianBossTabbedPage()
        {
            InitializeComponent();
        }
        //tab only shown when logged in as boss
    }
}