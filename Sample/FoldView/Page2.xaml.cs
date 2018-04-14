using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoldView
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Page2 : ContentPage
	{
		public Page2 ()
		{
			InitializeComponent ();

            this.Title = "Example 3 Views";

            ObservableCollection<Dictionary<string, object>> Data = new ObservableCollection<Dictionary<string, object>>();

            for (int i = 0; i < 2; i++)
            {
                Dictionary<string, object> model = new Dictionary<string, object>();
                Data.Add(model);
            }
            listView.BackgroundColor = Color.White;
            listView.HasUnevenRows = true;
            listView.ItemsSource = Data;
            listView.BindingContext = Data;
            listView.ItemTapped += ListView_ItemTapped;
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
        private void OnClick(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://wikitravel.org/en/Los_Angeles"));
        }
    }
}