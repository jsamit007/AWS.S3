using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;

namespace AWS.Cognito;

public class CognitoService
{
    private readonly IAmazonCognitoIdentityProvider _client;

    public CognitoService(IAmazonCognitoIdentityProvider client)
    {
        _client = client;
    }

    public async Task<AdminInitiateAuthResponse> AdminInitiateAuthResponseAsync(string userPoolId, string clientId, string username, string password)
    {
        var authRequest = new AdminInitiateAuthRequest
        {
            UserPoolId = userPoolId,
            ClientId = clientId,
            AuthFlow = AuthFlowType.ADMIN_USER_PASSWORD_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "USERNAME", username },
                { "PASSWORD", password }
            }
        };

        return await _client.AdminInitiateAuthAsync(authRequest);
    }
}
