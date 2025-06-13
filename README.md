# PicAFlick

## 🔑 TMDb API Token Setup

This application requires a **v4 API access token** from [The Movie Database (TMDb)](https://www.themoviedb.org/).

### Steps to Get an TMDb API Token:
1. Go to [https://www.themoviedb.org/signup](https://www.themoviedb.org/signup) and create a free TMDb account.
2. Visit your [API settings page](https://www.themoviedb.org/settings/api).
3. Scroll down to the section labeled **API Read Access Token (v4 auth)**.
4. Copy the **entire token** 

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
2. Open `.env` and replace the placeholder with your API token:
```cmd
TMDB_API_Token=your_api_read_access_token_here
```
### Database Setup
This app uses SQL Server for data storage. You must set the `DB_CONNECTION_STRING` in your `.env` file to connect to your SQL Server instance. Look at the .env.template for an example.