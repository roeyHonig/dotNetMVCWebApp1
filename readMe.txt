My 1st API end point - /api/products

How does ASP.NET that when a request to `api/products` the GetProducts() is to be called?

Request
   |
Routing
   |
MapControllers()
   |
ProductsController
   |
GetProducts()

what is actually hepening:

GET /api/products requests arrives

1. Routing
2. מוצא Controller מתאים
3. יוצר מופע של ProductsController
4. מפעיל GetProducts()
5. מחזיר JSON