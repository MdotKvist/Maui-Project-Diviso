using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace DivisoMaui
{
    public partial class Search : ContentPage
    {
        private List<DawaAddress> autocompleteSuggestions = new List<DawaAddress>();
        public List<string> savedAddresses = new List<string>();

        public Search()
        {
            InitializeComponent();

            savedAddresses = new List<string>();

            // Add double tap gesture recognizer to handle double click on suggestion
            BindingContext = this;
            DoubleTapGestureRecognizers = new Command(async (obj) =>
            {
                if (obj is DawaAddress selectedAddress)
                {
                    SaveAddress(selectedAddress.tekst);
                }
            });
        }
        private void SearchEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = e.NewTextValue;

            // Call the DAWA API for autocomplete suggestions
            if (!string.IsNullOrEmpty(query))
            {
                SearchAddress(query);
            }
            else
            {
                // Hide the CollectionView if query is empty
                autocompleteCollectionView.IsVisible = false;
            }
        }


        private async void SearchAddress(string query)
        {
            // Creates an HttpClient to make a HTTP requests
            using (var client = new HttpClient())
            {
                // Define the api's url
                string apiUrl = $"https://dawa.aws.dk/adresser/autocomplete?q={query}";
                var response = await client.GetAsync(apiUrl);

                // // Checks if the response is a success
                if (response.IsSuccessStatusCode)
                {
                    // Converts the JSON string to a list of addresses
                    var json = await response.Content.ReadAsStringAsync();
                    autocompleteSuggestions = JsonSerializer.Deserialize<List<DawaAddress>>(json);

                    // Update CollectionView with autocomplete suggestions
                    autocompleteCollectionView.ItemsSource = autocompleteSuggestions;
                    autocompleteCollectionView.IsVisible = true;
                }
                else
                {
                    // Handle error
                    Debug.WriteLine("Error fetching autocomplete suggestions");
                }
            }
        }

        public ICommand DoubleTapGestureRecognizers { get; }


        private void AutocompleteCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null && e.CurrentSelection.Count > 0)
            {
                // Get the selected address
                DawaAddress selectedAddress = (DawaAddress)e.CurrentSelection[0];

                // Save the selected address
                SaveAddress(selectedAddress.tekst);
                Debug.WriteLine(selectedAddress.tekst);
            }
        }
        private void SaveAddress(string address)
        {
            // Add the address to the list of saved addresses
            savedAddresses.Add(address);

            SaveAddressesToJsonFile(savedAddresses);
        }

        private void SaveAddressesToJsonFile(List<string> addresses)
        {
            // Konverter listen af adresser til en JSON-streng
            string json = JsonSerializer.Serialize(addresses);

            // Få adgang til appens datamappe
            var fileSystem = FileSystem.Current;
            string filePath = Path.Combine(fileSystem.AppDataDirectory, "saved_addresses.json");

            // Gem JSON-strengen til filen
            File.WriteAllText(filePath, json);

            Console.WriteLine($"Gemt fil til: {filePath}");
        }

        // Navigate to FavoritePage
        private void ButtonclickedFav(object sender, EventArgs e)
        {
            var Fav = new FavoritePage(savedAddresses);
            Navigation.PushAsync(Fav);
        }
        
    }

    // Class to deserialize DAWA API response
    public class DawaAddress
    {
        public string tekst { get; set; }
    }
}
