using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Rebus.Handlers;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Handler for UserRegisteredEvent messages from Rebus
/// </summary>
public class UserRegisteredEventHandler : IHandleMessages<UserRegisteredEvent>
{
    private readonly ILogger<UserRegisteredEventHandler> _logger;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of UserRegisteredEventHandler
    /// </summary>
    public UserRegisteredEventHandler(
        ILogger<UserRegisteredEventHandler> logger,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Handles the UserRegisteredEvent message
    /// </summary>
    public async Task Handle(UserRegisteredEvent message)
    {
        try
        {
            _logger.LogInformation(
                "Processando UserRegisteredEvent - UserId: {UserId}, Email: {Email}, Username: {Username}, Role: {Role}",
                message.User.Id,
                message.User.Email,
                message.User.Username,
                message.User.Role);

            // Validação: Verificar se o usuário existe no banco
            var user = await _userRepository.GetByIdAsync(message.User.Id);
            if (user == null)
            {
                _logger.LogWarning(
                    "Usuário não encontrado no banco de dados - UserId: {UserId}, Email: {Email}",
                    message.User.Id,
                    message.User.Email);
                throw new ArgumentException($"Usuário não encontrado: {message.User.Id}");
            }

            // Validação: Verificar se o email está preenchido
            if (string.IsNullOrWhiteSpace(message.User.Email))
            {
                _logger.LogWarning(
                    "Email do usuário não está preenchido - UserId: {UserId}",
                    message.User.Id);
                throw new ArgumentException("Email do usuário é obrigatório");
            }

            // Validação: Verificar se o status do usuário é válido
            if (message.User.Status == Domain.Enums.UserStatus.Inactive)
            {
                _logger.LogWarning(
                    "Usuário registrado está inativo - UserId: {UserId}, Email: {Email}",
                    message.User.Id,
                    message.User.Email);
            }            

            _logger.LogInformation(
                "UserRegisteredEvent processado com sucesso - UserId: {UserId}, Email: {Email}, Status: {Status}",
                message.User.Id,
                message.User.Email,
                message.User.Status);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Erro ao processar UserRegisteredEvent - UserId: {UserId}, Email: {Email}",
                message.User.Id,
                message.User.Email);
            throw;
        }
    }
}

