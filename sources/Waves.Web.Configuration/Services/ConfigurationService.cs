using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Waves.Web.Configuration.Entities;
using Waves.Web.Configuration.Services.Interfaces;

namespace Waves.Web.Configuration.Services;

/// <summary>
/// Configuration service.
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ConfigurationService> _logger;

    private ConfigurationRootEntity _configurationEntity;

    /// <summary>
    /// Creates new instance of <see cref="ConfigurationService"/>.
    /// </summary>
    /// <param name="configuration">Configuration.</param>
    /// <param name="logger">Logger.</param>
    public ConfigurationService(
        IConfiguration configuration,
        ILogger<ConfigurationService> logger)
    {
        _configuration = configuration;
        _logger = logger;
        InitializeConfiguration();
    }

    /// <inheritdoc />
    public CredentialEntity GetCredentialEntity(string name)
    {
        return _configurationEntity.Credentials.FirstOrDefault(x => x.Key == name).Value;
    }

    /// <inheritdoc />
    public AuthenticationEntity GetAuthenticationEntity(string name)
    {
        return _configurationEntity.Tokens.FirstOrDefault(x => x.Key == name).Value;
    }

    /// <inheritdoc />
    public string GetUrl(string name)
    {
        return _configurationEntity.Urls.FirstOrDefault(x => x.Key == name).Value;
    }

    /// <inheritdoc />
    public string GetTokenSecret()
    {
        return _configurationEntity.TokenSecret;
    }

    /// <inheritdoc />
    public Task Save()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes configuration.
    /// </summary>
    private void InitializeConfiguration()
    {
        var tokenNames = new List<string>()
        {
            // (none)
        };

        var credentialNames = new List<string>()
        {
            "Psql",
            "Admin",
        };

        var urlNames = new List<string>()
        {
            "Psql",
        };

        try
        {
            _configurationEntity = new ConfigurationRootEntity
            {
                TokenSecret = _configuration
                    .GetSection("TokenSecret")
                    .Value,
            };

            // tokens
            foreach (var name in tokenNames)
            {
                var token = string.Empty;
                var secret = string.Empty;
                try
                {
                    token = _configuration
                        .GetSection("Tokens")
                        .GetSection(name)
                        .GetSection("Token")
                        .Value;
                }
                catch (Exception e)
                {
                    _logger.LogWarning("Error occured while get token for {Name} not set: {Message}", name, e.Message);
                }

                try
                {
                    secret = _configuration
                        .GetSection("Tokens")
                        .GetSection(name)
                        .GetSection("Secret")
                        .Value;
                }
                catch (Exception e)
                {
                    _logger.LogWarning("Error occured while get secret for {Name} not set: {Message}", name, e.Message);
                }

                if (string.IsNullOrEmpty(token) && string.IsNullOrEmpty(secret))
                {
                    _logger.LogWarning("Authentication data for {Name} not set", name);
                }
                else
                {
                    var credential = new AuthenticationEntity()
                    {
                        Token = token,
                        Secret = secret,
                    };
                    _configurationEntity.Tokens.Add(name, credential);
                }
            }

            // credentials
            foreach (var name in credentialNames)
            {
                var userName = string.Empty;
                var password = string.Empty;

                try
                {
                    userName = _configuration
                        .GetSection("Credentials")
                        .GetSection(name)
                        .GetSection("Username")
                        .Value;
                }
                catch (Exception e)
                {
                    _logger.LogWarning("Error occured while get username for {Name} not set: {Message}", name, e.Message);
                }

                try
                {
                    password = _configuration
                        .GetSection("Credentials")
                        .GetSection(name)
                        .GetSection("Password")
                        .Value;
                }
                catch (Exception e)
                {
                    _logger.LogWarning("Error occured while get password for {Name} not set: {Message}", name, e.Message);
                }

                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                {
                    var credential = new CredentialEntity()
                    {
                        Username = userName,
                        Password = password,
                    };
                    _configurationEntity.Credentials.Add(name, credential);
                }
                else
                {
                    _logger.LogWarning("Credentials for {Name} not set", name);
                }
            }

            // urls
            foreach (var name in urlNames)
            {
                try
                {
                    var url = _configuration
                        .GetSection("Urls")
                        .GetSection(name)
                        .Value;

                    if (!string.IsNullOrEmpty(url))
                    {
                        _configurationEntity.Urls.Add(name, url);
                    }
                    else
                    {
                        _logger.LogWarning("Url for {Name} not set", name);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogWarning("Error occured while get url for {Name} not set: {Message}", name, e.Message);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error occured while initialize configuration: {Message}", e.Message);
        }
    }
}
