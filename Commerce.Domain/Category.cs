using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Commerce.Domain
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        [Timestamp]
        public byte[] ModifiedAt { get; set; }
    }
}
