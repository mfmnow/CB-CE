# Mfm.Bc.CurrencyExchanger project

## 1. Tech Stack
- .NET Core 8
- Moq (Unit testing)
- FluentAssertions (Unit testing)
- Swagger (API Docos)
- Visual Studio 2022

## 2. List of APIs
- `get-latest-exchange-rates/{currency}` -> Gets latest exchange rates against a given currencies.
- `convert-currency-amount/{fromCurrency}/{toCurrency}/{amount}` -> Converts an amount of a currency to another currency given current exchange rate
- `get-historical-exchange-rates-page/{currency}/{startDate}/{endDate}/{pageNumber}` -> Returns a single page of historical rates data of a currency.

## 3. Addressing excluded currencies
- Req: Allow users to convert amounts between different currencies. In case of TRY, PLN, THB, and MXN currency conversions, the endpoint should return a bad response, and these currencies should be excluded from the response.
- List of excluded currencies is in appsettings (overridable per env)
- It is part of the config Singleton of the App 

## 4. Addressing protecting CurrencyExchanger and Frankfurter APIs
### 4.1 Rate Limiting
- Using built-in .NET Core 8 RateLimiter Middleware
- WindowInSeconds and PermitLimit are configurable per env
### 4.2 Caching
- The biggest and heaviest call to retrieve historical rates (fortunatly cacheable since it does not change) is cached against keys.
- This should improve performance and call third party less times
- I implemented a super simple ConcurrentDictionary caching logic.. maybe in Prod, Redis would be a better idea with load balancers
### 4.3 Future improvements
- Maybe queue requests and control how much calls we execute against third party APIs

## 5. Docos
### 5.1 Swaggar
- Used Swagger against Dev only for security purposes.
- Swagger integrates with assemblies and XML docs to give max level of documentation.
### 5.2 Code comments
- XML code comments is enforced in some projects

## 6. DI, HTTPClient and Configs
### 6.1 DI
- Used MS DI which is built-in .NET Core
- Used Singletons and Transits as needed
### 6.2 HTTPClient
- Used .NET Core built-in HTTPClient registration and assigning to services
### 6.3 Configs
- All Configs are Singletons and loaded on App start once.

## 7. Error handling
- Create an Error handling Middleware (ErrorHandlerMiddleware)

## 8. Logging
- Used Microsoft extension that could be hooked to any logging service like AppInsights or Serilog (Not configured).

## 9. Validations
- Used manual data, imputs and responses validations.

## 10 Furure work
- Using validation services
- Using mappers
- Applying External APIs queuing logic
- Write integration tests
- Finish unit tests

## 11. Run / Test
- Open the solution using VS.NET 2022
- Base URL should be: https://localhost:7151
- Review Swagger docos
- Import Postman collection (CurrencyExchanger.postman_collection.json) included in solution.
- Run endpoints using Postman
- Dev Rate limit is 3 requests per 10 seconds.
