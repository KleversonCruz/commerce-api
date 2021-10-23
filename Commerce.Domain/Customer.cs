using Commerce.Domain.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Domain
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<ShoppingSession> ShoppingSessions { get; set; }
        public ICollection<Shop> Shops { get; set; }
        public User User { get; set; }

        [Timestamp]
        public byte[] ModifiedAt { get; set; }
    }
}
