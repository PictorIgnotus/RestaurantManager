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

        public async Task<IEnumerable<ProductDTO>> ReadProductsAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/products/");
                if(response.IsSuccessStatusCode)
                {
                    IEnumerable<ProductDTO> products = await response.Content.ReadAsAsync<IEnumerable<ProductDTO>>();

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
                    IEnumerable<OrderDTO> orders = await response.Content.ReadAsAsync<IEnumerable<OrderDTO>>();
                    return orders;
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

        public async Task<Boolean> CreateProductAsync(ProductDTO product)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync("api/products/", product);
                product.Id = (await response.Content.ReadAsAsync<ProductDTO>()).Id;
                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> UpdateOrderAsync(OrderDTO order)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync("api/orders/", order);
                return response.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("api/account/login/" + userName + "/" + userPassword);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
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
    }
}
