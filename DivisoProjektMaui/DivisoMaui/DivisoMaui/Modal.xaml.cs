using Microsoft.Maui.Controls;

namespace DivisoMaui
{
    public partial class Modal : ContentPage
    {
        public Modal()
        {
            InitializeComponent();
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            // Luk modalen
            Navigation.PopModalAsync();
        }
    }
}


