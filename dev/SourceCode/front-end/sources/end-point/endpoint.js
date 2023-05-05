function getEndPoint()
{
    const endPoint = 
    {
        analyticsHealth: 'https://localhost:7259/analysis/health',
        analyticsRegistration: 'https://localhost:7259/analysis/registrations',
        analyticsLogins: 'https://localhost:7259/analysis/logins',
        analyticsMaps: 'https://localhost:7259/analysis/maps',
        analyticsPins: 'https://localhost:7259/analysis/pins',
        registrationServer: 'https://localhost:7259/account/registration',
        authenticationServer: 'https://localhost:7259/account/authentication',
        disablePin: 'https://localhost:7259/Pin/DisablePin',
        postPin: 'https://localhost:7259/Pin/PostNewPin',
        getAllPin: 'https://localhost:7259/Pin/GetAllPins',
        completeUserPin: 'https://localhost:7259/Pin/CompleteUserPin',
        modifyPinType: 'https://localhost:7259/Pin/ModifyPinType',
        modifyPinContent: 'https://localhost:7259/Pin/ModifyPinContent',

        // * Events
        getEventHealth: 'https://localhost:7259/event/health',
        getEventPins: 'https://localhost:7259/event/getEventPins',
        createEventPin: 'https://localhost:7259/event/createEventPin',
        userJoinedEvents: 'https://localhost:7259/event/userEvents',
        unJoinEvent: 'https://localhost:7259/event/unjoin',
        joinEvent: 'https://localhost:7259/event/join',
        createdEvents: 'https://localhost:7259/event/createdEvents',
        cancelEvent: 'https://localhost:7259/event/deleteEvent',
        modifyTitle: 'https://localhost:7259/event/title',
        modifyDescription: 'https://localhost:7259/event/description',


    }

    return Object.freeze(endPoint);
}
