# Sudoku.Services.Web

This is built upon the packages from the *Sudoku.Resolver* project.

This is largely a work in progress and only a template has been constructed for one project.

The goal is to create an ecosystem of services using RabbitMQ.

This is somewhat inspired by the book **Pro microservices in .NET 6**

## Sudoku.Scraper.API

This service reads and publishes Sudoku puzzles over RabbitMQ from various sources.
These sources could be from various websites, files or other sources.

Instead of letting each instance handle multiple sources a second instance of the Sudoku.Scraper.API can be spun up with different configuration. For example, pointing to a different source.

This way we can scale throughput for a specific source or read from multiple different sources.

Each instance can also share a database. Where they keep track of already discovered/published puzzles.
For example, when an instance restarts it can pick of where it left of. It also prevents from it from publishing duplicate puzzles.

This project will be using the *Sudoku.Parser.** namespaces.

## Other

### Solving puzzles

The following extra services can be added as an extension.

Published messages from *Sudoku.Scraper.API* can be received by a service that solves puzzles.

The service can be spun up in different configurations. 
One instance solves puzzles only using Strategy A while a second instance solves the puzzles using strategy B. Each can be scaled up when needed.

This service will again have its own database. Containing a complete history of each received puzzle, what algorithm was used to solve the puzzle and other performance metrics to be able to compare algorithms performances.

Maybe certain algorithms are faster for certain puzzles then others? By collecting this data, later analyses can be done to optimize the process to for example predetermine an optimal strategy to solve a new puzzle. By looking for certain characteristics.
The analyses can even be done in a different service. The metrics data can be published over RabbitMQ and stored in an analyses service to create reports and run analyses on the collected data.

A unique identifier is generated for each puzzle. (For example, a hash) This way we can keep track of each puzzle we ever solved and if it was already solved for this algorithm. Including some metrics.

### Displaying puzzles.

A website can be built to listen to incoming messages from RabbitMQ.

A new puzzle is discovered by *Sudoku.Scraper.API*. The puzzle can be stored in this database and viewed by the web application build on top of it.

Another incoming message when a puzzle is solved. The solution can then be included on the website.

Now you can search for puzzles and view their solutions.