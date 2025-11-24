# Guia de Teste da API - Ambev Developer Evaluation

## 📋 Status do Projeto

✅ **Projeto está rodando!**

- **Container da API**: `ambev_developer_evaluation_webapi` (ATIVO)
- **Porta da API**: `http://localhost:62949`
- **Swagger UI**: `http://localhost:62949/swagger`
- **Banco de Dados**: PostgreSQL (ATIVO)
- **Cache**: Redis (ATIVO)
- **NoSQL**: MongoDB (ATIVO)

---

## 🚀 Como Testar a API

### Opção 1: Swagger UI (Recomendado)

1. Abra seu navegador
2. Acesse: **http://localhost:62949/swagger**
3. Explore e teste todos os endpoints diretamente na interface

### Opção 2: Script PowerShell Automatizado

Execute o script de teste que criamos:

```powershell
.\test-api.ps1
```

Este script irá:
- Criar um usuário de teste
- Autenticar o usuário
- Buscar o usuário criado
- Verificar a saúde da API

### Opção 3: Requisições Manuais

#### Endpoints Disponíveis

##### 1. **POST /api/users** - Criar Usuário

```powershell
$body = @{
    username = "joao.silva"
    password = "Senha123!@#"
    phone = "(11) 98765-4321"
    email = "joao.silva@example.com"
    status = 0  # 0 = Active, 1 = Inactive
    role = 0    # 0 = User, 1 = Admin
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:62949/api/users" `
    -Method POST `
    -ContentType "application/json" `
    -Body $body
```

##### 2. **POST /api/auth** - Autenticar Usuário

```powershell
$body = @{
    email = "joao.silva@example.com"
    password = "Senha123!@#"
} | ConvertTo-Json

$response = Invoke-RestMethod -Uri "http://localhost:62949/api/auth" `
    -Method POST `
    -ContentType "application/json" `
    -Body $body

$token = $response.data.token
Write-Host "Token: $token"
```

##### 3. **GET /api/users/{id}** - Buscar Usuário

```powershell
$userId = "GUID_DO_USUARIO_AQUI"
$token = "SEU_TOKEN_AQUI"

$headers = @{
    "Authorization" = "Bearer $token"
}

Invoke-RestMethod -Uri "http://localhost:62949/api/users/$userId" `
    -Method GET `
    -Headers $headers
```

##### 4. **DELETE /api/users/{id}** - Deletar Usuário

```powershell
$userId = "GUID_DO_USUARIO_AQUI"
$token = "SEU_TOKEN_AQUI"

$headers = @{
    "Authorization" = "Bearer $token"
}

Invoke-RestMethod -Uri "http://localhost:62949/api/users/$userId" `
    -Method DELETE `
    -Headers $headers
```

##### 5. **GET /health** - Health Check

```powershell
Invoke-RestMethod -Uri "http://localhost:62949/health" -Method GET
```

---

## 🔧 Exemplos com cURL (para outras ferramentas)

### Criar Usuário
```bash
curl -X POST "http://localhost:62949/api/users" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "maria.santos",
    "password": "Senha123!@#",
    "phone": "(11) 98765-4321",
    "email": "maria.santos@example.com",
    "status": 0,
    "role": 0
  }'
```

### Autenticar
```bash
curl -X POST "http://localhost:62949/api/auth" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "maria.santos@example.com",
    "password": "Senha123!@#"
  }'
```

### Buscar Usuário (com autenticação)
```bash
curl -X GET "http://localhost:62949/api/users/{USER_ID}" \
  -H "Authorization: Bearer {SEU_TOKEN}"
```

---

## 📊 Estrutura de Respostas

### Resposta de Sucesso (com dados)
```json
{
  "success": true,
  "message": "User created successfully",
  "data": {
    "id": "guid-aqui",
    "username": "joao.silva",
    "email": "joao.silva@example.com",
    ...
  }
}
```

### Resposta de Erro
```json
{
  "success": false,
  "message": "Error message here"
}
```

---

## 🔍 Verificar Status dos Containers

Para verificar se todos os containers estão rodando:

```powershell
docker ps
```

Você deve ver:
- ✅ `ambev_developer_evaluation_webapi`
- ✅ `ambev_developer_evaluation_database`
- ✅ `ambev_developer_evaluation_cache`
- ✅ `ambev_developer_evaluation_nosql`

---

## 🛠️ Comandos Úteis

### Ver logs da API
```powershell
docker logs ambev_developer_evaluation_webapi -f
```

### Reiniciar a API
```powershell
docker restart ambev_developer_evaluation_webapi
```

### Parar todos os containers
```powershell
docker-compose down
```

### Iniciar todos os containers
```powershell
docker-compose up -d
```

---

## ⚠️ Notas Importantes

1. **Porta dinâmica**: A porta mapeada pode mudar se você reiniciar os containers. Verifique sempre com `docker ps`.

2. **Autenticação**: Alguns endpoints requerem autenticação JWT. Use o endpoint `/api/auth` para obter o token.

3. **Ambiente de Desenvolvimento**: O Swagger só está disponível em ambiente de desenvolvimento.

4. **Validações**: Todos os endpoints têm validações. Verifique os campos obrigatórios antes de fazer requisições.

---

## 📝 Próximos Passos

1. Teste o Swagger UI para explorar a API interativamente
2. Execute o script `test-api.ps1` para testes automatizados
3. Use os exemplos acima para integração em outras ferramentas
4. Verifique os logs se encontrar algum erro


