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

## TMDb API Token Setup

This application requires a v4 API access token from The Movie Database (TMDb).

### Steps to Get a TMDb API Token

1. Create an account at https://www.themoviedb.org/signup  
2. Go to https://www.themoviedb.org/settings/api  
3. Copy your API Read Access Token (v4 auth)

---

## AI Setup (GitHub Models + Semantic Kernel)

This project includes AI functionality using Semantic Kernel and GitHub Models.

### Steps to Get a GitHub Models API Key

1. Go to https://github.com/marketplace/models  
2. Sign in to your GitHub account  
3. Create a GitHub Models API token with access to the OpenAI models (specifically gpt-4o)

   - Go to https://github.com/settings/tokens
   - Click "Generate new token"
   - Select access to GitHub Models / AI inference
   - Ensure the token has permission to use the model: openai/gpt-4o
   - Copy and store the token securely (it will not be shown again)

   Note: This project is currently configured to use the model "openai/gpt-4o". If you change the model in code, ensure your token has access to that model.

---

## Configure the App (User Secrets)

This project uses .NET User Secrets for local configuration.

From the project directory (where the `.csproj` file is located), run the following commands:

```bash
dotnet user-secrets init
dotnet user-secrets set "Tmdb:ApiToken" "your_api_read_access_token_here"
dotnet user-secrets set "GitHubModels:ApiKey" "your_github_models_api_key_here"
```
### Verify Configuration

To confirm your secrets are set correctly, run:

```bash
dotnet user-secrets list
```
You should see both:

- Tmdb:ApiToken  
- GitHubModels:ApiKey

## Attribution

This product uses the TMDb API but is not endorsed or certified by TMDb.
All movie and TV data is provided by [The Movie Database (TMDb)](https://www.themoviedb.org/).
