﻿using System.Collections.Generic;

namespace MyPhamUsa.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int SellPrice { get; set; }
        public int AvailableQuantity { get; set; }
        public List<string> ImagePaths { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }

    public class ProductOfStaffViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SellPrice { get; set; }
        public int AvailableQuantity { get; set; }
        public List<string> ImagePaths { get; set; }

    }

    public class ProductCreateViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int SellPrice { get; set; }
        public int ReceiveQuantity { get; set; }
        public List<string> Base64Images { get; set; }
        public List<int> CategoryIds { get; set; }
    }

    public class ClientProductViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int AvailableQuantity { get; set; }
        public List<string> ImagePaths { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
    }

    public class ProductUpdateViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OriginalPrice { get; set; }
        public string SellPrice { get; set; }
        public List<int> RemoveCategoryIds { get; set; }
        public List<int> NewCategoryIds { get; set; }
    }

    public class ProductStaffUpdateViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
