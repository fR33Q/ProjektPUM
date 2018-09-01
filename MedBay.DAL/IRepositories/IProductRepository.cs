using MedBay.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedBay.DAL.IRepositories
{
    public interface IProductRepository
    {
        string InsertProduct(Product product);
        string UpdateProduct(int id, Product product);
        string DeleteProduct(int id);
        Product GetProduct(int id);
        List<Product> GetAllProducts();
        List<Product> GetProductsByCategory(int categoryId);
        int GetCategoryId(string categoryName);


    }
}
