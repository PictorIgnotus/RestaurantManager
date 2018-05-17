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

        Task<IEnumerable<ProductDTO>> ReadProductsAsync();

        Task<IEnumerable<OrderDTO>> ReadOrdersAsync();

        Task<Boolean> CreateProductAsync(ProductDTO product);

        Task<Boolean> UpdateOrderAsync(OrderDTO order);
    }
}
