using Commerce.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Domain
{
    public class Shop
    {
        public int Id { get; set; }
        public string Cnpj { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Email { get; set; }
        public string ColorTheme { get; set; }
        public string ImageUrl { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Customer> Customers { get; set; }
        public IEnumerable<ShoppingSession> ShoppingSessions { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<User> Users { get; set; }

        [Timestamp]
        public byte[] ModifiedAt { get; set; }
    }
}
