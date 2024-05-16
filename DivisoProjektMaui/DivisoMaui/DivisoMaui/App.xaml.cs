namespace DivisoMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Application.Current.UserAppTheme = AppTheme.Light;
            MainPage = new AppShell();
            var mainPage = new NavigationPage(new Search());



        }
    }
}
