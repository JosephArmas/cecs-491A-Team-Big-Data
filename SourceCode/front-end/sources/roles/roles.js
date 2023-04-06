function getRole()
{
    const roles = Object.freeze 
    {
        reg:['Regular User', 'Reputation User'];
        service: ['Service User'];
        admin: ['Admin User'];
    } 

    return roles
}