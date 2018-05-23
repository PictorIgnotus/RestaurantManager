using Admin.Model;
using Admin.Persistence;
using PersistenceManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IRManagerModel model;
        private ObservableCollection<OrderDTO> orders;
        private ObservableCollection<OrderDTO> filteredOrders;
        private ObservableCollection<ProductDTO> products;
        private OrderDTO selectedOrder;
        private Boolean isLoaded;
        private Boolean justTransmitted;
        private String filterName;
        private String filterAddress;
        private Boolean completedOrders;
        private Boolean transmittedOrders;

        public ObservableCollection<OrderDTO> Orders
        {
            get { return filteredOrders; }
            private set
            {
                if (filteredOrders != value)
                {
                    filteredOrders = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ProductDTO> Products
        {
            get { return products; }
            private set
            {
                if (products != value)
                {
                    products = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean IsLoaded
        {
            get { return isLoaded; }
            private set
            {
                if (isLoaded != value)
                {
                    isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        public OrderDTO SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                if (selectedOrder != value)
                {
                    selectedOrder = value;
                    JustTransmitted = selectedOrder == null ? false : selectedOrder.CompletionDate == null;
                    OnPropertyChanged();
                }
            }
        }

        public String FilterName
        {
            get { return filterName; }
            set
            {
                if(filterName != value)
                {
                    filterName = value;
                    OnPropertyChanged();
                }
            }
        }
        public String FilterAddress
        {
            get { return filterAddress; }
            set
            {
                if (filterAddress != value)
                {
                    filterAddress = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean JustTransmitted
        {
            get { return justTransmitted; }
            private set
            {
                if (justTransmitted != value)
                {
                    justTransmitted = value;
                    OnPropertyChanged();
                }            }
        }

        public Boolean CompletedOrders
        {
            get { return completedOrders; }
            set
            {
                if (completedOrders != value)
                {
                    completedOrders = value;
                    OnPropertyChanged();
                }
            }
        }

        public Boolean TransmittedOrders
        {
            get { return transmittedOrders; }
            set
            {
                if (transmittedOrders != value)
                {
                    transmittedOrders = value;
                    OnPropertyChanged();
                }
            }
        }

        public ProductDTO EditedProduct{ get; private set; }

        public DelegateCommand CreateProductCommand {get; private set;}

        public DelegateCommand CompleteOrderCommand { get; private set; }

        public DelegateCommand LogoutCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand SaveChangesCommand { get; private set; }

        public DelegateCommand CancelChangesCommand { get; private set; }

        public DelegateCommand FilterOrdersCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }

        public event EventHandler ExitApplication;

        public event EventHandler ProductEditingStarted;

        public event EventHandler ProductEditingFinished;

        public event EventHandler LogoutSuccess;
        public event EventHandler LogoutFailed;

        public MainViewModel(IRManagerModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            this.model = model;
            this.model.OrderChanged += Model_OrderChanged;
            isLoaded = false;
            justTransmitted = false;
            filterName = "";
            filterAddress = "";
            completedOrders = false;
            transmittedOrders = false;

            CreateProductCommand = new DelegateCommand(param =>
            {
                EditedProduct = new ProductDTO();
                OnProductEditingStarted();
            });
            CompleteOrderCommand = new DelegateCommand(param => CompleteOrder(param as OrderDTO));

            SaveChangesCommand = new DelegateCommand(param => SaveChanges());
            CancelChangesCommand = new DelegateCommand(param => CancelChanges());
            FilterOrdersCommand = new DelegateCommand(param => FilterOrders());

            LogoutCommand = new DelegateCommand(param => LogoutAsync());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
        }

        private async void LogoutAsync()
        {
            try
            {
                Boolean result = await model.LogoutAsync();

                if (result)
                    OnLogoutSuccces();
                else
                    OnLogoutFailed();
            }catch (PersistenceUnavailableException)
            {
                OnMessageApplication("Nincs kapcsolat a szerverrel!");
            }
        }

        private void OnLogoutFailed()
        {
            if (LogoutFailed != null)
                LogoutFailed(this, EventArgs.Empty);
        }

        private void OnLogoutSuccces()
        {
            if (LogoutSuccess != null)
                LogoutSuccess(this, EventArgs.Empty);
        }

        private void FilterOrders()
        {
            Orders.Clear();
            foreach(var order in orders)
            {
                if (order.Name.ToUpper().Contains(FilterName.ToUpper()) && order.Address.ToUpper().Contains(FilterAddress.ToUpper()))
                {
                    if (CompletedOrders && TransmittedOrders || !(CompletedOrders || TransmittedOrders))
                    {
                        Orders.Add(order);
                    }
                    else if(CompletedOrders && order.CompletionDate != null)
                    {
                       Orders.Add(order);
                    }
                    else if(TransmittedOrders && order.CompletionDate == null)
                    {
                        Orders.Add(order);
                    }
                }
            }
        }

        public void CompleteOrder(OrderDTO order)
        {
            if (order == null)
                return;
            if (order.CompletionDate != null)
                return;

            model.CompleteOrder(order);
        }

        public void SaveChanges()
        {
            if(String.IsNullOrEmpty(EditedProduct.Name))
            {
                OnMessageApplication("A termék neve nincs megadva");
                return;
            }
            var nameList = products.Select(p => p.Name).ToList();
            if (nameList.Contains(EditedProduct.Name))
            {
                OnMessageApplication("Már van ilyen nevű termék!");
                return;
            }
            if(String.IsNullOrEmpty(EditedProduct.Price.ToString()))
            {
                OnMessageApplication("A termék ára nincs megadva");
                return;
            }
            
            if(EditedProduct.Category != CategoryType.Coffee && EditedProduct.Category != CategoryType.SoftDrink)
            {
                if(String.IsNullOrEmpty(EditedProduct.Description))
                {
                    OnMessageApplication("Nincs leírás!");
                    return;
                }
            }
            else
            {
                EditedProduct.Hot = false;
                EditedProduct.Vegetarian = false;
                EditedProduct.Description = null;
            }

            model.CreateProduct(EditedProduct);
            Products.Add(EditedProduct);

            OnProductEditingFinished();
        }

        public void CancelChanges()
        {
            EditedProduct = null;
            OnProductEditingFinished();
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }

        private void OnProductEditingStarted()
        {
            if (ProductEditingStarted != null)
                ProductEditingStarted(this, EventArgs.Empty);
        }

        private void OnProductEditingFinished()
        {
            if (ProductEditingFinished != null)
                ProductEditingFinished(this, EventArgs.Empty);
        }

        private async void LoadAsync()
        {
            try
            {
                await model.LoadAsync();
                orders = new ObservableCollection<OrderDTO>(model.Orders); // az adatokat egy követett gyűjteménybe helyezzük
                foreach(OrderDTO order in orders)
                {
                    foreach(var item in order.Items)
                    {
                        order.Price += item.Price * item.Amount; 
                    }
                }
                Orders = new ObservableCollection<OrderDTO>(orders);
                Products = new ObservableCollection<ProductDTO>(model.Products);
                IsLoaded = true;
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        private async void SaveAsync()
        {
            try
            {
                await model.SaveAsync();
                OnMessageApplication("A mentés sikeres!");
            }
            catch(PersistenceUnavailableException)
            {
                OnMessageApplication("A mentés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        private void Model_OrderChanged(object sender, OrderEventArgs e)
        {
            Int32 index = Orders.IndexOf(Orders.FirstOrDefault(order => order.Id == e.OrderId));

            Orders.RemoveAt(index);
            Orders.Insert(index, model.Orders[index]);

            SelectedOrder = Orders[index];
        }
    }
}
