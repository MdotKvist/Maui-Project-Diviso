using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace DivisoMaui
{
    public partial class ConfirmationModal : ContentPage
    {
        private string selectedAddresses;
        public IList<string> SavedAddresses { get; set; }
        public object addressesListView { get; set; }

        public ConfirmationModal(string selectedAddress, IList<string> savedAddresses)
        {
            InitializeComponent();

            this.selectedAddresses = selectedAddress;
            SavedAddresses = savedAddresses;
        }

        private async void DeleteButtonClicked(object sender, EventArgs e)
        {
            if (SavedAddresses != null)
            {
                SavedAddresses.Remove(selectedAddresses);

                // Udates JSON file with new addresses
                await UpdateJsonFile(SavedAddresses);

                await Navigation.PopModalAsync();
            }
        }

        private async Task UpdateJsonFile(IList<string> updatedAddresses)
        {
            try
            {
                // Define Path to JSON file
                var fileSystem = FileSystem.Current;
                string filePath = Path.Combine(fileSystem.AppDataDirectory, "saved_addresses.json");

                // Converts Json File to string variable called json
                string json = JsonSerializer.Serialize(updatedAddresses);

                // Save JSON string to the Json file
                await File.WriteAllTextAsync(filePath, json);

                Debug.WriteLine($"Updated JSON file With {updatedAddresses.Count} adresses.");
            }
            catch (Exception ex)
            {
                // Behandle eventuel fejl
                Debug.WriteLine($"Error while updating JSON-fil: {ex.Message}");
            }
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            // Close modal
            Navigation.PopModalAsync();
        }
    }
}
