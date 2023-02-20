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
    public partial class LibrarianTabbedPage : TabbedPage
    {
        public LibrarianTabbedPage()
        {
            InitializeComponent();
        }
        //tab page displayed only when logged in as librarian
    }
}