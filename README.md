# EVS - Egyptian Virtual School

A web application built with ASP.NET Core 8 and Razor Pages, developed using Visual Studio 2022.

## 📋 Project Overview

EVS (Egyptian Virtual School) is a modern web application leveraging the power of ASP.NET Core 8 framework with Razor Pages for server-side rendering and dynamic web content delivery.

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8
- **Architecture**: Razor Pages
- **IDE**: Visual Studio 2022
- **Database**: Entity Framework Core
- **Runtime**: .NET 8.0
- **Language**: HTML/C#

## 👥 Collaborators

| Name | Student ID |
|------|------------|
| Yusuf Alsaied | 202402431 |
| Rawan Ibrahim | 202401352 |
| Ali Ahmed | 202401187 |

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (or VS Code)
- SQL Server (or SQL Server Express)

### Installation

1. Clone the repository: 
   ```bash
   git clone https://github.com/imunderthetree/EVS.git
   cd EVS
   ```

2. Navigate to the Project directory:
   ```bash
   cd Project
   ```

3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Update the database connection string in `appsettings.json`

5. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

6. Run the application:
   ```bash
   dotnet run
   ```

7. Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

## 📁 Project Structure

```
EVS/
├── Project/           # Main application folder
│   ├── EVS.sln       # Solution file
│   └── EVS/          # Application project
├── .gitignore        # Git ignore configuration
└── README.md         # Project documentation
```

## 🔧 Configuration

The project uses standard ASP.NET Core configuration files:

- `appsettings.json` - Main configuration
- `appsettings.Development.json` - Development environment settings

## 📦 Build and Publish

### Development Build
```bash
cd Project
dotnet build
```

### Release Build
```bash
cd Project
dotnet build --configuration Release
```

### Publish
```bash
cd Project
dotnet publish --configuration Release --output ./publish
```

## 🧪 Testing

Run tests using:
```bash
cd Project
dotnet test
```

## ✨ Features

- Electronic voting system
- ASP.NET Core Razor Pages architecture
- Entity Framework Core for data management
- Responsive web interface
- Secure authentication and authorization

## 📝 License

This project is part of an academic assignment.

## 🤝 Contributing

This is an academic project. For major changes, please coordinate with the project team members.

## 📧 Contact

For questions or support, please contact the project collaborators listed above.

---

**Project Repository**: [https://github.com/imunderthetree/EVS](https://github.com/imunderthetree/EVS)

**Created**: December 2025
