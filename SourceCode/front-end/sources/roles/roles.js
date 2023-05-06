function getRole()
{
    const roles = 
    {
        reg:['Regular User', 'Reputable User'],
        service: ['Service User'],
        admin: ['Admin User']
    } 

    return Object.freeze(roles)
}