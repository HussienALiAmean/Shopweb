# Shop Web API

This project was generated with Asp.net version 8  and SQL Server DB .

## Development server

Run `Dotnet Run` for a dev server. Navigate to `http://localhost:5002/`.

## Code Dependancy 

1-aspnetcore.authentication.jwtbearer\8.0.7
2-aspnetcore.identity.entityframeworkcore\8.0.7
3-entityframeworkcore\8.0.7
4-entityframeworkcore.design\8.0.7
5-entityframeworkcore.relational\8.0.7
6-entityframeworkcore.sqlserver\8.0.7
7-entityframeworkcore.tools\8.0.7
8-automapper.extensions.microsoft.dependencyinjection\12.0.1


## API List

| End point                     | Request Opject                                                | Respons Opject                                     |
| ---------------------------   | ------------------------------------------------------------- | -------------------------------------------------- |
| POST  /api/Account/register   | {"email": "string","password": "string","role": "string"}     | {"message": "User registered successfully!"}       |
| POST  /api/Account/login      | {"email": "string","password": "string"}                      | Token                                              |
| Get   /api/Orders/all         |                                                               | [{id": 0,"name": "string","description": "string","price": 0,"quantity": 0,"isVisible": true}] |
| Get   /api/Orders             |                                                               | [{"id": 0,"name": "string","description": "string","price": 0,"quantity": 0,"isVisible": true}]|
| POST  /api/Orders             |[{"orderId": 0,"productId": 0,"quantity": 0,"totalePrice": 0}] | Order Done |
| GET   /api/Products           |                                                               |[{"id": 0,"name": "string","description": "string","price": 0,"quantity": 0,"isVisible": true}]|
| POST  /api/Products           | {"id": 0,"name": "string","description": "string","price": 0, "quantity": 0,"isVisible": true}|{"id": 0,"name": "string","description": "string","price": 0, "quantity": 0,"isVisible": true}|
| GET   /api/Products/visible   |                                | [{"id": 0,"name": "string","description": "string","price": 0,"quantity": 0,"isVisible": true}] |
| GET   /api/Products/{id}      |                                    | {"id": 0,"name": "string","description": "string","price": 0,"quantity": 0,"isVisible": true} |
| PUT   /api/Products/{id}        |{"id": 0,"name": "string","description": "string","price": 0,"quantity": 0,"isVisible": true}| {"id": 0,"name": "string","description": "string","price": 0,"quantity": 0,"isVisible": true}|
| DELETE /api/Products/{id}     |                                                               | True   |


