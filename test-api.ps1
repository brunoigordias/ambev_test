# Script de teste da API Ambev Developer Evaluation
# URL base da API
$baseUrl = "http://localhost:62949"

Write-Host "=== Testando API Ambev Developer Evaluation ===" -ForegroundColor Cyan
Write-Host ""

# Função para fazer requisições HTTP
function Invoke-ApiRequest {
    param(
        [string]$Method,
        [string]$Endpoint,
        [object]$Body = $null,
        [string]$Token = $null
    )
    
    $headers = @{
        "Content-Type" = "application/json"
    }
    
    if ($Token) {
        $headers["Authorization"] = "Bearer $Token"
    }
    
    $uri = "$baseUrl$Endpoint"
    
    try {
        if ($Body) {
            $jsonBody = $Body | ConvertTo-Json
            $response = Invoke-RestMethod -Uri $uri -Method $Method -Headers $headers -Body $jsonBody
        } else {
            $response = Invoke-RestMethod -Uri $uri -Method $Method -Headers $headers
        }
        
        return $response
    }
    catch {
        Write-Host "Erro: $($_.Exception.Message)" -ForegroundColor Red
        if ($_.Exception.Response) {
            $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
            $responseBody = $reader.ReadToEnd()
            Write-Host "Resposta: $responseBody" -ForegroundColor Red
        }
        return $null
    }
}

# 1. Criar um novo usuário
Write-Host "1. Criando usuário de teste..." -ForegroundColor Yellow
$createUserBody = @{
    username = "usuario_teste_$(Get-Random)"
    password = "Senha123!@#"
    phone = "(11) 99999-9999"
    email = "teste_$(Get-Random)@example.com"
    status = 0  # Active
    role = 0    # User
}

$newUser = Invoke-ApiRequest -Method "POST" -Endpoint "/api/users" -Body $createUserBody

if ($newUser) {
    Write-Host "Usuário criado com sucesso!" -ForegroundColor Green
    Write-Host "ID: $($newUser.data.id)" -ForegroundColor Green
    Write-Host "Email: $($newUser.data.email)" -ForegroundColor Green
    $userId = $newUser.data.id
    $userEmail = $newUser.data.email
    Write-Host ""
}

# 2. Autenticar o usuário
Write-Host "2. Autenticando usuário..." -ForegroundColor Yellow
$authBody = @{
    email = $userEmail
    password = "Senha123!@#"
}

$authResponse = Invoke-ApiRequest -Method "POST" -Endpoint "/api/auth" -Body $authBody

if ($authResponse) {
    Write-Host "Autenticação realizada com sucesso!" -ForegroundColor Green
    $token = $authResponse.data.token
    Write-Host "Token recebido: $($token.Substring(0, [Math]::Min(50, $token.Length)))..." -ForegroundColor Green
    Write-Host ""
} else {
    Write-Host "Erro na autenticação. Continuando sem token..." -ForegroundColor Yellow
    $token = $null
}

# 3. Buscar o usuário criado
Write-Host "3. Buscando usuário criado..." -ForegroundColor Yellow
if ($userId) {
    $getUser = Invoke-ApiRequest -Method "GET" -Endpoint "/api/users/$userId" -Token $token
    
    if ($getUser) {
        Write-Host "Usuário encontrado!" -ForegroundColor Green
        Write-Host "Username: $($getUser.data.username)" -ForegroundColor Green
        Write-Host "Email: $($getUser.data.email)" -ForegroundColor Green
        Write-Host ""
    }
}

# 4. Health Check
Write-Host "4. Verificando saúde da API..." -ForegroundColor Yellow
$health = Invoke-ApiRequest -Method "GET" -Endpoint "/health"

if ($health) {
    Write-Host "API está saudável!" -ForegroundColor Green
    Write-Host ""
}

Write-Host "=== Testes concluídos ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Para testar mais endpoints, acesse o Swagger em:" -ForegroundColor Cyan
Write-Host "$baseUrl/swagger" -ForegroundColor Yellow


