
# Job Application Tracker

## Project Overview

Job Application Tracker is an ASP.NET Core MVC application designed to help users organize and track job applications throughout the hiring process.

The application allows users to create, view, update, and delete job applications while tracking their current status.

## Features

- Create new job applications
- View saved applications
- Edit application information
- Delete applications
- Track application status
- Dashboard displaying application statistics

## Technologies Used

- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQLite Database
- Razor Views
- Chart.js

## Database

This project uses Entity Framework Core with SQLite to store and manage application data.

## CRUD Functionality

### Create
Users can add new job applications to the database.

### Read
Users can view saved job applications.

### Update
Users can modify application details, including status changes.

### Delete
Users can remove applications they no longer need.

## Challenges

One challenge I faced was configuring Entity Framework Core and connecting the application to the database. I worked through database migrations, DbContext configuration, and debugging data issues to successfully implement CRUD functionality.

## Future Improvements

Possible future improvements include:
- Email integration for automatically importing job applications
- User authentication
- More advanced dashboard analytics
- Additional application tracking features
