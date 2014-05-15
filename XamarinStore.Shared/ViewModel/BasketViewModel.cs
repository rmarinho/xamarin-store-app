using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using XamarinStore.Services;

namespace XamarinStore.ViewModel
{
    public class BasketViewModel : ViewModelBase
    {
        INavigationService _navService;
        public BasketViewModel(INavigationService navService)
        {
            _navService = navService;
            UpdateTotals();
        }

        public void UpdateTotals()
        {
            ProductCount = _currentOrder != null ?  _currentOrder.Products.Count : 0;
            if (ProductCount == 0)
            {
                TotalAmount = "";
                return;
            }
            var total = CurrentOrder.Products.Sum(x => x.Price);
            TotalAmount = total.ToString("C");
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
                                          }, ()=> ProductCount > 0));
            }
        }
    }
}
