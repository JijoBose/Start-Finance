using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite;
using InstaRichie.Models;
using Windows.UI.Popups;
using SQLite.Net;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace InstaRichie.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AssetsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        public AssetsPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            // Creating table
            conn.CreateTable<Assets>();

            /// Refresh Data
            var query = conn.Table<Assets>();
            AssetListView.ItemsSource = query.ToList();
        }

        private async void AddData(object sender, RoutedEventArgs e)
        {
            try
            {
                if (AssetNameText.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("No Account Name entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.Insert(new Assets()
                    {
                        AssetName = AssetNameText.Text,
                        AssetValue = Convert.ToDouble(AssetValue.Text)
                    });

                    var query = conn.Table<Assets>();
                    AssetListView.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the Value or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("A Similar Asset Nane already exists, Try a different name", "Oops..!");
                    await dialog.ShowAsync();
                }
            }
        }

        private async void ClearFileds_Click(object sender, RoutedEventArgs e)
        {
            AssetNameText.Text = string.Empty;
            AssetValue.Text = string.Empty;

            MessageDialog ClearDialog = new MessageDialog("Cleared", "information");
            await ClearDialog.ShowAsync();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            conn.CreateTable<Assets>();
            /// Refresh Data
            var query = conn.Table<Assets>();
            AssetListView.ItemsSource = query.ToList();
        }

        private async void DeleteAccout_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string AccSelection = ((Assets)AssetListView.SelectedItem).AssetName;
                if (AccSelection == "")
                {
                    MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    conn.CreateTable<Assets>();
                    var query1 = conn.Table<Assets>();
                    var query3 = conn.Query<Assets>("DELETE FROM Assets WHERE AssetName ='" + AccSelection + "'");
                    AssetListView.ItemsSource = query1.ToList();
                }
            }
            catch (NullReferenceException)
            {
                MessageDialog dialog = new MessageDialog("Not selected the Item", "Oops..!");
                await dialog.ShowAsync();
            }
        }
    }
}
