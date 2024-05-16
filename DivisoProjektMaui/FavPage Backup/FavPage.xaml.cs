using System.Diagnostics;
using System.Text.RegularExpressions;

namespace DivisoMaui
{
    public partial class FavPage : ContentPage
    {
        private string selectedItem;

        public IList<string> SavedAddresses { get; set; }

        public FavPage(List<string> savedAddresses)
        {
            InitializeComponent();

            SavedAddresses = savedAddresses;
            BindingContext = this; // Sæt BindingContext til denne side
            

        }

        private void DeleteSelectedButtonClicked(object sender, EventArgs e)
        {
            selectedItem = addressesListView.SelectedItem as string;
            if (!string.IsNullOrEmpty(selectedItem))
            {
                var Confmodal = new ConfirmationModal(selectedItem, SavedAddresses);
                Navigation.PushModalAsync(Confmodal);


            }





            //var selectedItem = addressesListView.SelectedItem as string;
            //if (selectedItem != null)
            //{
            //    SavedAddresses.Remove(selectedItem);
            //    addressesListView.ItemsSource = null; // Nulstil ItemsSource
            //    addressesListView.ItemsSource = SavedAddresses; // Opdater ItemsSource

            //}
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

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
