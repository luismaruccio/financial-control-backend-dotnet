# Financial Control API (.NET)
This project is part of a portfolio showcasing the development of a financial control system using various technologies. The Financial Control API built with .NET serves as the backend for managing user authentication, category registration, and transaction tracking, including single, recurring, and installment transactions. The API is designed with a clean architecture, following a layered structure to ensure scalability, maintainability, and ease of testing.

### Key Features:
- **User Authentication:** Secure login and registration system.
- **Category Management:** Create, read, update, and delete financial categories.
- **Transaction Management:** Record and manage financial transactions, supporting single, recurring, and installment types.
- **Financial Health Insights:** Provides data endpoints for generating financial health reports and visualizations.
  
### Project Structure:
- **API Layer:** Exposes endpoints for client interactions.
- **Application Layer:** Contains business logic and application services.
- **Domain Layer:** Defines core entities, aggregates, and domain services.
- **Infrastructure Layer:** Implements data access and external service integrations.
- **IoC Layer:** Manages dependency injection and service registration.
  
### Getting Started:
To run this project locally, follow these steps:

1. **Clone the Repository:**
```bash
git clone https://github.com/your-username/financial-control-api-dotnet.git
cd financial-control-api-dotnet
```
2. **Configure the Database:**
- Update the connection string in appsettings.json.
  
3. **Run Migrations:**
```bash
dotnet ef database update
```

4. **Build and Run the Project:**
```bash
dotnet build
dotnet run
```

5. **Access the API:**
- The API will be available at https://localhost:5001 by default.

Feel free to explore the endpoints and functionalities provided by this API. Contributions and feedback are welcome!
