using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace XamarinStore
{
    public class User : INotifyPropertyChanged
    {
        public User()
        {

        }
        public string _firstName = string.Empty;
        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                if (_firstName == value)
                {
                    return;
                }

                _firstName = value;
                RaisePropertyChanged("FirstName");
            }
        }
   
        public string _LastName = string.Empty;
        public string LastName
        {
            get
            {
                return _LastName;
            }

            set
            {
                if (_LastName == value)
                {
                    return;
                }

                _LastName = value;
                RaisePropertyChanged("LastName");
            }
        }

        public string _Email = string.Empty;
        public string Email
        {
            get
            {
                return _Email;
            }

            set
            {
                if (_Email == value)
                {
                    return;
                }

                _Email = value;
                RaisePropertyChanged("Email");
            }
        }

        public string Token { get; set; }

        public string _Phone = string.Empty;
        public string Phone
        {
            get
            {
                return _Phone;
            }

            set
            {
                if (_Phone == value)
                {
                    return;
                }

                _Phone = value;
                RaisePropertyChanged("Phone");
            }
        }

        public string _Address = string.Empty;
        public string Address
        {
            get
            {
                return _Address;
            }

            set
            {
                if (_Address == value)
                {
                    return;
                }

                _Address = value;
                RaisePropertyChanged("Address");
            }
        }

        public string _Address2 = string.Empty;
        public string Address2
        {
            get
            {
                return _Address2;
            }

            set
            {
                if (_Address2 == value)
                {
                    return;
                }

                _Address2 = value;
                RaisePropertyChanged("Address2");
            }
        }

        public string _City = string.Empty;
        public string City
        {
            get
            {
                return _City;
            }

            set
            {
                if (_City == value)
                {
                    return;
                }

                _City = value;
                RaisePropertyChanged("City");
            }
        }

        public string _State = string.Empty;
        public string State
        {
            get
            {
                return _State;
            }

            set
            {
                if (_State == value)
                {
                    return;
                }

                _State = value;
                RaisePropertyChanged("State");
            }
        }

        public string _Country = string.Empty;
        public string Country
        {
            get
            {
                return _Country;
            }

            set
            {
                if (_Country == value)
                {
                    return;
                }

                _Country = value;
                RaisePropertyChanged("Country");
            }
        }

        public string _ZipCode = string.Empty;
        public string ZipCode
        {
            get
            {
                return _ZipCode;
            }

            set
            {
                if (_ZipCode == value)
                {
                    return;
                }

                _ZipCode = value;
                RaisePropertyChanged("ZipCode");
            }
        }

        public async Task<Tuple<bool, string>> IsInformationValid()
        {

            if (string.IsNullOrEmpty(FirstName))
                return new Tuple<bool, string>(false, "First name is required");

            if (string.IsNullOrEmpty(LastName))
                return new Tuple<bool, string>(false, "Last name is required");

            if (string.IsNullOrEmpty(Phone))
                return new Tuple<bool, string>(false, "Phone number is required");

            if (string.IsNullOrEmpty(FirstName))
                return new Tuple<bool, string>(false, "First name is required");

            if (string.IsNullOrEmpty(LastName))
                return new Tuple<bool, string>(false, "Last name is required");

            if (string.IsNullOrEmpty(Address))
                return new Tuple<bool, string>(false, "Address is required");

            if (string.IsNullOrEmpty(City))
                return new Tuple<bool, string>(false, "City is required");

            if (Country.ToLower() == "usa")
            {
                var states = await WebService.Shared.GetStates(await WebService.Shared.GetCountryFromCode(Country));
                if (!states.Contains(State))
                    return new Tuple<bool, string>(false, "State is required");
            }

            if (string.IsNullOrEmpty(Country))
                return new Tuple<bool, string>(false, "Country is required");

            if (string.IsNullOrEmpty(ZipCode))
                return new Tuple<bool, string>(false, "ZipCode is required");


            return new Tuple<bool, string>(true, "");
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}

