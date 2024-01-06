using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace game_company
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

        }
        // Manejar el evento Clicked del botón
        private async void OnNavigateToCrudGameClicked(object sender, EventArgs e)
        {
            // Envolver la página principal en un NavigationPage si aún no está envuelta
            if (!(App.Current.MainPage is NavigationPage))
            {
                App.Current.MainPage = new NavigationPage(new MainPage());
            }

            // Navegar a la página CrudGame.xaml
            await Navigation.PushAsync(new CrudGame());
        }
        private async void OnNavigateToCrudCategoryClicked(object sender, EventArgs e)
        {
            // Envolver la página principal en un NavigationPage si aún no está envuelta
            if (!(App.Current.MainPage is NavigationPage))
            {
                App.Current.MainPage = new NavigationPage(new MainPage());
            }

            // Navegar a la página CrudGame.xaml
            await Navigation.PushAsync(new CrudCategory());
        }
        private async void OnNavigateToCrudDeveloperClicked(object sender, EventArgs e)
        {
            // Envolver la página principal en un NavigationPage si aún no está envuelta
            if (!(App.Current.MainPage is NavigationPage))
            {
                App.Current.MainPage = new NavigationPage(new MainPage());
            }

            // Navegar a la página CrudGame.xaml
            await Navigation.PushAsync(new CrudDeveloper());
        }
    }
}
