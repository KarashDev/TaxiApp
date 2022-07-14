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

namespace TaxiApp.Driver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthenticationService _authenticationService;

        private SharedModels.Driver currentCustomer;

        public MainWindow(IServiceProvider serviceProvider, IAuthenticationService _authenticationService)
        {
            InitializeComponent();

            this._serviceProvider = serviceProvider;
            this._authenticationService = _authenticationService;

            lbl_CurrentUserName.Visibility = Visibility.Hidden;
        }

        private void btn_Registrate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var registrationResult = _authenticationService.Register(txtbox_Login.Text, txtBox_Password.Text);

                if (registrationResult.Result == RegistrationResult.Success)
                    MessageBox.Show("Водитель зарегистрирован", "Taxi App");
                else if (registrationResult.Result == RegistrationResult.EmptyInput)
                    MessageBox.Show("Оба поля должны быть заполнены", "Taxi App");
                else if (registrationResult.Result == RegistrationResult.UsernameAlreadyExists)
                    MessageBox.Show("Водитель с указанными данными уже существует", "Taxi App");

            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при регистрации водителя", "Taxi App");
            }
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loginResult = _authenticationService.Login(txtbox_Login.Text, txtBox_Password.Text);

                if (loginResult != null)
                {
                    lbl_CurrentUserName.Content = $"Текущий водитель: {loginResult.Result.username}";
                    lbl_CurrentUserName.Visibility = Visibility.Visible;

                    currentCustomer = loginResult.Result;

                    MessageBox.Show($"Вы успешно вошли в систему", "Taxi App");
                }
                else
                    MessageBox.Show($"Ошибка: водитель с такими данными не найден", "Taxi App");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при поиске водителя в базе", "Taxi App");
            }
        }
    }
}
