using System;
using System.Collections.Generic;
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

                // Opdater JSON-filen med de nye adresser
                await UpdateJsonFile(SavedAddresses);

                await Navigation.PopModalAsync();
            }
        }

        private async Task UpdateJsonFile(IList<string> updatedAddresses)
        {
            try
            {
                // Få adgang til appens datamappe
                var fileSystem = FileSystem.Current;
                string filePath = Path.Combine(fileSystem.AppDataDirectory, "saved_addresses.json");

                // Serialiser den opdaterede liste til en JSON-streng
                string json = JsonSerializer.Serialize(updatedAddresses);

                // Gem JSON-strengen til filen
                await File.WriteAllTextAsync(filePath, json);

                Console.WriteLine($"Opdateret JSON-fil med {updatedAddresses.Count} adresser.");
            }
            catch (Exception ex)
            {
                // Behandle eventuel fejl
                Console.WriteLine($"Fejl ved opdatering af JSON-fil: {ex.Message}");
            }
        }

        private void CloseButton_Clicked(object sender, EventArgs e)
        {
            // Luk modalen
            Navigation.PopModalAsync();
        }
    }
}
