# PicAFlickSPA
This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 20.0.2.

## Running the Angular App Locally
To start a local development server, run:
```bash
ng serve
```
By default, this runs over HTTP at http://localhost:4200/.

If your backend API uses HTTPS (like this project does), you should run the frontend with HTTPS to avoid mixed content errors:
```ng serve --ssl \
  --ssl-cert "src/assets/ssl/localhost.crt" \
  --ssl-key "src/assets/ssl/localhost.key"
```
Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

📌 Note: The certificate files are not included in the repo. See [Local Development with HTTPS](#local-development-with-https) below for instructions on generating and trusting your own.
Once the server is running, open your browser and navigate to the appropriate URL.
The application will automatically reload whenever you modify any of the source files.

## Local Development with HTTPS
This project uses HTTPS for local development to match production environments and avoid browser issues with mixed content.

### Generating a Self-Signed Certificate
The certificate and key files used by Angular are excluded from version control:
```
src/assets/ssl/localhost.key  
src/assets/ssl/localhost.crt  
src/assets/ssl/localhost-openssl.cnf  
```
To generate them, run the following:
```
openssl req -x509 -newkey rsa:2048 -nodes \
  -keyout src/assets/ssl/localhost.key \
  -out src/assets/ssl/localhost.crt \
  -days 365 \
  -subj "/CN=localhost" \
  -config src/assets/ssl/localhost-openssl.cnf
```
### Trust the Certificate (optional but recommended)
To avoid browser warnings, trust the certificate:

**On Windows:**
```
certutil -addstore root src/assets/ssl/localhost.crt
```
**On macOS:**
```
sudo security add-trusted-cert -d -r trustRoot -k /Library/Keychains/System.keychain src/assets/ssl/localhost.crt
```

## Code scaffolding
Angular CLI includes powerful code scaffolding tools. To generate a new component, run:
```bash
ng generate component component-name
```
For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:
```bash
ng generate --help
```
## Building
To build the project run:
```bash
ng build
```
This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Additional Resources
For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
