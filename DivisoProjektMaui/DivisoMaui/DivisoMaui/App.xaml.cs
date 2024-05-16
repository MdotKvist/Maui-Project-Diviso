namespace DivisoMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Forces app to stay in light mode
            Application.Current.UserAppTheme = AppTheme.Light;

            MainPage = new AppShell();
            var mainPage = new NavigationPage(new Search());



        }
    }
}
