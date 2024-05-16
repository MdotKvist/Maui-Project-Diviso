using System.Diagnostics;
using System.Windows.Input;

namespace DivisoMaui
{
    public partial class MainPage : ContentPage
    {
        string correctPassword = "1234"; // The correct password

        public MainPage()
        {
            InitializeComponent();
            
            // Makes databinding on this page possible
            BindingContext = this;
        }

        // Command that toggles password visibility
        public ICommand TogglePasswordVisibilityCommand => new Command(TogglePasswordVisibility);

        // This Functions sets the command for the ability to toggle password visibility
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

        // Event handler for the login button
        void OnLoginClicked(object sender, EventArgs e)
        {
            // Gets the entered password from the password entry
            string enteredPassword = passwordEntry.Text;

            // Checks to see if the password matches the Variable enteredPassword
            if (enteredPassword == correctPassword)
            {
                // Navigates to Search pages if password is correct
                var search = new Search();
                Navigation.PushAsync(search);
                Debug.WriteLine("You have been logged in!");
            }
            else
            {
                // Shows modal popup if password is incorrect
                var modal = new Modal();
                Navigation.PushModalAsync(modal);
                Debug.WriteLine("Password does not match up. Try again.");
            }
        }

    }

}
