using Admin.Model;
using Admin.Persistence;
using Admin.View;
using Admin.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Admin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IRManagerModel model;
        private LoginWindow loginView;
        private LoginViewModel loginViewModel;
        private MainWindow mainView;
        private MainViewModel mainViewModel;
        private ItemEditorWindow editorView;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
            Exit += new ExitEventHandler(App_Exit);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            model = new RManagerModel(new RManagerServicePersistence("http://localhost:9505/"));

            loginViewModel = new LoginViewModel(model);
            loginViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            loginViewModel.LoginSuccess += new EventHandler(ViewModel_LoginSuccess);
            loginViewModel.LoginFailed += new EventHandler(ViewModel_LoginFailed);

            loginView = new LoginWindow();
            loginView.DataContext = loginViewModel;
            loginView.Show();
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("A bejelentkezés sikertelen!", "Étterem manager", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            mainViewModel = new MainViewModel(model);
            mainViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            mainViewModel.ProductEditingStarted += new EventHandler(MainViewModel_ProductEditingStarted);
            mainViewModel.ProductEditingFinished += new EventHandler(MainViewModel_ProductEditingFinished);
            mainViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);

            mainView = new MainWindow();
            mainView.DataContext = mainViewModel;
            mainView.Show();

            loginView.Close();
        }

        private void ViewModel_ExitApplication(object sender, EventArgs e)
        {
            Shutdown();
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            if (model.IsUserLoggedIn)
            {
                model.LogoutAsync();
            }
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "Étterem manager", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainViewModel_ProductEditingStarted(object sender, EventArgs e)
        {
            editorView = new ItemEditorWindow(); // külön szerkesztő dialógus az épületekre
            editorView.DataContext = mainViewModel;
            editorView.Show();
        }

        private void MainViewModel_ProductEditingFinished(object sender, EventArgs e)
        {
            editorView.Close();
        }
    }
}
