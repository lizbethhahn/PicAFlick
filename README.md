# PicAFlick

PicAFlick is a full-stack movie and TV watchlist application built to explore real-world software architecture using modern .NET technologies.

Experience PicAFlick in action — from natural language chat to AI-driven recommendations powered by Semantic Kernel and TMDb.<br>
[**Watch the demo**](https://drive.google.com/file/d/1IBB3vIgP2ciia-I4rLeiGiPy4sFgJqvQ/view?usp=drive_link)

It serves as both a personal tool for managing what to watch next and a hands-on implementation of clean architecture principles, API design, and client-server interaction.

## Key Features

- Search and add movies and TV shows via the TMDb API
- Manage a personalized watchlist
- Mark items as watched and track viewing status
- Store and retrieve data using SQL Server
- Console-based AI assistant (CapstoneChatbot) that interacts with the API

## Architecture Overview

The system is structured with clear separation of concerns:

- ASP.NET Core Web API → Handles business logic and data access  
- Angular SPA → Frontend client for user interaction  
- SQL Server → Persistent data storage  
- CapstoneChatbot (Console App) → AI-driven interface using Semantic Kernel  

Data flow:

Client (Angular / Console)  
↓ HTTP  
Web API  
↓  
Domain Services  
↓  
Database  

## Tech Stack

- C# / .NET 8
- ASP.NET Core Web API
- Angular
- Entity Framework Core
- SQL Server
- TMDb API
- Semantic Kernel (AI integration)

---

## Local Development Setup

PicAFlick consists of an Angular frontend, an ASP.NET Core Web API, a SQL Server database, and an optional AI console application built with Semantic Kernel.

The following steps configure the services and credentials required to run the application locally.

Unless otherwise noted, all paths and `cd` commands below are relative to the PicAFlick repository root.

### 1. TMDb API Setup

PicAFlick uses [The Movie Database (TMDb)](https://www.themoviedb.org/) for movie and TV data.

1. Create a TMDb account at https://www.themoviedb.org/signup
2. Go to https://www.themoviedb.org/settings/api
3. Copy your API Read Access Token (v4 auth) and API Key.

These credentials will be added to .NET User Secrets in the steps below.

### 2. GitHub Models Setup

The AI functionality uses Semantic Kernel with GitHub Models.

1. Go to https://github.com/marketplace/models
2. Sign in to your GitHub account.
3. Create a GitHub Models API token with access to the OpenAI models, specifically `openai/gpt-4o`.
4. Copy and store the token securely. It will be added to .NET User Secrets below.

> **Note:** The AI application is currently configured to use `openai/gpt-4o`. If the model is changed in code, the GitHub Models token must have access to the selected model.

### 3. Configure the PicAFlick Web API

The Web API uses .NET User Secrets to keep the TMDb API token out of source control.

Navigate to the Web API project:

```bash
cd PicAFlick.WebApi
```

Initialize User Secrets:

```bash
dotnet user-secrets init
```

Add your TMDb API Read Access Token:

```bash
dotnet user-secrets set "Tmdb:ApiToken" "your_api_read_access_token_here"
```

Verify the configuration:

```bash
dotnet user-secrets list
```

You should see:

```text
Tmdb:ApiToken = ...
```

### 4. Configure the Local Database

The Web API uses SQL Server for local data storage.

Configure the `Default` connection string in:

```text
PicAFlick.WebApi/appsettings.Development.json
```

Example using Windows authentication:

```json
{
  "ConnectionStrings": {
    "Default": "Server=localhost;Database=PicAFlick;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Update the server and database values as needed for your local SQL Server configuration.

### 5. Configure CapstoneChatbot

The AI console application is a separate .NET project and has its own User Secrets configuration.

Navigate to:

```bash
cd CapstoneChatbot/CapstoneChatbot.App
```

Initialize User Secrets:

```bash
dotnet user-secrets init
```

Add the required credentials:

```bash
dotnet user-secrets set "Tmdb:ApiToken" "your_api_read_access_token_here"
dotnet user-secrets set "Tmdb:ApiKey" "your_tmdb_api_key_here"
dotnet user-secrets set "GithubModels:ApiKey" "your_github_models_api_key_here"
```

Verify the configuration:

```bash
dotnet user-secrets list
```

You should see:

```text
Tmdb:ApiToken = ...
Tmdb:ApiKey = ...
GithubModels:ApiKey = ...
```

Never commit API tokens, API keys, or other secrets to source control.

### 6. Run PicAFlick

A complete local development session uses Visual Studio and two terminals.

#### Start the Web API

Open `PicAFlick.sln` in Visual Studio.

Set `PicAFlick.WebApi` as the startup project and click the green **Run/Play** button.

The API will start using the development settings in `launchSettings.json`, and Swagger should open in your browser.

Keep Visual Studio running while using PicAFlick. Running the API through Visual Studio enables debugging and breakpoints.

#### Start the Angular SPA

Open a terminal at the PicAFlick repository root and navigate to:

```bash
cd PicAFlick.SPA
```

Start the Angular development server:

```bash
npm start
```

The application should be available at:

```text
https://localhost:4200
```

Keep this terminal running while using PicAFlick.

#### Start the AI Assistant

With the PicAFlick Web API already running, open another terminal and navigate to:

```bash
cd CapstoneChatbot/CapstoneChatbot.App
```

Start the AI console application:

```bash
dotnet run
```

The AI assistant uses Semantic Kernel and GitHub Models to interpret natural-language requests and communicates with the PicAFlick Web API.

### Development Setup at a Glance

When running the complete application locally, you will typically have:

- **Visual Studio:** `PicAFlick.WebApi` running with the debugger
- **Terminal 1:** `PicAFlick.SPA` running with `npm start`
- **Terminal 2:** `CapstoneChatbot.App` running with `dotnet run`
- **Browser:** `https://localhost:4200`

## Attribution

This product uses the TMDb API but is not endorsed or certified by TMDb.
All movie and TV data is provided by [The Movie Database (TMDb)](https://www.themoviedb.org/).
