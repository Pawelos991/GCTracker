using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GC_Tracker_Datalayer.Model;

namespace GCTracker_Tests.Helper
{
    public static class ProductMock
    {
        public static List<Product> GetFakeProductList()
        {
            return new List<Product>()
        {
            new Product
            {
                Id = 0,
                Name = "John Doe",
                Price = 0,
                ProducentCode = "ProdCodeMock",
                ImageAddress = "Img"
            },
            new Product
            {
                Id = 1,
                Name = "NameMock1",
                Price = 32,
                ProducentCode = "ProdCode",
                ImageAddress = "Img123111"
            },
            new Product
            {
                Id = 2,
                Name = "NameMock2",
                Price = 12,
                ProducentCode = "Prd123",
                ImageAddress = "Img1233231"
            },
            new Product
            {
                Id = 3,
                Name = "NameMock3",
                Price = 42,
                ProducentCode = "Prd13",
                ImageAddress = "Img1233123"
            },
            new Product
            {
                Id = 4,
                Name = "NameMock4",
                Price = 76,
                ProducentCode = "Prd17",
                ImageAddress = "Img12354"
            },
            new Product
            {
                Id = 5,
                Name = "NameMock5",
                Price = 2,
                ProducentCode = "Prd134",
                ImageAddress = "Img1233232"
            },
            new Product
            {
                Id = 6,
                Name = "NameMock6",
                Price = 1,
                ProducentCode = "Prd15",
                ImageAddress = "Img1232323"
            },
            new Product
            {
                Id = 7,
                Name = "NameMock7",
                Price = 34,
                ProducentCode = "Prd13",
                ImageAddress = "Img12332"
            },
            new Product
            {
                Id = 8,
                Name = "NameMock8",
                Price = 10,
                ProducentCode = "Prd2",
                ImageAddress = "Img123v"
            },
            new Product
            {
                Id = 9,
                Name = "NameMock19",
                Price = 12,
                ProducentCode = "Prd1",
                ImageAddress = "Img123"
            }
        };
        }
    }
}
