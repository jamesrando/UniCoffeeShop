using System;

namespace UniCoffeeShop.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public uint Quantity { get; set; }

        public decimal GetTotal()
        {
            return Product.Price * Quantity;
        }

        public override string ToString()
        {
            return string.Format("ProductId={0},Quantity={1}", Product.Id, Quantity);
        }

        public static CartItem ConvertStringToCartItem(string item)
        {
            try
            {
                string[] itemSplit = item.Split(',');

                CartItem cItem = new CartItem
                {
                    Product = new ProductDBAccessLayer().GetProduct(itemSplit[0].Split('=')[1]),
                    Quantity = Convert.ToUInt32(itemSplit[1].Split('=')[1])
                };

                return cItem;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
    }
}
