using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XamarinStore.Services;

namespace XamarinStore.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        #region Ctor
        INavigationService _navService;
        public MainViewModel(INavigationService navService)
        {
            _navService = navService;
#pragma warning disable 4014
            Init();
#pragma warning restore 4014
        } 
        #endregion

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

        #region Properties
        private bool isAnimating  = false;
        
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
                if (_selectedProduct != null)
                {
                    _navService.Navigate("detail");
                    //TIP: we can't bind or set the property to the Color property yet because this item isn't part of the collection, they we have a navy color but is not the same object
                    //will fix that if i have time 
                    //SelectedColor = _selectedProduct.Color;
                    SelectedColor = _selectedProduct.Colors.FirstOrDefault(c => c.Name.Equals(_selectedProduct.Color.Name, StringComparison.OrdinalIgnoreCase));
                    SelectedSize = _selectedProduct.Sizes.FirstOrDefault(c => c.Name.Equals(_selectedProduct.Size.Name, StringComparison.OrdinalIgnoreCase));
                }
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

                isAnimating = false;
                RaisePropertyChanged(() => ProductCount);
                AddToBasketCommand.RaiseCanExecuteChanged();
            }
        } 
        #endregion

        #region Commands
        
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
                                              //little hack to handle the cart animation
                                              isAnimating = true;
                                              AddToBasketCommand.RaiseCanExecuteChanged();
                                          }, () => { return (SelectedColor != null && SelectedSize != null && !isAnimating); }));
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

        #endregion

    }
}