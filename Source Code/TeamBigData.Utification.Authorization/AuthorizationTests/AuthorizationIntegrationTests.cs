using System;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Models;
using System.Security.Principal;
using TeamBigData.Utification.Authorization;
using TeamBigData.Utification.AuthZ.Abstraction;


namespace AuthorizationTests
{
    [TestClass]
    public class AuthorizationIntegrationTests
    {
        [TestMethod]
        public void CheckRole(UserProfile userProfile)
        {

            if((IPrincipal)userProfile.IsInRole("Admin User") == true)
            {
                var response = new Response();

            }
        }
        
        public void AnonymousAccess(UserProfile userProfile)
        {
            //Anonymous user is restriced to AnonymousView, RegistrationView, and LoginView
        }

        public void UserProfileReflectsUser(UserProfile userProfile)
        {
            //User can only see their personal Profile
        }

        public void ModificationsAreActive(UserProfile userProfile)
        {
            //Modifications will reflect upon next authentication
        }

        public void RestrictedAccessView(UserProfile userProfile)
        {
            //Access of users is restricted to views
        }
        public void RestrictedAccessModification(UserProfile userProfile)
        {
            //Modification is protected
        }

        public void AdminPrivilegeDelete(UserProfile userProfile)
        {
            //Admins may delete. may not delete self 
        }

        public void AdminMayNotHaveReputation(UserProfile userProfile)
        {

        }
        public void AdminMayNotHaveUserPermissions(UserProfile userProfile)
        {
            //Admins may not create pins, services, reqeust services, or upload pictures
        }
        public void AdminOnlyViews(UserProfile userProfile)
        {
            //Admins have AdminView, UsageAnalysis, UserManagement, NotificationAnalysis,
            //ReputationAnalysis, EventsAnalysis, ServicesAnalysis,AccountPictures,and LitterMapAdmin view
        }
        public void ReputableIsCreated(UserProfile userProfile)
        {
            //Role is elevated when reputation is greater than 4.2 rating
        }
        public void ReputableLosesRole(UserProfile userProfile)
        {
            //Reputable Users are deelevated when reputation falls below 4.2. Upon next authentication
        }

        public void ReputableViews(UserProfile userProfile)
        {
            //Access to Event Creation and Service Request view
        }

        public void ServiceUserCreated(UserProfile userProfile)
        {
            //User elevated to ServiceUser when profile is updated with Service
        }

        public void ServiceUserViews(UserProfile userProfile)
        {
            //Service User can only see Service Creation view, Map view, Event view,
            //Notification view, and AccessProfile view
        }

        public void ServiceAccessProfile(UserProfile userProfile)
        {
            //Service users can deelivate themselves in AccessProfile view. Must happen upon request
        }

        public void ServiceUserReputation(UserProfile userProfile)
        {
            //Reputation wont changed a ServiceUser's role while a ServiceUser
        }


        public void RegularUserCreation(UserProfile userProfile)
        {
            //After creating a UserAccount, the user is no longer anonymous
        }
        public void RegularUserReputation(UserProfile userProfile)
        {
            //RegularUser only has a Regular identity when reputation is below 4.2
        }

        public void RegularUserViews(UserProfile userProfile)
        {
            //Regular users can access LitterMapView, JoinEventView, AlertView,
            //ReputationView, ProfileView, and PictureUploadView
        }
        
    }
}