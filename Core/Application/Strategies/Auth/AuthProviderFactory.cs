namespace custom_chat_backend.Core.Application.Strategies.Auth;

public class AuthProviderFactory
{
    private readonly IEnumerable<IExternalAuthProviderStrategy> _strategies;

    public AuthProviderFactory(IEnumerable<IExternalAuthProviderStrategy> strategies)
    {
        _strategies = strategies;
    }

    public IExternalAuthProviderStrategy GetStrategy(string provider)
    {
        var strategy = _strategies.FirstOrDefault(s => 
            s.ProviderName.Equals(provider, StringComparison.OrdinalIgnoreCase));

        return strategy ?? throw new ArgumentException($"El proveedor de autenticación '{provider}' no está soportado.");
    }
}