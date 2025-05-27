# PicAFlick

## 🔑 TMDb API Key Setup

This application requires an API key from [The Movie Database (TMDb)](https://www.themoviedb.org/).

### Steps to Get an TMDb API Key:
1. Go to [https://www.themoviedb.org/signup](https://www.themoviedb.org/signup) and create a free account.
2. Visit your [API settings page](https://www.themoviedb.org/settings/api) and apply for a free developer key.
3. Copy your API key.

### Clone the repo
```bash
git clone https://github.com/lizbethhahn/PicAFlick.git
```

### Configure the App
1. In the root folder of the project, copy `.env.template` to a new file named `.env`.

 **Windows (CMD):**
```cmd
copy .env.template .env
```  
2. Open `.env` and replace the placeholder with your API key:
```cmd
TMDB_API_KEY=your_api_key_here
```
### Database Setup
This app uses SQL Server for data storage. You must set the `DB_CONNECTION_STRING` in your `.env` file to connect to your SQL Server instance. Look at the .env.template for an example.