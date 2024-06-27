# ArabTube: A Safe Video Streaming Platform

Welcome to **ArabTube**, a unique video streaming platform designed to filter out sexual content in videos and photos and eliminate hate speech in comments and video text. This project is a collaborative effort by [Your Name] and Ahmed Awad. We have focused on building a secure, user-friendly backend using ASP.NET and various modern tools and principles.

## Table of Contents

- [Features](#features)
- [Technologies Used](#technologies-used)
- [Architecture](#architecture)
- [Setup and Installation](#setup-and-installation)

## Features

ArabTube provides almost all the functionalities of a typical video streaming platform, with additional features aimed at enhancing security and content filtering:

1. **User Authentication and Account Management**
   - Register / Login / Confirm Email / Forget Password / Reset Password

2. **Video Management**
   - Upload Video / Like Video / Dislike Video / Flag Video / Add Video to Playlist / Update Video / Delete Video

3. **Comment Management**
   - Write Comment / Like Comment / Dislike Comment / Flag Comment / Update Comment / Delete Comment

4. **Playlist Management**
   - Create Playlist / Add Video to Playlist / Save Another Playlist / Default Playlists (Liked Videos, Watch Later) / Public and Private Playlists / Update Playlist / Delete Playlist

5. **User History**
   - Track and display the history of watched videos sorted by the last watch time

6. **Notifications**
   - Notify users when tagged in a comment, when a subscribed user uploads a video, and when someone likes or dislikes their comment

7. **Subscriptions**
   - Subscribe to other users

8. **User Banning**
   - Users flagged 50 times are banned for 30 days

9. **Admin Controls**
   - Review and delete flagged videos and comments, with the ability to increase the flag count for users

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

ArabTube is built using the N-Tier Architecture, which includes 4 layers:

1. **Presentation Layer**
2. **Logic Layer**
3. **Data Access Layer**
4. **Data Layer**

We also employ several design patterns and principles:

- **Design Patterns**: Repository Pattern, Unit of Work Pattern, Dependency Injection Pattern
- **SOLID Principles**: Single Responsibility Principle, Interface Segregation Principle, Dependency Inversion Principle

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
