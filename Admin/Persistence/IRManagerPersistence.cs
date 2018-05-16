using PersistenceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Persistence
{
    public interface IRManagerPersistence
    {
        Task<Boolean> LoginAsync(String username, String userPassword);

        Task<Boolean> LogoutAsync();

        Task<IEnumerable<ShoppingCartItemDTO>> ReadProductsAsync();

        Task<IEnumerable<OrderDTO>> ReadOrdersAsync();
    }
}
