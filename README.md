# Job Tracker

Job Hunt is a SaaS platform designed to assist job seekers in managing their job applications. This backend API provides features to track job applications, manage their status, and store relevant details such as company information, job titles, application status, salary range, and more.

## Features

- User authentication and authorization with JWT
- CRUD operations for job applications and interviews
- Track job titles, companies, industries, locations
- Application status tracking and interview scheduling
- Email notifications for important updates
- Rate limiting for API protection
- Secure payment processing with Stripe
- Cross-Origin Resource Sharing (CORS) support

## Technology Stack

- **Backend Framework**: ASP.NET Core 9.0
- **Database**: PostgreSQL
- **ORM**: Entity Framework Core
- **Authentication**: JWT Bearer Authentication
- **Documentation**: Swagger/OpenAPI
- **Email**: SMTP Integration
- **Payments**: Stripe
- **Validation**: FluentValidation
- **Security**: BCrypt for password hashing

## Prerequisites

- [.NET 9.0 or later](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Postman](https://www.postman.com/) or any API testing tool
- SMTP server access for email notifications
- Stripe account for payment processing

## Getting Started

1. **Clone the Repository**

```bash
git clone https://github.com/yourusername/job-hunt-backend.git
cd job-hunt-backend
```

2. **Install Dependencies**

```bash
dotnet restore
```

3. **Configure Settings**

Copy `appsettings.example.json` to `appsettings.json` and update with your values:

```bash
cp appsettings.example.json appsettings.json
```

Update the following in your new `appsettings.json`:

- Database connection string
- JWT secret key
- SMTP credentials
- Stripe API keys
- Allowed CORS origins

4. **Setup Database**

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

5. **Run the Application**

```bash
dotnet run
```

The API will be available at `https://localhost:5001`

## API Documentation

### Authentication Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/auth/register` | Register new user |
| POST | `/api/auth/login` | Login user |
| POST | `/api/auth/refresh` | Refresh JWT token |

### Jobs Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/jobs` | Get all job applications |
| GET | `/api/jobs/{id}` | Get job by ID |
| POST | `/api/jobs` | Create new job application |
| PUT | `/api/jobs/{id}` | Update existing job |
| DELETE | `/api/jobs/{id}` | Delete job application |

### Interviews Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/jobs/{jobId}/interviews` | Get all interviews for a job |
| POST | `/api/jobs/{jobId}/interviews` | Schedule new interview |
| PUT | `/api/jobs/{jobId}/interviews/{id}` | Update interview details |
| DELETE | `/api/jobs/{jobId}/interviews/{id}` | Delete interview |

## Request Examples

### Create Job Application

```json
{
  "jobTitle": "Software Developer",
  "company": "Tech Corp",
  "industry": "Technology",
  "location": "New York",
  "companyOverview": "Leading tech company",
  "whyIWantToWorkHere": "Great growth opportunities",
  "dateApplied": "2024-12-01T00:00:00",
  "jobPostingLink": "<https://example.com/job/123>",
  "contactNameOrInfo": "John Doe",
  "applicationStatus": "Applied",
  "followUpDate": "2024-12-15T00:00:00",
  "notes": "Remember to follow up",
  "resumeVersionUsed": "v1.2",
  "referral": "Jane Smith",
  "salaryRange": "$80,000 - $100,000"
}

```

### Schedule Interview
```json
{
  "scheduledAt": "2024-12-20T14:00:00",
  "interviewType": "Technical",
  "interviewerName": "John Smith",
  "location": "Virtual/Zoom",
  "notes": "System design discussion"
}
```

## Authentication

The API uses JWT Bearer authentication. Include the JWT token in the Authorization header:

```
Authorization: Bearer <your_token>
```

## Rate Limiting

- General endpoints: 100 requests per minute
- Authentication endpoints: 10 requests per minute

## Error Handling

The API uses standard HTTP status codes and returns error responses in the following format:

```json
{
  "error": "Error message here",
  "details": "Additional error details if available"
}
```

## Development

- Use Swagger UI for API testing (available in development at `/swagger`)
- Configure environment-specific settings in `appsettings.Development.json`
- Enable detailed error messages in development mode

## Configuration

### Sample appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=JobHunt;Username=yourusername;Password=yourpassword"
  },
  "JwtSettings": {
    "SecretKey": "YourVeryLongAndSecureKeyHereThatIsAtLeast32CharactersLong",
    "Issuer": "JobHuntBackend",
    "Audience": "JobHuntUsers",
    "ExpirationInMinutes": 60
  },
  "RateLimiting": {
    "GeneralLimit": {
      "PermitLimit": 100,
      "Window": 60,
      "ReplenishmentPeriod": 1
    },
    "AuthLimit": {
      "PermitLimit": 10,
      "Window": 60,
      "ReplenishmentPeriod": 1
    }
  },
  "AllowedOrigins": [
    "http://localhost:3000",
    "https://yourdomain.com"
  ],
  "EmailSettings": {
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUsername": "your-email@gmail.com",
    "SmtpPassword": "your-app-specific-password",
    "FromEmail": "your-email@gmail.com",
    "FromName": "Job Hunt App"
  },
  "StripeSettings": {
    "SecretKey": "sk_test_your_stripe_secret_key",
    "WebhookSecret": "whsec_your_webhook_secret",
    "PublishableKey": "pk_test_your_publishable_key"
  }
}
```

## Security Considerations

- All endpoints (except authentication) require valid JWT token
- Passwords are hashed using BCrypt
- Rate limiting prevents brute force attacks
- CORS policy restricts allowed origins
- Sensitive data is never logged
- HTTPS is enforced in production

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## License

This project is licensed under the MIT License - see the LICENSE file for details.
