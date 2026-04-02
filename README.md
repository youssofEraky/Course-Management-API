# Course Management System API

ASP.NET Core Web API for managing university courses, students, and instructors.

---

## Technologies Used

| Technology | Description |
|---|---|
| **ASP.NET Core 8** | Web framework for building the REST API |
| **Entity Framework Core 8** | ORM for database access and migrations |
| **SQL Server / LocalDB** | Relational database |
| **JWT (JSON Web Tokens)** | Stateless authentication mechanism |
| **BCrypt.Net-Next** | Password hashing library |
| **Swagger / Swashbuckle** | Auto-generated API documentation and testing UI |

---

## How to Run

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server or SQL Server LocalDB (included with Visual Studio)

### Steps

**1. Clone / place the project**
```bash
cd CourseManagementAPI
```

**2. Restore NuGet packages**
```bash
dotnet restore
```

**3. Update the connection string**

Open `appsettings.json` and update `ConnectionStrings:DefaultConnection` to point to your SQL Server instance. The default uses LocalDB:
```
Server=(localdb)\mssqllocaldb;Database=CourseManagementDB;Trusted_Connection=True;
```

**4. Apply EF Core migrations**
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**5. Run the API**
```bash
dotnet run
```

**6. Open Swagger UI**

Navigate to: `https://localhost:5001/swagger`

---

## Authentication

1. Register an instructor via `POST /api/instructors` or a student via `POST /api/students`
2. Login via `POST /api/auth/login` — you will receive a JWT token
3. Click **Authorize** in Swagger UI and enter: `Bearer <your_token>`
4. All protected endpoints will now work

### Roles
| Role | Access |
|---|---|
| `Admin` | Full access to all endpoints |
| `Instructor` | Manage courses, view students |
| `Student` | View courses, manage own enrollments |

---

## API Endpoints Summary

### Auth
| Method | Route | Description |
|---|---|---|
| POST | `/api/auth/login` | Login and receive JWT |

### Instructors
| Method | Route | Auth |
|---|---|---|
| GET | `/api/instructors` | Admin |
| GET | `/api/instructors/{id}` | Admin, Instructor |
| POST | `/api/instructors` | Public |
| PUT | `/api/instructors/{id}` | Admin, Instructor |
| PUT | `/api/instructors/{id}/profile` | Admin, Instructor |
| DELETE | `/api/instructors/{id}` | Admin |

### Students
| Method | Route | Auth |
|---|---|---|
| GET | `/api/students` | Admin, Instructor |
| GET | `/api/students/{id}` | Admin, Instructor, Student |
| POST | `/api/students` | Public |
| PUT | `/api/students/{id}` | Admin, Student |
| DELETE | `/api/students/{id}` | Admin |

### Courses
| Method | Route | Auth |
|---|---|---|
| GET | `/api/courses` | Any authenticated |
| GET | `/api/courses/{id}` | Any authenticated |
| POST | `/api/courses` | Admin, Instructor |
| PUT | `/api/courses/{id}` | Admin, Instructor |
| DELETE | `/api/courses/{id}` | Admin |

### Enrollments
| Method | Route | Auth |
|---|---|---|
| GET | `/api/enrollments` | Admin |
| GET | `/api/enrollments/student/{studentId}` | Admin, Instructor, Student |
| GET | `/api/enrollments/course/{courseId}` | Admin, Instructor |
| POST | `/api/enrollments` | Admin, Student |
| PUT | `/api/enrollments/student/{sId}/course/{cId}` | Admin, Instructor |
| DELETE | `/api/enrollments/student/{sId}/course/{cId}` | Admin, Student |

---

## Why HTTP-Only Cookies Are an Industry Standard for Auth Security

Although this project uses JWT tokens sent via the `Authorization` header (common for APIs and SPAs), HTTP-only cookies are widely preferred in browser-based applications for the following reasons:

- **XSS protection**: JavaScript cannot access HTTP-only cookies, so even if an attacker injects malicious scripts into the page, they cannot steal the authentication token.
- **Automatic handling**: The browser automatically attaches cookies to every request — no manual token management in JavaScript code.
- **Secure + SameSite flags**: Combined with `Secure` (HTTPS only) and `SameSite=Strict` or `SameSite=Lax`, cookies are protected against both XSS and CSRF attacks.
- **Session revocation**: Server-side session stores backed by cookies can be instantly invalidated, unlike JWTs which remain valid until expiry.

For purely API-driven or mobile clients, Authorization header JWTs are acceptable. For web apps with a browser frontend, HTTP-only cookies are the safer default.
