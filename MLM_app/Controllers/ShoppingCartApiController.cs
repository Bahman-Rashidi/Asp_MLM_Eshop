using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MLM_app.ViewModels;

namespace MLM_app.Controllers
{
    public class ShoppingCartApiController : ApiController
    {
        #region Displaying the number of items available in the shopping cart
        // Displaying the number of items in the shopping cart
        public int Get()
        {
            int cartitemscount = 0;
            var Session = HttpContext.Current.Session;
            if (Session["ShoppingCartItems"] != null)
            {
                List<ProductInShoppingCart> products = Session["ShoppingCartItems"] as List<ProductInShoppingCart>;

                foreach (var productInShoppingCart in products)
                {
                    cartitemscount += productInShoppingCart.ProductCount;
                }
            }
            return cartitemscount;
        }
        #endregion

        #region This method adds the item number and its quantity to the shopping cart
        // This method adds the item to the shopping cart
        public int Get(int ProductID)
        {
            int cartitemscount = 0;

            var Session = HttpContext.Current.Session;

            List<ProductInShoppingCart> products = new List<ProductInShoppingCart>();

            if (Session["ShoppingCartItems"] != null)
            {
                products = Session["ShoppingCartItems"] as List<ProductInShoppingCart>;

                ProductInShoppingCart selected = products.Find(p => p.ProductID == ProductID);
                if (selected != null)
                {
                    selected.ProductCount++;
                    int index = products.FindIndex(p => p.ProductID == ProductID);
                    products[index] = selected;
                }
                else
                {
                    selected = new ProductInShoppingCart() { ProductID = ProductID, ProductCount = 1 };
                    products.Add(selected);
                }
                Session["ShoppingCartItems"] = products;

            }
            else
            {
                ProductInShoppingCart selected = new ProductInShoppingCart() { ProductID = ProductID, ProductCount = 1 };
                products.Add(selected);
                Session["ShoppingCartItems"] = products;
            }
            foreach (var productInShoppingCart in products)
            {
                cartitemscount += productInShoppingCart.ProductCount;
            }
            return cartitemscount;
        }
        #endregion
    }
}
