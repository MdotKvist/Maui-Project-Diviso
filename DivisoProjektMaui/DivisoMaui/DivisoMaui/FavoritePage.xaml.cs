using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DivisoMaui
{
    public partial class FavoritePage : ContentPage
    {
        private string selectedItem;

        public IList<string> SavedAddresses { get; set; }

        public FavoritePage(List<string> savedAddresses)
        {
            InitializeComponent();

            SavedAddresses = savedAddresses;
            BindingContext = this; 
        }

        private async Task LoadSavedAddressesFromJsonFile()
        {
            try
            {
                // Define Path to json file
                var fileSystem = FileSystem.Current;
                string filePath = Path.Combine(fileSystem.AppDataDirectory, "saved_addresses.json");

                // Read json file and save in variabel
                string json = await File.ReadAllTextAsync(filePath);

                // Convert JSON-string to list of adresses
                SavedAddresses = JsonSerializer.Deserialize<List<string>>(json);
            }
            catch (Exception ex)
            {
                // Behandle eventuel fejl
                Debug.WriteLine($"Error while reading JSON file: {ex.Message}");
            }
        }


        private void DeleteSelectedButtonClicked(object sender, EventArgs e)
        {
            // Retrieve the currently selected item from the ListView
            selectedItem = addressesListView.SelectedItem as string;
            // Check if there is a selected item and that it's not empty
            if (!string.IsNullOrEmpty(selectedItem))
            {
                // Navigate to the ConfirmationModal page
                var Confmodal = new ConfirmationModal(selectedItem, SavedAddresses);
                Navigation.PushModalAsync(Confmodal);


            }

        }

        private void searchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get the text value from the search entry
            string searchText = e.NewTextValue;

            // If search entry is empty show the list SavedAdrresses
            if (string.IsNullOrEmpty(searchText))
            {
                addressesListView.ItemsSource = SavedAddresses;
            }
            else
            {
                // else show the filtered list
                addressesListView.ItemsSource = null;
                addressesListView.ItemsSource = SavedAddresses
                 .Where(address => Regex.IsMatch(address, searchText, RegexOptions.IgnoreCase))
                 .ToList();

            }
        }

        // Navigate to Search page
        private async void ButtonClickedSearched(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Read saved adresses from JSON filen
            await LoadSavedAddressesFromJsonFile();

            // Calls function ItemsSource
            UpdateItems();
        }

        private void UpdateItems()
        {
            addressesListView.SelectedItem = null; // Reset selectedItem
            addressesListView.ItemsSource = null; // Reset ItemsSource
            addressesListView.ItemsSource = SavedAddresses; // Update ItemsSource
        }

    }
}
