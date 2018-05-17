using Admin.Model;
using Admin.Persistence;
using PersistenceManager;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private IRManagerModel model;
        private ObservableCollection<OrderDTO> orders;
        private ObservableCollection<ShoppingCartItemDTO> items;
        private ObservableCollection<ProductDTO> products;
        private OrderDTO selectedOrder;
        private Boolean isLoaded;

        public ObservableCollection<OrderDTO> Orders
        {
            get { return orders; }
            private set
            {
                if (orders != value)
                {
                    orders = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<ShoppingCartItemDTO> Items
        {
            get { return items; }
            private set
            {
                if(items != value)
                {
                    items = value;
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
                    OnPropertyChanged();
                }
            }
        }

        public ProductDTO EditedProduct { get; private set; }

        public DelegateCommand CreateProductCommand {get; private set;}

        public DelegateCommand CompleteOrderCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand SaveChangesCommand { get; private set; }

        public DelegateCommand CancelChangesCommand { get; private set; }

        public DelegateCommand LoadCommand { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }

        public event EventHandler ExitApplication;

        public event EventHandler ProductEditingStarted;

        public event EventHandler ProductEditingFinished;

        public MainViewModel(IRManagerModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            this.model = model;
            this.model.OrderChanged += Model_OrderChanged;
            isLoaded = false;

            CreateProductCommand = new DelegateCommand(param =>
            {
                EditedProduct = new ProductDTO();
                OnProductEditingStarted();
            });
            CompleteOrderCommand = new DelegateCommand(param => CompleteOrder(param as OrderDTO));

            SaveChangesCommand = new DelegateCommand(param => SaveChanges());
            CancelChangesCommand = new DelegateCommand(param => CancelChanges());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
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
                OnMessageApplication("Az étel neve nincs megadva");
                return;
            }
            if(String.IsNullOrEmpty(EditedProduct.Price.ToString()))
            {
                OnMessageApplication("Az étel ára nincs megadva");
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
                if(!String.IsNullOrEmpty(EditedProduct.Description))
                {
                    OnMessageApplication("Italokhoz nincs leírás");
                    return;
                }
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
                Orders = new ObservableCollection<OrderDTO>(model.Orders); // az adatokat egy követett gyűjteménybe helyezzük
                foreach(OrderDTO order in Orders)
                {
                    foreach(var item in order.Items)
                    {
                        order.Price += item.Price * item.Amount; 
                    }
                }
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
