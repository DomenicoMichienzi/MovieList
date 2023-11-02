# MovieList

This repository contains an ASP.NET Core Web API application that implements REST, GraphQL, and gRPC endpoints in a single 
ASP.NET Core project. This allows you to provide multiple communication 
protocols for clients to interact with your API.

The project is inspired by and follows the book 
[Building Web APIs with ASP.NET Core](https://www.manning.com/books/building-web-apis-with-asp-net-core).

### Key Features

* ASP.NET Core 6
* Entity Framework Core
* REST
* GraphQL using [Hot Chocolate](https://chillicream.com/docs/hotchocolate/v13)
* gRPC
* Data Validation
* Logging
* Caching
* API documentation using Swagger and Swashbuckle
* Dataset on Kaggle [here](https://www.kaggle.com/datasets/akshaypawar7/millions-of-movies)
* SQLite was used as the Database, so it can be uploaded here on GitHub

### A few details
* For space reasons the dataset has been reduced to 41k movies, from over 700k, taking only movies from 2021 to 2022

### Where to go
* Swagger: `https://localhost:7156/swagger`
* GraphQL: `https://localhost:7156/graphql`
