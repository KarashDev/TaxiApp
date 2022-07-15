
using Microsoft.AspNetCore.SignalR.Client;
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
using TaxiApp.Driver.Services;

namespace TaxiApp.Driver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HubConnection connection;

        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthenticationService _authenticationService;
        private readonly ITaxiAnswerService _taxiAnswerSender;

        private SharedModels.Driver currentDriver;

        public MainWindow(IServiceProvider serviceProvider, IAuthenticationService _authenticationService, ITaxiAnswerService taxiAnswerSender)
        {
            InitializeComponent();


            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44348/taxi")
                .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };


            this._serviceProvider = serviceProvider;
            this._authenticationService = _authenticationService;
            this._taxiAnswerSender = taxiAnswerSender;

            lbl_CurrentUserName.Visibility = Visibility.Hidden;
            lbl_NewOrder.Visibility = Visibility.Hidden;
            btn_AcceptOrder.Visibility = Visibility.Hidden;
            lbl_TimerNotif.Visibility = Visibility.Hidden;

            btn_Registrate.IsEnabled = false;
            btn_Login.IsEnabled = false;
        }

        private async void btn_ConnectSignalR_Click(object sender, RoutedEventArgs e)
        {
            btn_Registrate.IsEnabled = true;
            btn_Login.IsEnabled = true;

            connection.On<SharedModels.Customer>("NewCustomer",
                                  (customer) =>
                                  {
                                      currentDriver.customer = customer;

                                      lbl_NewOrder.Visibility = Visibility.Visible;
                                      btn_AcceptOrder.Visibility = Visibility.Visible;

                                      lbl_NewOrder.Content = $"НОВЫЙ ЗАКАЗ: Имя: {customer.username} === Место: {customer.currentCoordinate}";
                                  });

            try
            {
                await connection.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к SignalR", "Taxi App");
            }
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

                    currentDriver = loginResult.Result;

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

        private async void btn_AcceptOrder_Click(object sender, RoutedEventArgs e)
        {
            lbl_TimerNotif.Visibility = Visibility.Visible;
            var timer = new System.Windows.Threading.DispatcherTimer
            {
                Interval = new TimeSpan(0, 5, 0)
            };
            timer.Tick += async (o, t) => 
            {
                try
                {
                    await _taxiAnswerSender.SendOrder(connection, "AcceptOrder", currentDriver);
                }
                catch (Exception)
                {
                    MessageBox.Show("Ошибка при отправке заказа на сервер", "Taxi App");
                }
                timer.IsEnabled = true;
            };
            timer.Start();
        }
    }
}
