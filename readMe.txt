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
notice how we are retruning a list object in the code. ASP.NET built in JSON serilaizaer converts the list to JSON




Retruning a Model - /api/products2



Retrning specific product based on a path parmeter /api/products2/1
The #1 is a path parmeter extracted by the controller and used as argument when calling the method GetProductById(int id)



Query parmeter - /api/products2/search?name=Mou
path - /api/products2/search?name=Mou
Query - ?name=Mou

Query parms are usally being used for filtering, paging, sorting



Multiple query parms - /api/products2/filter?minPrice=80&maxPrice=500