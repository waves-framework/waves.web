using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Waves.Web.Configuration.Options;
using Waves.Web.Configuration.Services.Interfaces;

namespace Waves.Web.Configuration.Services;

/// <summary>
/// Configuration service.
/// </summary>
public class ConfigurationService : IConfigurationService
{
    private readonly ILogger<ConfigurationService> _logger;
    private readonly ServiceOptions _options;

    /// <summary>
    /// Creates new instance of <see cref="ConfigurationService"/>.
    /// </summary>
    /// <param name="options">Options.</param>
    /// <param name="logger">Logger.</param>
    public ConfigurationService(
        IOptions<ServiceOptions> options,
        ILogger<ConfigurationService> logger)
    {
        _logger = logger;

        if (options.Value != null)
        {
            _options = options.Value;
        }
    }

    /// <inheritdoc />
    public AuthenticationOptions GetAuthentication()
    {
        return _options.Authentication;
    }

    /// <inheritdoc />
    public CredentialOption GetCredential(string name)
    {
        return _options.Credentials.FirstOrDefault(x => x.Key.Equals(name)).Value;
    }

    /// <inheritdoc />
    public SecretOption GetSecret(string name)
    {
        return _options.Secrets.FirstOrDefault(x => x.Key.Equals(name)).Value;
    }

    /// <inheritdoc />
    public string GetConnectionUrl(string name)
    {
        return _options.Urls.FirstOrDefault(x => x.Key.Equals(name)).Value;
    }

    /// <inheritdoc />
    public string GetSetting(string name)
    {
        return _options.Settings.FirstOrDefault(x => x.Key.Equals(name)).Value;
    }

    /// <inheritdoc />
    public Task Save()
    {
        throw new NotImplementedException();
    }
}
