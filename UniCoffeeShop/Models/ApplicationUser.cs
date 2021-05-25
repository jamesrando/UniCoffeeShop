using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UniCoffeeShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ShoppingCart { get; set; }

        public Cart GetShoppingCart()
        {
            Cart cart = new Cart
            {
                ShoppingCart = Cart.ConvertStringToCartItems(ShoppingCart)
            };
            return cart;
        }

        public void SetShoppingCart(Cart cart)
        {
            ShoppingCart = cart.ToString();
        }

        public int GetShoppingCartCount()
        {
            Cart cart = GetShoppingCart();
            if (cart.ShoppingCart != null)
                return cart.ShoppingCart.Count;
            return 0;
        }
    }
}
