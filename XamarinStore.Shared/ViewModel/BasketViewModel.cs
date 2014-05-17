using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XamarinStore.Helpers;
using XamarinStore.Services;

namespace XamarinStore.ViewModel
{
    public class BasketViewModel : ViewModelBase
    {
        #region Ctor

        INavigationService _navService;
        public BasketViewModel(INavigationService navService)
        {
            _navService = navService;
            UpdateTotals();
            GetCountries();
        } 

        #endregion

        #region Private Methods
        void UpdateTotals()
        {
            ProductCount = _currentOrder != null ? _currentOrder.Products.Count : 0;
            if (ProductCount == 0)
            {
                TotalAmount = "";
                return;
            }
            var total = CurrentOrder.Products.Sum(x => x.Price);
            TotalAmount = total.ToString("C");


            // GetStates();
        }

        async void GetCountries()
        {
            var countries = await WebService.Shared.GetCountries();
            Countries = new ObservableCollection<Country>(countries);
        }

        async void GetStates()
        {
            var states = await WebService.Shared.GetStates(SelectedCountry.Name);
            States = new ObservableCollection<string>(states);
        }

        async Task PlaceOrder()
        {
            if (SelectedCountry == null)
            {
                await MessageBox.ShowAsync("You need to Selecte a country", "Error",
                                           MessageBoxButton.OK);
                return;
            }
            User.Country = SelectedCountry.Code;
            var isValid = await User.IsInformationValid();
            if (!isValid.Item1)
            {
                await MessageBox.ShowAsync(isValid.Item2, "Error",
                                             MessageBoxButton.OK);
                return;
            }

            _navService.Navigate("processing");
            //if (ShippingComplete != null)
            //    ShippingComplete(this, EventArgs.Empty);
        } 
        #endregion

        public async void ProcessOrder()
        {
            IsBusy = true;
            OrderStatus = "We are processing your order, please wait";
            var result = await WebService.Shared.PlaceOrder(User);
            IsOrderComplete = result.Success;
            OrderStatus = IsOrderComplete ? "Your order has been placed!" : result.Message;
            IsBusy = false;
        }

        #region Properties 
        public User User { get { return WebService.Shared.CurrentUser; } }


        private string _orderStatus = "Processing...";
        public string OrderStatus
        {
            get
            {
                return _orderStatus;
            }

            set
            {
                if (_orderStatus == value)
                {
                    return;
                }

                _orderStatus = value;
                RaisePropertyChanged(() => OrderStatus);
            }
        }

        private ObservableCollection<Country> _countries = null;
        public ObservableCollection<Country> Countries
        {
            get
            {
                return _countries;
            }

            set
            {
                if (_countries == value)
                {
                    return;
                }

                _countries = value;
                RaisePropertyChanged(() => Countries);
            }
        }

        private Country _selectedCountry = null;
        public Country SelectedCountry
        {
            get
            {
                return _selectedCountry;
            }

            set
            {
                if (_selectedCountry == value)
                {
                    return;
                }

                _selectedCountry = value;
                RaisePropertyChanged(() => SelectedCountry);
                PlaceOrderCommand.RaiseCanExecuteChanged();
                GetStates();
            }
        }

        private ObservableCollection<string> _states = null;
        public ObservableCollection<string> States
        {
            get
            {
                return _states;
            }

            set
            {
                if (_states == null)
                    _states = value;
                else
                {
                    _states.Clear();
                    foreach (var state in value)
                    {
                        _states.Add(state);
                    }
                }



                RaisePropertyChanged(() => States);
            }
        }

        private string _selectedState = "";
        public string SelectedState
        {
            get
            {
                return _selectedState;
            }

            set
            {
                if (_selectedState == value)
                {
                    return;
                }

                _selectedState = value;
                RaisePropertyChanged(() => SelectedState);
            }
        }


        private string _totalAmount = "100€";
        public string TotalAmount
        {
            get
            {
                return _totalAmount;
            }

            set
            {
                if (_totalAmount == value)
                {
                    return;
                }

                _totalAmount = value;
                RaisePropertyChanged(() => TotalAmount);
            }
        }

        private int _productCount = 0;
        public int ProductCount
        {
            get
            {
                return _productCount;
            }

            set
            {
                if (_productCount == value)
                {
                    return;
                }

                _productCount = value;


                RaisePropertyChanged(() => ProductCount);
                CheckOutCommand.RaiseCanExecuteChanged();
            }
        }

        private Order _currentOrder = null;
        public Order CurrentOrder
        {
            get
            {
                return _currentOrder;
            }

            set
            {

                _currentOrder = value;
                RaisePropertyChanged(() => CurrentOrder);
                UpdateTotals();
            }
        }


        private bool _isbusy = false;
        public bool IsBusy
        {
            get
            {
                return _isbusy;
            }

            set
            {
                if (_isbusy == value)
                {
                    return;
                }

                _isbusy = value;


                RaisePropertyChanged(() => IsBusy);
                CheckOutCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isorderComplete = false;
        public bool IsOrderComplete
        {
            get
            {
                return _isorderComplete;
            }

            set
            {
                if (_isorderComplete == value)
                {
                    return;
                }

                _isorderComplete = value;


                RaisePropertyChanged(() => IsOrderComplete);
                CheckOutCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion

        #region Commands

        private RelayCommand _checkOutCommand;
        public RelayCommand CheckOutCommand
        {
            get
            {
                return _checkOutCommand
                    ?? (_checkOutCommand = new RelayCommand(
                                          () =>
                                          {
                                              _navService.Navigate("login");
                                          }, () => ProductCount > 0));
            }
        }

        private RelayCommand _placeOrderCommand;
        public RelayCommand PlaceOrderCommand
        {
            get
            {
                return _placeOrderCommand
                    ?? (_placeOrderCommand = new RelayCommand(
                                         async () =>
                                         {
                                             await PlaceOrder();
                                         }, () =>
                                         {

                                             return true;
                                         }));
            }

        }

        private RelayCommand _doneCommand;
        public RelayCommand DoneCommand
        {
            get
            {
                return _doneCommand
                    ?? (_doneCommand = new RelayCommand(
                                          () =>
                                          {
                                              _navService.Navigate("done");
                                          }, () =>
                                         {

                                             return true;
                                         }));
            }

        }
        
        #endregion

    }
}
