using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaxiApp.Customer.Services;

namespace TaxiApp.Customer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthenticationService _authenticationService;

        public MainWindow(IServiceProvider serviceProvider, IAuthenticationService _authenticationService)
        {
            InitializeComponent();

            this._serviceProvider = serviceProvider;
            this._authenticationService = _authenticationService;
        }

        private void btn_Registrate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var registrationResult = _authenticationService.Register(txtbox_Login.Text, txtBox_Password.Text);

                if (registrationResult.Result == RegistrationResult.Success)
                    MessageBox.Show("Пользователь зарегистрирован", "Taxi App");
                else if (registrationResult.Result == RegistrationResult.EmptyInput)
                    MessageBox.Show("Оба поля должны быть заполнены", "Taxi App");
                else if (registrationResult.Result == RegistrationResult.UsernameAlreadyExists)
                    MessageBox.Show("Пользователь с указанными данными уже существует", "Taxi App");

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при регистрации пользователя", "Taxi App");
            }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {

        }




    }
}
