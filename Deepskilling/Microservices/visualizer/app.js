// JWT Authentication & Authorization Simulator

// C# Code displays for Questions 1-4
const questions = {
    1: {
        category: "Question 1: Implement JWT Authentication",
        title: "Implement JWT Authentication in ASP.NET Core Web API",
        desc: "Build a login endpoint that verifies credentials and returns a secure JWT token signed with SymmetricSecurityKey using HMAC SHA256.",
        file: "AuthController.cs",
        code: [
            "[HttpPost(\"login\")]",
            "public IActionResult Login([FromBody] LoginModel model) {",
            "    if (IsValidUser(model)) {",
            "        var token = GenerateJwtToken(model.Username, model.Role);",
            "        return Ok(new { Token = token });",
            "    }",
            "    return Unauthorized();",
            "}",
            "",
            "private string GenerateJwtToken(string username, string role) {",
            "    var claims = new[] {",
            "        new Claim(ClaimTypes.Name, username),",
            "        new Claim(ClaimTypes.Role, role)",
            "    };",
            "    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(\"ThisIsASecretKey...\"));",
            "    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);",
            "    var token = new JwtSecurityToken(issuer: \"MyAuthServer\", audience: \"MyApiUsers\", claims: claims,",
            "        expires: DateTime.Now.AddMinutes(60), signingCredentials: creds);",
            "    return new JwtSecurityTokenHandler().WriteToken(token);",
            "}"
        ]
    },
    2: {
        category: "Question 2: Secure Endpoint",
        title: "Secure an API Endpoint Using JWT Bearer Auth",
        desc: "Protect actions by applying the standard [Authorize] metadata validation filter, returning 401 Unauthorized if the client lacks a valid token header.",
        file: "SecureController.cs",
        code: [
            "[ApiController]",
            "[Route(\"api/[controller]\")]",
            "public class SecureController : ControllerBase {",
            "    ",
            "    [HttpGet(\"data\")]",
            "    [Authorize] // Requires a valid JWT token signed by MyAuthServer",
            "    public IActionResult GetSecureData() {",
            "        return Ok(new { Message = \"This is protected data. Authorization successful!\" });",
            "    }",
            "}"
        ]
    },
    3: {
        category: "Question 3: Role Authorization",
        title: "Add Role-Based Authorization Restrictions",
        desc: "Link role claims inside user security tokens and query permissions on controllers using parameter rules like [Authorize(Roles = \"Admin\")].",
        file: "AdminController.cs",
        code: [
            "[ApiController]",
            "[Route(\"api/[controller]\")]",
            "public class AdminController : ControllerBase {",
            "    ",
            "    [HttpGet(\"dashboard\")]",
            "    [Authorize(Roles = \"Admin\")] // Blocks execution unless user has the 'Admin' role claim",
            "    public IActionResult GetAdminDashboard() {",
            "        return Ok(new { Message = \"Welcome to the admin dashboard. Elevated privileges verified!\" });",
            "    }",
            "}"
        ]
    },
    4: {
        category: "Question 4: Expiry & Authentication Events",
        title: "Validate JWT Token Expiry and Capture Bearer Events",
        desc: "Configure events inside middleware to handle expired tokens. Capture SecurityTokenExpiredException to inject custom headers in response streams.",
        file: "Program.cs",
        code: [
            "builder.Services.AddAuthentication(\"Bearer\")",
            "    .AddJwtBearer(\"Bearer\", options => {",
            "        options.TokenValidationParameters = new TokenValidationParameters { ... };",
            "        options.Events = new JwtBearerEvents {",
            "            OnAuthenticationFailed = context => {",
            "                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException)) {",
            "                    context.Response.Headers.Add(\"Token-Expired\", \"true\");",
            "                }",
            "                return Task.CompletedTask;",
            "            }",
            "        };",
            "    });"
        ]
    }
};

// Application Client State
let activeQuestion = 1;
let activeToken = null;
let tokenExpired = false;
let userClaims = null;

// Base64 helper utils
function base64urlEncode(jsonObj) {
    const jsonStr = JSON.stringify(jsonObj);
    const base64 = btoa(unescape(encodeURIComponent(jsonStr)));
    return base64.replace(/=/g, '').replace(/\+/g, '-').replace(/\//g, '_');
}

function base64urlDecode(str) {
    // Add padding
    let base64 = str.replace(/-/g, '+').replace(/_/g, '/');
    while (base64.length % 4) {
        base64 += '=';
    }
    return decodeURIComponent(escape(atob(base64)));
}

// Generate a real decodable JWT Token structure
function generateMockToken(username, role) {
    const header = {
        alg: "HS256",
        typ: "JWT"
    };

    // Calculate expiry timestamp (seconds from epoch)
    const expTime = Math.floor(Date.now() / 1000) + 3600; // 1 hour duration

    const payload = {
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": username,
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role": role,
        iss: "MyAuthServer",
        aud: "MyApiUsers",
        exp: expTime,
        jti: "d0f8ea33-8a07-42ad-bbcb-b2f5ea11db28"
    };

    const encodedHeader = base64urlEncode(header);
    const encodedPayload = base64urlEncode(payload);
    // Mock signature using HMAC
    const encodedSignature = "pYvF_sHqG8B4mX9V63LzQ-W1oP2K-J0tR5c_wE8xM2q";

    return {
        tokenString: `${encodedHeader}.${encodedPayload}.${encodedSignature}`,
        header: header,
        payload: payload
    };
}

// Update Active Question Source panel
function loadQuestion(qId) {
    const q = questions[qId];
    document.getElementById("current-category").innerText = q.category;
    document.getElementById("q-title").innerText = q.title;
    document.getElementById("q-explanation").innerText = q.desc;
    document.getElementById("csharp-file-name").innerText = q.file;

    const codeDisplay = document.getElementById("code-display");
    codeDisplay.innerHTML = "";

    q.code.forEach((line, index) => {
        const lineSpan = document.createElement("span");
        lineSpan.className = "code-line-span";
        lineSpan.innerText = line || " ";
        codeDisplay.appendChild(lineSpan);
    });
}

// Handle User Authentication request simulation
function handleLogin() {
    const usernameInput = document.getElementById("username").value.trim();
    const passwordInput = document.getElementById("password").value;
    const selectedRole = document.getElementById("role-select").value;

    if (!usernameInput || !passwordInput) {
        alert("Please enter username and password!");
        return;
    }

    if (passwordInput !== "password") {
        document.getElementById("client-auth-state").innerText = "Unauthorized";
        document.getElementById("client-auth-state").style.backgroundColor = "rgba(239, 68, 68, 0.1)";
        document.getElementById("client-auth-state").style.color = "var(--error)";
        
        document.getElementById("encoded-token-display").innerText = "Unauthorized: Login Failed.";
        document.getElementById("decoded-claims-display").innerText = "{\n  \"error\": \"401 Invalid Credentials\"\n}";
        activeToken = null;
        return;
    }

    // Generate Token
    const jwtObj = generateMockToken(usernameInput, selectedRole);
    activeToken = jwtObj.tokenString;
    tokenExpired = false;
    userClaims = jwtObj.payload;

    // Render Encoded Token String with colors (jwt.io style)
    const parts = activeToken.split('.');
    document.getElementById("encoded-token-display").innerHTML = `
        <span class="jwt-part-header">${parts[0]}</span>.<span class="jwt-part-payload">${parts[1]}</span>.<span class="jwt-part-signature">${parts[2]}</span>
    `;

    // Render Decoded JSON Payload
    document.getElementById("decoded-claims-display").innerText = JSON.stringify(jwtObj.payload, null, 2);

    // Update state badges
    const authState = document.getElementById("client-auth-state");
    authState.innerText = `Token Active (${selectedRole})`;
    authState.style.backgroundColor = "rgba(16, 185, 129, 0.1)";
    authState.style.color = "var(--success)";
}

// Simulate Token Expiry (updates exp payload and state)
function simulateExpiry() {
    if (!activeToken) {
        alert("Please generate a token first by logging in.");
        return;
    }

    tokenExpired = true;
    
    // Set exp timestamp to past
    userClaims.exp = Math.floor(Date.now() / 1000) - 60; // Expired 1 min ago
    
    // Re-encode
    const parts = activeToken.split('.');
    const encodedPayload = base64urlEncode(userClaims);
    activeToken = `${parts[0]}.${encodedPayload}.${parts[2]}`;

    // Update views
    document.getElementById("encoded-token-display").innerHTML = `
        <span class="jwt-part-header">${parts[0]}</span>.<span class="jwt-part-payload">${encodedPayload}</span>.<span class="jwt-part-signature">${parts[2]}</span>
    `;
    document.getElementById("decoded-claims-display").innerText = JSON.stringify(userClaims, null, 2);

    const authState = document.getElementById("client-auth-state");
    authState.innerText = "Token Expired";
    authState.style.backgroundColor = "rgba(239, 68, 68, 0.1)";
    authState.style.color = "var(--error)";
}

// Simulate requesting endpoints
function executeApiCall() {
    const selectedEndpoint = document.getElementById("api-endpoint-select").value;
    const includeAuth = document.getElementById("include-auth-header").checked;

    const statusBadge = document.getElementById("response-status-code");
    const headersDisplay = document.getElementById("response-headers-display");
    const bodyDisplay = document.getElementById("response-body-display");

    statusBadge.className = "status-badge";
    headersDisplay.innerText = "";
    bodyDisplay.innerText = "";

    // 1. Check Auth presence
    if (!includeAuth || !activeToken) {
        // Q2 logic: Endpoint blocked without authorization
        statusBadge.innerText = "401 Unauthorized";
        statusBadge.classList.add("status-unauthorized");
        headersDisplay.innerText = "Date: " + new Date().toUTCString() + "\nServer: Kestrel\nWWW-Authenticate: Bearer";
        bodyDisplay.innerText = "{\n  \"status\": 401,\n  \"title\": \"Unauthorized\",\n  \"detail\": \"Lacking valid credentials authorization headers.\"\n}";
        return;
    }

    // 2. Check Token Expiration (Q4 logic)
    if (tokenExpired) {
        statusBadge.innerText = "401 Unauthorized";
        statusBadge.classList.add("status-unauthorized");
        // Q4 header injection check:
        headersDisplay.innerText = "Date: " + new Date().toUTCString() + "\nServer: Kestrel\nToken-Expired: true\nWWW-Authenticate: Bearer error=\"invalid_token\", error_description=\"The token expired\"";
        bodyDisplay.innerText = "{\n  \"status\": 401,\n  \"title\": \"Unauthorized\",\n  \"detail\": \"The authentication token has expired.\"\n}";
        return;
    }

    // 3. Perform Role Verification checks (Q3 logic)
    const userRole = userClaims["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
    const username = userClaims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];

    if (selectedEndpoint === "secure") {
        // General authorize success
        statusBadge.innerText = "200 OK";
        statusBadge.classList.add("status-success");
        headersDisplay.innerText = "Date: " + new Date().toUTCString() + "\nServer: Kestrel\nContent-Type: application/json; charset=utf-8";
        bodyDisplay.innerText = JSON.stringify({
            Message: "This is protected data. Authorization successful!",
            AuthorizedUser: username,
            AssignedRole: userRole
        }, null, 2);
    } 
    else if (selectedEndpoint === "admin") {
        if (userRole === "Admin") {
            // Admin role access success
            statusBadge.innerText = "200 OK";
            statusBadge.classList.add("status-success");
            headersDisplay.innerText = "Date: " + new Date().toUTCString() + "\nServer: Kestrel\nContent-Type: application/json; charset=utf-8";
            bodyDisplay.innerText = JSON.stringify({
                Message: "Welcome to the admin dashboard. Elevated privileges verified!",
                AuthorizedAdmin: username,
                Permissions: "ReadWriteAll"
            }, null, 2);
        } else {
            // Role unauthorized (403 Forbidden)
            statusBadge.innerText = "403 Forbidden";
            statusBadge.classList.add("status-forbidden");
            headersDisplay.innerText = "Date: " + new Date().toUTCString() + "\nServer: Kestrel\nContent-Type: application/json; charset=utf-8";
            bodyDisplay.innerText = JSON.stringify({
                status: 403,
                title: "Forbidden",
                detail: "Access denied. Insufficient privileges to view the Admin Dashboard."
            }, null, 2);
        }
    }
}

// Setup Event listeners
document.addEventListener("DOMContentLoaded", () => {
    // Select Question
    document.querySelectorAll(".nav-btn").forEach(btn => {
        btn.addEventListener("click", (e) => {
            document.querySelectorAll(".nav-btn").forEach(b => b.classList.remove("active"));
            e.currentTarget.classList.add("active");
            activeQuestion = parseInt(e.currentTarget.getAttribute("data-q"));
            loadQuestion(activeQuestion);
        });
    });

    // Control triggers
    document.getElementById("btn-login").addEventListener("click", handleLogin);
    document.getElementById("btn-simulate-expire").addEventListener("click", simulateExpiry);
    document.getElementById("btn-send-request").addEventListener("click", executeApiCall);

    // Initial load Q1
    loadQuestion(1);
});
