# About

Web API for measuring distance between two airports using CTeleport API.
API has the only endpoint where IATA codes should be passed.

# Launch

You can launch it by docker:
- go to command prompt;
- `cd` to root directory of the solution (where is `docker-compose` file located);
- write `docker compose up`;
- go to `localhost:5051/swagger`.

# Internal design

Solution represents clean architecture.

Solution contains 4 projects:
- API - has queries/commands implementation, controllers, DI container;
- Application - business-logic. Contains query-handlers, validators, pipelines. Also contains contracts for anti-corruption layer;
- CTeleportClient - encapsulates interaction with external system. Implements contracts for anti-corruption layer. As we use only `GET`-request - it's enough to retry requests. We don't have need to handle idempotency. So i implemented retry policy by `Polly`.
- CacheClient - encapsulates interaction with cache. Use distributed redis-caching. 

# Request-flow

- request arrives in controller and it sends it to mediator where the request is validated by validator;
    - if request has some errors - validator throws exception which is intercepted in middleware and converts it to specific HTTP status-code (this logic is implemented in `AirportControl.API.ServiceCollectionExtensions`);
    - if everything is ok we get into `GetDistanceQueryHandler`;
- then we call CTeleportProvider which take data from cache or goes to third-party API:
  - if cache contains such key we read data from cache and immediately give it to Application layer;
  - if cache doesn't contain such key we go further and call third-party API.
- then we calculate distance between airports by there location. We do it in use-case. As an improvement we can allocate calculating code to another service. It will fit if we will have multiple use-cases that need to calculate distance between two points;
- than we save calculated value to cache and return to consumer.
