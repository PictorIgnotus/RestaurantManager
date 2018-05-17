using PersistenceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Model
{
    public interface IRManagerModel
    {
        IReadOnlyList<OrderDTO> Orders { get; }

        IReadOnlyList<ProductDTO> Products { get; }

        Boolean IsUserLoggedIn { get; }

        event EventHandler<OrderEventArgs> OrderChanged;

        Task<Boolean> LoginAsync(String userName, String userPassword);

        Task<Boolean> LogoutAsync();

        Task LoadAsync();

        Task SaveAsync();

        void CompleteOrder(OrderDTO order);

        void CreateProduct(ProductDTO product);
    }
}
