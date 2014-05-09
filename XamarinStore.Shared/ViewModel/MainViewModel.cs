using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

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

        public MainViewModel()
        {
            Init();
        }

        async Task Init()
        {
            StoreTitle = IsInDesignMode
               ? "Xamarin Store DesignMode"
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

    }
}
