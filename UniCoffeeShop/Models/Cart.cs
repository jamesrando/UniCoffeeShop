using System;
using System.Collections.Generic;
using System.Linq;

namespace UniCoffeeShop.Models
{
    public class Cart
    {
        public List<CartItem> ShoppingCart = new List<CartItem>();
        public string CurrencyCode = "£";

        public decimal GetTotal()
        {
            decimal total = 0.0M;

            foreach (CartItem item in ShoppingCart)
                total += item.GetTotal();

            return total;
        }

        public decimal GetDiscount()
        {
            return 0.0M;
        }

        public void AddItemToCart(CartItem item)
        {
            try
            {
                CartItem currentCartItem = ShoppingCart.First(i => i.Product.Id == item.Product.Id);
                if (currentCartItem != null)
                    currentCartItem.Quantity += item.Quantity;
                else
                    ShoppingCart.Add(item);
            }
            catch (Exception)
            {
                ShoppingCart.Add(item);
            }
        }

        public bool RemoveItemFromCart(string ProductID)
        {
            CartItem currentCartItem = ShoppingCart.First(i => i.Product.Id == ProductID);
            if (currentCartItem != null)
            {
                ShoppingCart.Remove(currentCartItem);
                return true;
            }
            return false;
        }

        public bool AdjustQuantity(string productId, uint newQuantity)
        {
            CartItem currentCartItem = ShoppingCart.First(i => i.Product.Id == productId);
            if (currentCartItem != null)
            {
                currentCartItem.Quantity = newQuantity;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            if (ShoppingCart.Count > 1)
            {
                List<string> cart = new List<string>();

                foreach (CartItem cItem in ShoppingCart)
                    cart.Add(cItem.ToString());

                return string.Join('|', cart);
            }
            else if (ShoppingCart.Count == 1)
                return ShoppingCart[0].ToString();

            return null;
        }

        public static List<CartItem> ConvertStringToCartItems(string items)
        {
            if (!String.IsNullOrWhiteSpace(items))
            {
                List<CartItem> cart = new List<CartItem>();

                if (items.Contains('|'))
                {
                    foreach (string item in items.Split('|'))
                    {
                        CartItem cItem = CartItem.ConvertStringToCartItem(item);
                        if (cItem != null)
                            cart.Add(cItem);
                    }
                }
                else
                {
                    CartItem item = CartItem.ConvertStringToCartItem(items);
                    if (item != null)
                        cart.Add(item);
                }

                return cart;

            }
            return new List<CartItem>();
        }
    }
}
