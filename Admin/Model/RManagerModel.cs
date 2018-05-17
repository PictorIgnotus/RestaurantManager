using Admin.Persistence;
using PersistenceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Model
{
    public class RManagerModel : IRManagerModel
    {

        private enum DataFlag
        {
            Create,
            Update
        }

        private IRManagerPersistence persistence;
        private List<ProductDTO> products;
        private List<OrderDTO> orders;
        private Dictionary<OrderDTO, DataFlag> orderFlags;
        private Dictionary<ProductDTO, DataFlag> productFlags;

        public RManagerModel(IRManagerPersistence persistence)
        {
            if (persistence == null)
                throw new ArgumentNullException(nameof(persistence));
            IsUserLoggedIn = false;
            this.persistence = persistence;
        }

        public Boolean IsUserLoggedIn { get; private set; }

        public IReadOnlyList<ProductDTO> Products
        {
            get { return products; }
        }
        public IReadOnlyList<OrderDTO> Orders
        {
            get { return orders; }
        }

        public event EventHandler<OrderEventArgs> OrderChanged;

        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            IsUserLoggedIn = await persistence.LoginAsync(userName, userPassword);
            return IsUserLoggedIn;
        }

        public async Task<Boolean> LogoutAsync()
        {
            if (!IsUserLoggedIn)
                return true;
            IsUserLoggedIn = !(await persistence.LogoutAsync());

            return IsUserLoggedIn;
        }

        public void CreateProduct(ProductDTO product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (products.Contains(product))
                throw new ArgumentException("The product is already in the collection.", nameof(product));

            product.Id = (products.Count > 0 ? products.Max(b => b.Id) : 0) + 1;
            productFlags.Add(product, DataFlag.Create);
            products.Add(product);

        }

        public void CompleteOrder(OrderDTO order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            OrderDTO orderToModify = orders.FirstOrDefault(o => o.Id == order.Id);

            if (orderToModify == null)
                throw new ArgumentException("The order does not exist.", nameof(order));

            orderToModify.CompletionDate = new DateTime();
            orderToModify.CompletionDate = DateTime.Now;

            orderFlags.Add(order, DataFlag.Update);

            OnOrderComplete(order.Id);
        }

        public async Task LoadAsync()
        {
            products = (await persistence.ReadProductsAsync()).ToList();
            orders = (await persistence.ReadOrdersAsync()).ToList();

            orderFlags = new Dictionary<OrderDTO, DataFlag>();
            productFlags = new Dictionary<ProductDTO, DataFlag>();
        }

        public async Task SaveAsync()
        {
            List<ProductDTO> productsToSave = productFlags.Keys.ToList();

            foreach(ProductDTO product in productsToSave)
            {
                Boolean result = await persistence.CreateProductAsync(product); ;

                if (!result)
                    throw new InvalidOperationException("Operation " + productFlags[product] + " failed on product " + product.Id);

                productFlags.Remove(product);
            }

            List<OrderDTO> ordersToSave = orderFlags.Keys.ToList();

            foreach(OrderDTO order in ordersToSave)
            {
                Boolean result = await persistence.UpdateOrderAsync(order); ;

                if (!result)
                    throw new InvalidOperationException("Operation " + orderFlags[order] + " failed on product " + order.Id);

                orderFlags.Remove(order);
            }
        }

        private void OnOrderComplete(Int32 orderId)
        {
            if (OrderChanged != null)
                OrderChanged(this, new OrderEventArgs { OrderId = orderId });
        }
    }
}
