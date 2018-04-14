using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FoldView
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();

            this.Title = "Xamarin Fold View";
            btn1.Clicked += (sender, e) => {
                this.Navigation.PushAsync(new Page1());
            };
            btn2.Clicked += (sender, e) => {
                this.Navigation.PushAsync(new Page2());
            };
        }
	}
}
