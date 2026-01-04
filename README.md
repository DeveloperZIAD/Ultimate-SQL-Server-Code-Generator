# Ultimate SQL Server Code Generator

A powerful, open-source Windows desktop tool designed to generate clean, production-ready C# code from SQL Server databases.

This tool automates the creation of **Entity classes**, **Data Logic**, and **Business Logic** layers with **DTOs** and **full automatic mapping** – supporting both **tables** and **stored procedures**.

Perfect for .NET developers who want to accelerate backend development while following best practices (clean architecture, safe resource management, exception handling).

## Features

- **Entity Generation** from any table (POCO classes)
- **Full CRUD Data Logic** for tables (GetAll, GetById, Insert, Update, Delete) with:
  - Proper `using` statements
  - `try-catch` blocks
  - Parameterized queries
  - Safe connection handling
- **Business Logic with Custom DTOs**:
  - Auto-generated DTOs based on result sets
  - Full automatic mapping from `SqlDataReader`
  - Clean service classes
- **Stored Procedure Support**:
  - Generate Data Logic (`Execute` + `GetReader`)
  - Generate Business Logic with auto-mapped DTOs
- **User-Friendly Interface**:
  - Connect to any SQL Server instance (Windows or SQL Authentication)
  - Browse databases, tables, and stored procedures
  - Tabbed view for different code outputs
  - Real-time status feedback
  - One-click code saving

## Requirements

- Windows OS
- .NET Framework 4.8
- Access to a SQL Server instance

## How to Use

1. Download the latest release (`Code Generator.exe`)
2. Run the application
3. Enter your server details and connect
4. Select a database
5. Choose a **table** or **stored procedure**
6. Switch between tabs to view generated code:
   - **Entity Class** – POCO model
   - **Data & Business Logic** – For stored procedures
   - **Data Logic (Table)** – CRUD operations using direct SQL
   - **Business Logic (Table)** – Service with DTO and mapping
7. Click **Save Code to File** to export

## Generated Code Highlights

- Fully safe resource management (`using` statements)
- Exception handling with meaningful messages
- Proper use of `CommandType.StoredProcedure`
- Automatic DTO generation with correct C# types
- Complete reader-to-object mapping (no manual TODOs)

## License

This project is licensed under the **MIT License** – free to use, modify, and distribute.

## Contributing

Contributions are welcome! Feel free to:
- Open issues for bugs or feature requests
- Submit pull requests
- Share the tool with your team

## Author

Developed with passion by ZEYAD .NET enthusiast to save developers time and effort.

⭐ Star this repo if you found it useful!  
Your support means the world.

---
Made with ❤️ for the .NET community
