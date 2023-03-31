1. Make sure you have docker installed.
2. Run `docker-compose up -d` this will spin up the rabbitmq and mssql for you.
Note: I used arm image for mssql, you may have to update it in the docker-compose.yml
3. Run console app (TaskManagementSystem) and (TaskManagementSystem.Api.Consumer). 
4. Try create task, then update it. Update will send async command to message broker, and consumer will handle the command.
