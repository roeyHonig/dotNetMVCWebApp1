REST API
    ↓
HTTP Verbs
    ↓
Controllers
    ↓
Swagger
    ↓
DTOs
    ↓
Validation
    ↓
Authentication (JWT)
    ↓
Authorization (RBAC)
    ↓
OAuth2 (Conceptual)
    ↓
Blazor / Front-End
    ↓
Google Login Demo










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


POST - create new entites
DTO - Data Transfer Object - only data properties no logic - ment to transfer data between different layers of the app BE <-> FE
Why not use the entire entity - securty, we might not want to expose all the fileds
During POST requests the controller converts the json in the request body to the DTO via model binding


| Action           | HTTP Verb | URL                             |
| ---------------- | --------- | ------------------------------- |
| Get all products | GET       | `/api/products`                 |
| Get a product    | GET       | `/api/products2/{id}`           |
| Search           | GET       | `/api/products2/search?name=...`|
| Create a product | POST      | `/api/products2`                |
| Update a product | PUT       | `/api/products2/{id}`           |
| Delete a product | DELETE    | `/api/products2/{id}`           |





JWT Authentication – Summary

1. Login
- Client sends username + password to /api/auth/login
- Server validates credentials
- If valid → server generates JWT token

JWT contains:
- Header (algorithm)
- Payload (user info / claims)
- Signature (signed with server secret)

Server returns:
{
  "token": "<JWT>"
}

--------------------------------------------------

2. Using the Token
- Client stores the token
- Sends it on every request:

Authorization: Bearer <JWT>

Example:
GET /api/products3

--------------------------------------------------

3. Server Validation Flow
- ASP.NET Core Authentication Middleware intercepts request
- Validates:
  - Signature is correct
  - Token is not expired
- If valid → request continues to controller
- If invalid → request is rejected

--------------------------------------------------

4. Authorization Results
- No/invalid token → 401 Unauthorized
- Valid token → 200 OK (access granted)

--------------------------------------------------

use https://www.jwt.io/ to decode JWT



==========================
JWT (Local Authentication)
==========================

Client
  |
  | 1. Login (username/password)
  v
Auth Server (your API)
  |
  | 2. Validate credentials
  |
  | 3. Generate JWT (signed locally)
  v
Client receives JWT
  |
  | 4. Send request with:
  |    Authorization: Bearer <JWT>
  v
API Middleware (ASP.NET Core)
  |
  | 5. Validate token locally:
  |    - Verify signature (secret key)
  |    - Check expiration
  |    - Check claims (roles)
  |
  | 6. If valid → continue
  | 7. If invalid → reject (401/403)
  v
Controller executes logic


Key idea:
- No external calls
- Fully stateless
- Fast validation



=========================================
OAuth2 / OpenID Connect (External Provider)
=========================================

Client
  |
  | 1. Login via external provider (Google)
  v
Google / Identity Provider
  |
  | 2. Authenticate user
  | 3. Issue Access Token / ID Token
  v
Client receives external token
  |
  | 4. Send request to your API:
  |    Authorization: Bearer <Google Token>
  v
API Middleware (ASP.NET Core)
  |
  | 5. Validate token:
  |    Option A (common):
  |      - Validate signature using Google's public keys (JWKS)
  |      - Check expiration + issuer + audience
  |
  |    Option B (less common):
  |      - Call Google introspection endpoint
  |      - Ask: "Is this token still valid?"
  |
  | 6. If valid → continue
  | 7. If invalid → reject (401/403)
  v
Controller executes logic


Key idea:
- Identity handled externally
- May require external metadata (public keys)
- Slight overhead vs local JWT



==========================
MAIN DIFFERENCE (IMPORTANT)
==========================

JWT (self-issued):
- You validate locally
- No network call needed
- Fast
- Stateless

OAuth2 (external provider):
- Token issued by external system (Google, Microsoft, etc.)
- Validation may use external public keys or introspection
- More flexible
- Supports SSO (Single Sign-On)






=========================================
RBAC + OAuth2 (Google example)
=========================================

User
  |
  | 1. Login with Google
  v
Google Identity Provider
  |
  | 2. Authenticate user
  |    (returns user identity, e.g. email, name, sub)
  v
Your API / Server
  |
  | 3. Lookup user in local database
  |    (map Google identity → roles in your system)
  |
  |    Example database table:
  |    -------------------------------------
  |    GoogleId      Email                Role
  |    -------------------------------------
  |    123456789     john@example.com     Admin
  |    987654321     jane@example.com     User
  |
  v
Determine role for user
  |
  | 4. Generate local JWT including role claim:
  |      - ClaimTypes.Name → user email
  |      - ClaimTypes.Role → role from database
  v
Client receives JWT
  |
  | 5. Subsequent requests to API:
  |      - Include JWT in Authorization header
  |      - ASP.NET Core validates signature + expiration
  |      - [Authorize(Roles = "...")] enforces permissions
  v
Controller executes authorized logic.
