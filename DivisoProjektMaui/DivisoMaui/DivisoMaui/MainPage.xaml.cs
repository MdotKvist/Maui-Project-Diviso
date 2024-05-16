using System.Diagnostics;
using System.Windows.Input;

namespace DivisoMaui
{
    public partial class MainPage : ContentPage
    {
        string correctPassword = "1234"; // Den korrekte password

        public MainPage()
        {
            InitializeComponent();
            
            BindingContext = this;
        }

        public ICommand TogglePasswordVisibilityCommand => new Command(TogglePasswordVisibility);

        private void TogglePasswordVisibility()
        {
            if (passwordEntry.IsPassword)
            {
                passwordEntry.IsPassword = false;
            }
            else
            {
                passwordEntry.IsPassword = true;
            }
        }

        void OnLoginClicked(object sender, EventArgs e)
        {
            string enteredPassword = passwordEntry.Text;

            if (enteredPassword == correctPassword)
            {
                // Fra din nuværende side
                var search = new Search();
                Navigation.PushAsync(search);
                Debug.WriteLine("Du er nu logget ind!");
            }
            else
            {
                // Vise modal popup
                var modal = new Modal();
                Navigation.PushModalAsync(modal);
                Debug.WriteLine("Ugyldigt password. Prøv igen.");
            }
        }

    }

}
