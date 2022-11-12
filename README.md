# To Do - Fullstack To-Do Application
## Overview

This application can be completely separeted in two independent projects: the backend application, an API developed on .NET
and a frontend application developed in React.

For the backend application, I tried to implement some concepts of Domain Driven Design as an exercise of what I was studying at the time. 
This made the code a bit more complex, but the intention of this was to start applying concepts to write better code and check on this project whenever I
need to remember basic concepts of DDD.

The main model (ToDo) contains methods for each action that should be notified to the user, create, update, delete and mark task as completed. This was a 
good guide for the methods that should be implemented in the service class (not a layers since I didn't divide the whole project for simplicity's sake).
The methods on this service class notify all users connected to this API through a SignalR hub. Also, worth mentioning that the application uses a sqlite database
that gets checked everytime a connection to the database is requested.

For the frontend application, a simple SPA using React, which has two sections: A completed tasks section and a to be finised task section. The application 
fetches data from the API and also listen to events in the SignalR hub. The hub notifies whenever a ToDo is created, updated, deleted or completed, and the application fetches data and rerender the page everytime this happens.
