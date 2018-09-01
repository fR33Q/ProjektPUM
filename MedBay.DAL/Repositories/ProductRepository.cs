using MedBay.DAL.Entity;
using MedBay.DAL.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedBay.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public string InsertProduct(Product product)
        {
            try
            {

                MedbayEntities context = new MedbayEntities();
                context.Product.Add(product);
                context.SaveChanges();

                return product.Product_Name + " was succesfully inserted";
            }
            catch (Exception e)
            {
                return "Error:" + e;
            }
        }

        public string UpdateProduct(int id, Product product)
        {
            try
            {
                MedbayEntities context = new MedbayEntities();

                Product oldProduct = context.Product.Find(id);

                oldProduct.Product_Name = product.Product_Name;
                oldProduct.Price = product.Price;
                oldProduct.Product_Description = product.Product_Description;
                oldProduct.PictureFileName = product.PictureFileName;
                oldProduct.CategoryID = product.CategoryID;


                context.SaveChanges();
                return product.Product_Name + " was succesfully updated";

            }
            catch (Exception e)
            {
                return "Error:" + e;
            }
        }

        public string DeleteProduct(int id)
        {
            try
            {
                MedbayEntities context = new MedbayEntities();
                Product deleteProduct = context.Product.Find(id);

                context.Product.Attach(deleteProduct);
                context.Product.Remove(deleteProduct);
                context.SaveChanges();

                return deleteProduct.Product_Name + "was succesfully deleted";
            }
            catch (Exception e)
            {
                return "Error:" + e;
            }
        }

        public Product GetProduct(int id)
        {
            try
            {
                using (MedbayEntities context = new MedbayEntities())
                {
                    Product product = context.Product.Find(id);
                    return product;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Product> GetAllProducts()
        {
            try
            {
                using (MedbayEntities context = new MedbayEntities())
                {
                    List<Product> products = (from x in context.Product
                                               select x).ToList();
                    return products;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
        public List<Product> GetProductsByCategory(int categoryId)
        {
            try
            {
                using (MedbayEntities context = new MedbayEntities())
                {
                    List<Product> products = (from x in context.Product
                                               where x.CategoryID == categoryId
                                               select x).ToList();
                    return products;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int GetCategoryId(string categoryName)
        {
                using (MedbayEntities context = new MedbayEntities())
                {
                    int categoryId = (from x in context.Category
                                      where x.Name == categoryName
                                      select x.Id).SingleOrDefault();
                    return categoryId;
                }
            
        }
    }
}
