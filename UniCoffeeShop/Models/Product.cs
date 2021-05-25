using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace UniCoffeeShop.Models
{
    public class Product
    {
        [Display(Name = "Product ID")]
        public string Id { get; set; }

        [Display(Name = "Product Name")]
        public string Name { get; set; }

        [Display(Name = "Product Description")]
        public string Description { get; set; }

        [Display(Name = "Product Price (£)")]
        public decimal Price { get; set; }

        [Display(Name = "Product Picture")]
        public byte[] Picture { get; set; }
    }

    public class ProductDBAccessLayer
    {
        private readonly SqlConnection con = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=aspnet-UniCoffeeShop-5E52ED11-3599-43C6-9DAE-5CDEE454529C;Trusted_Connection=True;MultipleActiveResultSets=true");

        public bool AddProduct(Product product)
        {
            if (GetProduct(product.Id) == null)
            {
               
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Data.Product(Id,Name,Description,Price) VALUES(@ProdId,@ProdName,@ProdDescription,@ProdPrice)"))
                    {
                        cmd.Parameters.AddWithValue("@ProdId", product.Id);
                        cmd.Parameters.AddWithValue("@ProdName", product.Name);
                        cmd.Parameters.AddWithValue("@ProdDescription", product.Description);
                        cmd.Parameters.AddWithValue("@ProdPrice", product.Price);
                        

                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return true;
                    }
                
            }
            return false;
        }

        public bool EditProduct(string ProductID, Product newProduct)
        {
            if (GetProduct(newProduct.Id) != null)
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Data.Product SET Id = @ProdId, Name = @ProdName, Description = @ProdDescription, Price = @ProdPrice, Picture = @ProdPicture WHERE Id = @pID"))
                    {
                        cmd.Parameters.AddWithValue("@pID", ProductID);

                        cmd.Parameters.AddWithValue("@ProdId", newProduct.Id);
                        cmd.Parameters.AddWithValue("@ProdName", newProduct.Name);
                        cmd.Parameters.AddWithValue("@ProdDescription", newProduct.Description);
                        cmd.Parameters.AddWithValue("@ProdPrice", newProduct.Price);
                        if (newProduct.Picture != null)
                            cmd.Parameters.AddWithValue("@ProdPicture", newProduct.Picture);

                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }

        public bool DeleteProduct(string ProductID)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Data.Product WHERE Id = @pID"))
                {
                    cmd.Parameters.AddWithValue("@pID", ProductID);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                con.Close();
            }
        }

        public Product GetProduct(string ProductID)
        {
            try
            {
                Product product = new Product();
                using (SqlCommand cmd = new SqlCommand("SELECT Name, Description, Price, Picture FROM Data.Product WHERE Id = @pID"))
                {
                    cmd.Parameters.AddWithValue("@pID", ProductID);
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        reader.Read();
                        product.Id = ProductID;
                        product.Name = reader["Name"].ToString();
                        product.Description = reader["Description"].ToString();
                        product.Price = Convert.ToDecimal(reader["Price"]);
                        try
                        {
                            product.Picture = (byte[])reader["Picture"];
                        }
                        catch (InvalidCastException) // Leave image empty/null
                        { }
                    }
                    con.Close();
                }
                return product;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                con.Close();
            }
        }

        public Product[] GetAllProducts()
        {
            try
            {
                List<Product> products = new List<Product>();
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name, Description, Price, Picture FROM Data.Product"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Product product = new Product
                        {
                            Id = reader["Id"].ToString(),
                            Name = reader["Name"].ToString(),
                            Description = reader["Description"].ToString(),
                            Price = Convert.ToDecimal(reader["Price"])
                        };

                        try
                        {
                            product.Picture = (byte[])reader["Picture"];
                        }
                        catch (InvalidCastException) // Leave image empty/null
                        { }

                        products.Add(product);
                    }
                    con.Close();
                }
                return products.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}