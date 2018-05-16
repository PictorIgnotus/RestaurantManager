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
            Create
        }

        private IRManagerPersistence persistence;
        private List<ShoppingCartItemDTO> products;
        private List<OrderDTO> orders;
        private Dictionary<ShoppingCartItemDTO, DataFlag> productFlag;

        public RManagerModel(IRManagerPersistence persistence)
        {
            if (persistence == null)
                throw new ArgumentNullException(nameof(persistence));
            IsUserLoggedIn = false;
            this.persistence = persistence;
        }

        public Boolean IsUserLoggedIn { get; private set; }

        public IReadOnlyList<ShoppingCartItemDTO> Products
        {
            get { return products; }
        }
        public IReadOnlyList<OrderDTO> Orders
        {
            get { return orders; }
        }

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

        public void CreateProduct(ShoppingCartItemDTO product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (products.Contains(product))
                throw new ArgumentException("The product is already in the collection.", nameof(product));

            product.Id = (products.Count > 0 ? products.Max(b => b.Id) : 0) + 1;
            productFlag.Add(product, DataFlag.Create);
            products.Add(product);

        }

        public async Task LoadAsync()
        {
            products = (await persistence.ReadProductsAsync()).ToList();
        }
    }
}
