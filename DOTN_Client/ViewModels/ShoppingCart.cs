﻿using DOTN_Models;

namespace DOTN_Client.ViewModels
{
    public class ShoppingCart
    {
        public int ProductId { get; set; }
        public ProductDTO Product { get; set; }
        public int Count { get; set; }

    }
}
