# CostsSettler API

This is the CostsSettler application API.

### How to build server Docker images:

1. Clone the repository.
2. Make sure you have Docker engine installed.
3. Run command `docker compose -f docker-compose.build.yaml build` in `./src` directory.

### How to build the client application Docker images:

1. Clone the repository: https://github.com/Tomasz-Terlecki/CostsSettler-SPA.
2. Make sure you have Docker engine installed.
3. Build docker image using command `docker build -t costssettler.spa:latest .` in main folder.

### How to run the system

1. Add `127.0.0.1 costssettler.com` to your `hosts` file. It is needed to make "Login with Google" work properly. Google auth services require a [top-level domain](https://en.wikipedia.org/wiki/List_of_Internet_top-level_domains) like `.com`, `.org` etc.
2. Build server images (instructions above).
3. Build client app image (instructions above).
4. Set some environment variables:
   * `SA_PASSWORD` in `./src/costs-settler-api.env` and `./src/mssql.env` -- password to MS SQL Server.
   * `KeycloakClientConfig__Secret` in `./src/costs-settler-api.env` -- CostsSettler-API client secret.
   * `COSTSSETTLER_API_SECRET` in `./src/keycloak.env` -- same value as in `KeycloakClientConfig__Secret`.
   * `KEYCLOAK_ADMIN_PASSWORD` in `./src/keycloak.env` -- password to keycloak admin panel.
5. Run command `docker compose up -d` in `./src` folder of server application repository.
6. All containers should be started automatically.
   * Keycloak configuration should import automatically.
   * `CostsSettler-API` database should migrate automatically on `costs-settler-api` container startup.
   * **Your CostsSettler system instance should be working properly.**