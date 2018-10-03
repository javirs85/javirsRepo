using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GBControlStation
{
    public partial class App : Application
    {
        private GameBrain Brain;

        public App()
        {
            InitializeComponent();

            Brain = new GameBrain();
            MainPage = new MainPage(Brain);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
