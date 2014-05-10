using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using XamarinStore.Services;
using XamarinStore.Views;

namespace XamarinStore.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _storeTitle;
        public string StoreTitle
        {
            get
            {
                return _storeTitle;
            }
            set
            {
                Set(() => StoreTitle, ref _storeTitle, value);
            }
        }

        INavigationService _navService;
        public MainViewModel(INavigationService navService)
        {
            _navService = navService;
            Init();
        }

        async Task Init()
        {
            StoreTitle = IsInDesignMode
               ? "Xamarin Store D"
               : "Xamarin Store";
            var prod = await WebService.Shared.GetProducts();
            if (prod != null)
            {
                Products = new ObservableCollection<Product>(prod);
            }
        }

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get
            {
                return _products;
            }
            set
            {
                Set(() => Products, ref _products, value);
            }
        }
        /// <summary>
        /// The <see cref="SelectedProduct" /> property's name.
        /// </summary>
        private Product _selectedProduct = null;
        public Product SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }

            set
            {
                if (_selectedProduct == value)
                {
                    return;
                }
                App.RootFrame.Navigate(typeof(ProductDetailPage));

                _selectedProduct = value;
                RaisePropertyChanged(() => SelectedProduct);
                _navService.Navigate("detail");
            }
        }

    }
}
