using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using XamarinStore.Services;
using XamarinStore.Views;
using System.Linq;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
namespace XamarinStore.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
       
        INavigationService _navService;
        public MainViewModel(INavigationService navService)
        {
            _navService = navService;
            Init();
        }

        async Task Init()
        {
          
            var prod = await WebService.Shared.GetProducts();
            if (prod != null)
            {
                Products = new ObservableCollection<Product>(prod);
            }
            if (IsInDesignMode)
                SelectedProduct = Products.FirstOrDefault();
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

        private ProductSize _selectedSize;
        public ProductSize SelectedSize
        {
            get
            {
                return _selectedSize;
            }

            set
            {
                _selectedSize = value;
                RaisePropertyChanged(() => SelectedSize);
            }
        }

        private ProductColor _selectedColor;
        public ProductColor SelectedColor
        {
            get
            {
                return _selectedColor;
            }

            set
            {
                _selectedColor = value;
                RaisePropertyChanged(() => SelectedColor);
            }
        }

        private Product _selectedProduct = null;
        public Product SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }

            set
            {
        
                _selectedProduct = value;
                RaisePropertyChanged(() => SelectedProduct);
                _navService.Navigate("detail");
                //TIP: we can't bind or set the property to the Color property yet because this item isn't part of the collection, they we have a navy color but is not the same object
                //will fix that if i have time 
                //SelectedColor = _selectedProduct.Color;
                SelectedColor = _selectedProduct.Colors.FirstOrDefault(c => c.Name.Equals(_selectedProduct.Color.Name, StringComparison.OrdinalIgnoreCase));
                SelectedSize = _selectedProduct.Sizes.FirstOrDefault(c => c.Name.Equals(_selectedProduct.Size.Name, StringComparison.OrdinalIgnoreCase));
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

            }
        }

        private RelayCommand _addToBasketCommand;
        public RelayCommand AddToBasketCommand
        {
            get
            {
                return _addToBasketCommand
                    ?? (_addToBasketCommand = new RelayCommand(
                                          () =>
                                          {
                                              SelectedProduct.Color = SelectedColor;
                                              SelectedProduct.Size = SelectedSize;
                                              WebService.Shared.CurrentOrder.Add(SelectedProduct);
                                              ProductCount++;
                                          }, () => { return (SelectedColor != null && SelectedSize != null); }));
            }
        }

        private RelayCommand _showBasketCommand;
        public RelayCommand ShowBasketCommand
        {
            get
            {
                return _showBasketCommand
                    ?? (_showBasketCommand = new RelayCommand(
                                          () =>
                                          {
                                              SimpleIoc.Default.GetInstance<BasketViewModel>().CurrentOrder = WebService.Shared.CurrentOrder;
                                              _navService.Navigate("basket");
                                          }));
            }
        }


    }
}