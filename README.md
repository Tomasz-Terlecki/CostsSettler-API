# CostsSettler API

This is the CostsSettler application API.

### How to run the application

1. Add `127.0.0.1 costssettler.com` to your `hosts` file. It is needed to make "Login with Google" work properly. Google auth services require a [top-level domain](https://en.wikipedia.org/wiki/List_of_Internet_top-level_domains) like `.com`, `.org` etc.
2. Clone the repository.
3. Make sure you have Docker engine installed.
4. Run command `docker-compose up -d --build` in `src` directory.
   * All containers should be started automatically.
   * Keycloak configuration should import automatically.
   * `CostsSettler-API` database should migrate automatically on `costs-settler-api` container startup.
   * **Your `CostsSettler-API` instance should be working properly.**
