using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShoppingCart;
using ShoppingCart.ShoppingCart;

namespace ShoppingCart.ProductCatalog
{
    public interface IProductCatalogClient
    {
        Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds);
    }
}
