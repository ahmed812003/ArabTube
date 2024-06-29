# ArabTube: A Safe Video Streaming Platform

Welcome to **ArabTube**, a unique video streaming platform designed to filter out sexual content in videos and photos and eliminate hate speech in comments and video text. This project is a collaborative effort by Me and [Ahmed Awad](https://github.com/ahmedawad72). We have focused on building a secure, user-friendly backend using ASP.NET and various modern tools and principles.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Architecture](#architecture)
- [Documentation](#documentation)
- [Setup and Installation](#setup-and-installation)

## Features

ArabTube provides almost all the functionalities of a typical video streaming platform, with additional features aimed at enhancing security and content filtering:

1. User Authentication
   - Register / Login
   - Email confirmation
   - Password recovery (Forget/Reset)

2. Video Management
   - Upload, update, and delete videos
   - Like/Dislike videos
   - Flag inappropriate content
   - Add videos to playlists

3. Comment System
   - Write, update, and delete comments
   - Like/Dislike comments
   - Flag inappropriate comments

4. Playlist Functionality
   - Create, update, and delete playlists
   - Public and private playlist options
   - Default playlists: Liked Videos and Watch Later
   - Save playlists from other users

5. User History
   - Track and sort watched videos

6. Notifications
   - User mentions in comments
   - New video uploads from subscribed channels
   - Likes/Dislikes on user's comments

7. Subscriptions
   - Subscribe to other users' channels

8. Content Moderation
   - User flagging system (50 flags result in a 30-day ban)
   - Admin review system for flagged content

## Technologies Used

- **Backend Framework**: ASP.NET
- **RESTful API**: For front-end integration
- **Database Mapping**: Entity Framework
- **Data Querying**: LINQ
- **API Security**: Microsoft Identity, JWT
- **Video Storage**: Azure Blob Storage
- **Database**: SQL Server
- **Video Processing**: FFMPEG, XFFmpeg.NET
- **Logging**: Serilog
- **Object Mapping**: AutoMapper
- **Caching**: In-Memory Caching

## Architecture

- N-tier Architecture (4 layers: Presentation, Logic, Data Access, Data)
- Repository Pattern
- Unit of Work Pattern
- Dependency Injection
- SOLID Principles applied:
  - Single Responsibility Principle
  - Interface Segregation Principle
  - Dependency Inversion Principle

## API Documentation

For detailed API documentation and testing, please visit our Swagger UI:
[ArabTube API Documentation](https://arabtubedemo1.runasp.net/swagger/index.html)


## Setup and Installation

To set up the project locally, follow these steps:

1. Clone the repository:
    ```bash
    git clone https://github.com/your-repo/ArabTube.git
    ```

2. Navigate to the project directory and restore the dependencies:
    ```bash
    cd ArabTube
    dotnet restore
    ```

3. Update the configuration settings in `appsettings.json` with your Azure Blob Storage, SQL Server, and other necessary credentials.

4. Run the database migrations:
    ```bash
    dotnet ef database update
    ```

5. Start the application:
    ```bash
    dotnet run
    ```

---

We hope you find **ArabTube** to be a valuable tool for safe and secure video streaming. For any queries or support, feel free to contact us.
