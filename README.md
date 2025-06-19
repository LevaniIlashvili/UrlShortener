# 🔗 URL Shortener

containerized URL shortening service built with **.NET 9** and **Apache Cassandra**, featuring analytics tracking, expiration handling, and custom aliases. Shorten long URLs and track usage with a scalable backend.

---

## 🚀 Features

- 🔄 Shortens long URLs into Base62 short codes (6–8 characters)
- ⏳ Supports **temporary** and **permanent** URLs
- 📈 Tracks click analytics (IP + User Agent + timestamp)
- 🗓️ Background job to auto-expire and clean up expired URLs
- 🔧 RESTful API with Swagger UI documentation
- 🐳 Fully containerized via Docker Compose

---

## 🛠️ Tech Stack

| Component        | Technology         |
|------------------|--------------------|
| Backend          | ASP.NET Core (.NET 9 Web API) |
| Database         | Apache Cassandra 4.1 |
| Containerization | Docker & Docker Compose |
| Background Tasks | Hosted Services (IHostedService) |
| API Docs         | Swagger (Swashbuckle) |

---

## 📦 Cassandra Schema

### `urls` (primary URL table)

| Column           | Type       | Description                  |
|------------------|------------|------------------------------|
| short_code       | text       | Primary key (Base62 code)    |
| original_url     | text       | Long URL                     |
| created_at       | timestamp  | Creation date                |
| expiration_date  | timestamp  | Optional expiration          |
| click_count      | bigint     | Number of redirects          |
| is_active        | boolean    | Flag for active/inactive     |

### `click_analytics` (per-click data)

| Column       | Type      | Description                    |
|--------------|-----------|--------------------------------|
| short_code   | text      | Partition key                  |
| click_date   | timestamp | Clustering key (DESC)          |
| user_agent   | text      | User agent string              |
| ip_address   | text      | IP address of the user         |

---

## 📌 API Endpoints

| Endpoint                         | Method | Description                           |
|----------------------------------|--------|---------------------------------------|
| `/api/urls`                      | POST   | Create a short URL                    |
| `/api/urls/{shortCode}`          | GET    | Get details for a specific short URL  |
| `/api/urls/{shortCode}`          | PUT    | Update original URL or expiration     |
| `/api/urls/{shortCode}`          | DELETE | Delete a short URL                    |
| `/{shortCode}`                   | GET    | Redirect to the original URL          |

---

## 📋 Input Validation

- ✅ Valid URL format (`https://example.com`)
- ✅ Expiration date cannot be in the past
- ✅ Custom alias must be alphanumeric (`a-zA-Z0-9_-`)
- ✅ Custom alias must be unique (no collisions)

---

## ⏱ Expiration Handling

- A **background service** runs periodically
- Deactivates or deletes URLs past their expiration
- Accessing an expired URL returns HTTP **404**

---

## 📊 Click Analytics

Each redirect:

- Increments `click_count`
- Stores:
  - Timestamp
  - User Agent
  - IP Address

---

### 🐳 Running with Docker Compose

To run the application with Cassandra and the API together:

git clone https://github.com/your-username/url-shortener.git
cd url-shortener
cd docker
docker-compose up --build
