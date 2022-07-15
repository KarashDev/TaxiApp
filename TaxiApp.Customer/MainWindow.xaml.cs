using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        HubConnection connection;

        private readonly IServiceProvider _serviceProvider;
        private readonly IAuthenticationService _authenticationService;

        private SharedModels.Customer currentCustomer;

        public MainWindow(IServiceProvider serviceProvider, IAuthenticationService _authenticationService)
        {
            InitializeComponent();

            connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:44348/chat")
                .Build();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };



            this._serviceProvider = serviceProvider;
            this._authenticationService = _authenticationService;

            cmb_Coordinates.ItemsSource = Enum.GetValues(typeof(Coordinates.CustomerCoordinates));
            cmb_Coordinates.SelectedIndex = 0;

            lbl_CurrentUserName.Visibility = Visibility.Hidden;
            cmb_Coordinates.Visibility = Visibility.Hidden;
            btn_CallTaxi.Visibility = Visibility.Hidden;
        }

        private async void btn_ConnectSignalR_Click(object sender, RoutedEventArgs e)
        {
            //connection.On<string, string>("ReceiveMessage", (user, message) =>
            //{
            //    this.Dispatcher.Invoke(() =>
            //    {
            //        var newMessage = $"{user}: {message}";
            //        //messagesList.Items.Add(newMessage);
            //        lbl_ConnectionId.Content = user;
            //    });
            //});

            connection.On<string>("Connected",
                                   (connectionid) =>
                                   {
                                       //MessageBox.Show(connectionid);
                                       lbl_ConnectionId.Content = connectionid;
                                   });

            connection.On<SharedModels.Customer>("NewCustomer",
                                  (customer) =>
                                  {
                                      //MessageBox.Show(connectionid);
                                      lbl_ConnectionId.Content = customer.username;
                                  });

            try
            {
                await connection.StartAsync();
                //messagesList.Items.Add("Connection started");
                //connectButton.IsEnabled = false;
                //sendButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //messagesList.Items.Add(ex.Message);
            }
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
            try
            {
                var loginResult = _authenticationService.Login(txtbox_Login.Text, txtBox_Password.Text);

                if (loginResult.Result != null)
                {
                    lbl_CurrentUserName.Content = $"Текущий пользователь: {loginResult.Result.username}";
                    lbl_CurrentUserName.Visibility = Visibility.Visible;
                    cmb_Coordinates.Visibility = Visibility.Visible;
                    btn_CallTaxi.Visibility = Visibility.Visible;

                    currentCustomer = loginResult.Result;

                    MessageBox.Show($"Вы успешно вошли в систему", "Taxi App");
                }
                else
                    MessageBox.Show($"Ошибка: пользователь с такими данными не найден", "Taxi App");
            }
            catch (Exception)
            {
                MessageBox.Show("Ошибка при поиске пользователя в базе", "Taxi App");
            }
        }

        private async void btn_CallTaxi_Click(object sender, RoutedEventArgs e)
        {
            var chosenCoordinate = cmb_Coordinates.SelectedItem.ToString();
            currentCustomer.currentCoordinate = chosenCoordinate;

            try
            {
                await connection.InvokeAsync("NewCustomer", currentCustomer);
            }
            catch (Exception ex)
            {
                //messagesList.Items.Add(ex.Message);
            }





        }

        
    }
}
