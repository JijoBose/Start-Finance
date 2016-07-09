using StartFinance.Models;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SQLite.Net;
using System.IO;

namespace StartFinance.Views
{
    public sealed partial class SettingsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");

        Template10.Services.SerializationService.ISerializationService _SerializationService;

        public SettingsPage()
        {
            InitializeComponent();
            _SerializationService = Template10.Services.SerializationService.SerializationService.Json;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var index = int.Parse(_SerializationService.Deserialize(e.Parameter?.ToString()).ToString());
            MyPivot.SelectedIndex = index;
        }

        private void ResetMan_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);
            /// deleteing table
            conn.DropTable<Accounts>();
            conn.DropTable<Assets>();
            conn.DropTable<Debt>();
            conn.DropTable<Transactions>();
            conn.DropTable<WishList>();
            conn.DropTable<Category>();
        }

        private void BusyTextTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BusyTextTextBox.Text.ToString() == "reset")
            {
                ResetMan.IsEnabled = true;
            }
            else
            {
                ResetMan.IsEnabled = false;
            }

        }

        private void BusyTextTextBox_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (BusyTextTextBox.Text.ToString() == "reset")
            {
                ResetMan.IsEnabled = true;
            }
            else
            {
                ResetMan.IsEnabled = false;
            }
        }
    }
}
