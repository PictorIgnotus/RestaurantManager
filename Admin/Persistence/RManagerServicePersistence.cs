using PersistenceManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Persistence
{
    public class RManagerServicePersistence : IRManagerPersistence
    {
        private HttpClient client;

        public RManagerServicePersistence(String baseAddress)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(baseAddress);
        }

        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/account/login/" + userName + "/" + userPassword);
                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/account/logout");
                return !response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<ShoppingCartItemDTO>> ReadProductsAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/buildings/");
                if(response.IsSuccessStatusCode)
                {
                    IEnumerable<ShoppingCartItemDTO> products = await response.Content.ReadAsAsync<IEnumerable<ShoppingCartItemDTO>>();

                    return products;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch(Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<OrderDTO>> ReadOrdersAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/orders/");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IEnumerable<OrderDTO>>();
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch(Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> CreateProductAsync(ShoppingCartItemDTO product)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/products/", product);
                product.Id = (await response.Content.ReadAsAsync<ShoppingCartItemDTO>()).Id;
                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
    }
}
