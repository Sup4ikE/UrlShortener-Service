# 🔗 URL Shortener Service

![CI](https://github.com/Sup4ikE/UrlShortener-Service/actions/workflows/ci.yml/badge.svg)
![CD](https://github.com/Sup4ikE/UrlShortener-Service/actions/workflows/cd.yml/badge.svg)

A simple and clean **URL Shortener API** built with **ASP.NET Core**.  
The service allows creating short URLs, redirecting to original links, tracking usage, and managing stored URLs.

This project was created as a **pet-project** to practice backend architecture, testing, and CI/CD.

---

## ✨ Features

- Create short URLs
- Redirect to original URLs
- Store URLs in a database
- Track redirect clicks
- Pagination for stored URLs
- Delete short URLs
- Unit & integration tests
- CI/CD with GitHub Actions
- Dockerized application

---

## 🧱 Tech Stack

- **ASP.NET Core** (.NET 9)
- **Entity Framework Core**
- **SQLite** (for simplicity & testing)
- **xUnit** (unit & integration tests)
- **Docker**
- **GitHub Actions** (CI/CD)
- **GitHub Container Registry (GHCR)**

---

## 📦 Project structure:

- **Api/** — ASP.NET Core Web API
- **Core/** — Domain logic & abstractions
- **Infrastructure/** — Database & repositories
- **Tests/** — Unit & integration tests
- **UrlShortener.sln** — Solution file
- **Dockerfile** — Docker build configuration

---

## 🚀 API Endpoints (example)

| Method | Endpoint                               | Description               |
|--------|----------------------------------------|---------------------------|
| POST   | `/api/shortener/shorten`               | ➕ Create short URL        |
| GET    | `/api/shortener/{code}`                | 🔁 Redirect to original URL|
| GET    | `/api/shortener/stats/{code}`          | 📄 Get stats from URL     |
| GET    | `/api/shortener/all?page=1&size=10`    | 📄 Get paginated URLs     |
| DELETE | `/api/shortener/{code}`                | ❌ Delete short URL       |

---

🐳 Run with Docker

Pull image from GHCR
docker pull ghcr.io/<owner>/urlshortener-service:latest

Run container
docker run -p 8080:8080 ghcr.io/<owner>/urlshortener-service:latest

API will be available at:

http://localhost:8080

---

🔄 CI / CD

CI
Restore dependencies
Build in Release mode
Run tests with code coverage

CD
Build Docker image
Push image to GitHub Container Registry
Runs only after successful CI

---

📄 License

This project is licensed under the MIT License.

👨‍💻 Author

Created by Oleg Pona
Junior Backend Developer (.NET)
