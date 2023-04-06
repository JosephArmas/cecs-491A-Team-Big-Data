function getEndpoint()
{
    const endPoint = Object.freeze
    {
        analyticsHealth: 'https://localhost:7259/analysis/health';
        analyticsRegistration: 'https://localhost:7259/analysis/registrations';
        analyticsLogins: 'https://localhost:7259/analysis/logins';
        analyticsMaps: 'https://localhost:7259/analysis/maps';
        analyticsPins: 'https://localhost:7259/analysis/pins';
        registrationServer: 'https://localhost:7259/account/registration';
        authenticationServer: 'https://localhost:7259/account/authentication';
    }

    return endPoint;
}
