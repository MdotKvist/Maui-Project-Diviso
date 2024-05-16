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

                // Read json file save in variabel
                string json = await File.ReadAllTextAsync(filePath);

                // Deserialise JSON-string to list of adresses
                SavedAddresses = JsonSerializer.Deserialize<List<string>>(json);
            }
            catch (Exception ex)
            {
                // Behandle eventuel fejl
                Console.WriteLine($"Fejl ved læsning af JSON-fil: {ex.Message}");
            }
        }


        private void DeleteSelectedButtonClicked(object sender, EventArgs e)
        {
            selectedItem = addressesListView.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedItem))
            {
                var Confmodal = new ConfirmationModal(selectedItem, SavedAddresses);
                Navigation.PushModalAsync(Confmodal);


            }

        }

        private void searchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue;

            if (string.IsNullOrEmpty(searchText))
            {
                addressesListView.ItemsSource = SavedAddresses;
            }
            else
            {
                addressesListView.ItemsSource = null;
                addressesListView.ItemsSource = SavedAddresses
                 .Where(address => Regex.IsMatch(address, searchText, RegexOptions.IgnoreCase))
                 .ToList();

            }
        }


        private async void ButtonClickedSearched(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Læs gemte adresser fra JSON-filen
            await LoadSavedAddressesFromJsonFile();

            // Opdater ItemsSource her
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
