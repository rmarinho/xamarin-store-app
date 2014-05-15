using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Popups;
using XamarinStore.Helpers;
using XamarinStore.Services;
using MvvmLight = GalaSoft.MvvmLight.Command;

namespace XamarinStore.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        readonly string XamarinAccountEmail = "me@ruimarinho.net";
        readonly string DefatultAvatarImage = "../Resources/Images/user-default-avatar.png";
        INavigationService _navService;
        public LoginViewModel(INavigationService navService)
        {
            _navService = navService;
            Username = XamarinAccountEmail;
        }

        public bool ShouldShowInstructions
        {
            get { return string.IsNullOrEmpty(XamarinAccountEmail); }
        }

        async void Login(string username, string password)
        {
            IsBusy = true;
            //  BTProgressHUD.Show("Logging in...");

            var success = await WebService.Shared.Login(username, password);
            if (success)
            {
                var canContinue = await WebService.Shared.PlaceOrder(WebService.Shared.CurrentUser, true);
                if (!canContinue.Success)
                {

                    await MessageBox.ShowAsync("Only one shirt per person. Edit your cart and try again.", "Sorry",
                                              MessageBoxButton.OK);
                    IsBusy = false;
                    return;
                }
            }
            IsBusy = false;

            if (success)
            {
                _navService.Navigate("checkout");
            }
            else
            {
                await MessageBox.ShowAsync("Only one shirt per person. Edit your cart and try again.", "Sorry",
                                            MessageBoxButton.OK);
            }


        }
        private MvvmLight.RelayCommand _loginCommand;
        public MvvmLight.RelayCommand LoginCommand
        {
            get
            {
                return _loginCommand
                    ?? (_loginCommand = new MvvmLight.RelayCommand(
                                         async () =>
                                         {
                                             Login(Username, Password);

                                         }, () => { return (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password)); }));
            }
        }

        private string _username = string.Empty;
        public string Username
        {
            get
            {
                return _username;
            }

            set
            {
                if (_username == value)
                {
                    return;
                }

                _username = value;
                RaisePropertyChanged(() => Username);
                RaisePropertyChanged(() => GravatarImageUrl);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                if (_password == value)
                {
                    return;
                }

                _password = value;
                RaisePropertyChanged(() => Password);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public string GravatarImageUrl
        {
            get
            {
                return string.IsNullOrEmpty(Username) ? DefatultAvatarImage : Gravatar.GetURL (Username,200);
            }

        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                if (_isBusy == value)
                {
                    return;
                }

                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

    }
}
