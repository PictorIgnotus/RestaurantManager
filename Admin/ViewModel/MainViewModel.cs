using Admin.Model;
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
        private ObservableCollection<ShoppingCartItemDTO> products;
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

        public ObservableCollection<ShoppingCartItemDTO> Products
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


        public event EventHandler ExitApplication;

        public event EventHandler ProductEditingStarted;

        public event EventHandler ProductEditingFinished;

        public MainViewModel(IRManagerModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");
            this.model = model;
            isLoaded = false;

            CreateProductCommand = new DelegateCommand(param =>
            {
                EditedProduct = new ProductDTO();
                OnProductEditingStarted();
            });

            SaveChangesCommand = new DelegateCommand(param => SaveChanges());
            CancelChangesCommand = new DelegateCommand(param => CancelChanges());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
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
                ProductEditingStarted(this, EventArgs.Empty);
        }
    }
}
