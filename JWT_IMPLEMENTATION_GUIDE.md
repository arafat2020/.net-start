# JWT Authentication Implementation Guide

This guide explains how JWT (JSON Web Token) authentication is implemented in this .NET Web API and how to use it.

## 1. Concepts

**JWT (JSON Web Token)** is a compact, URL-safe means of representing claims to be transferred between two parties. It consists of three parts:
- **Header**: Type of token and algorithm.
- **Payload**: Claims (user identity, roles, etc.).
- **Signature**: Used to verify the token hasn't been tampered with.

## 2. Implementation Steps

### A. Install Packages
We installed the `Microsoft.AspNetCore.Authentication.JwtBearer` package.

### B. Configuration (`appsettings.json`)
We added a `Jwt` section:
```json
"Jwt": {
  "Key": "YourSuperSecretKey...",
  "Issuer": "PaymentApi",
  "Audience": "PaymentApiUsers"
}
```

### C. Services Setup (`Program.cs`)
We registered the authentication services and configured the `JwtBearer` options to validate the issuer, audience, and signing key.

### D. Protected Routes
The `PaymentsController` now has the `[Authorize]` attribute. This means any request to this controller MUST include a valid JWT in the Header:
`Authorization: Bearer <token>`

## 3. How to Use

### Step 1: Login
Use the `AuthController` to get a token.
- **Endpoint**: `POST /api/auth/login`
- **Body**:
  ```json
  {
    "username": "admin",
    "password": "password123"
  }
  ```
- **Response**:
  ```json
  {
    "token": "eyJhbG..."
  }
  ```

### Step 2: Use Token in Swagger
1. Click the **"Authorize"** button at the top of the Swagger page.
2. In the "Value" field, type `Bearer <your_token>` (replace `<your_token>` with the one you got from login).
3. Click **Authorize**.
4. Now you can call the `Payments` endpoints!

### Step 3: Use Token in Raw Requests
Add the following header to your HTTP request:
`Authorization: Bearer <your_token>`

## 4. Security Notes
- **Never** hardcode credentials or secrets in a production app. Use **Environment Variables** or **Azure Key Vault**.
- Use a **longer, more complex secret key** in production.
- Implement **Refresh Tokens** for better security and user experience.
