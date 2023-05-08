using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.SQLDataAccess.UserhashDB;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.SQLDataAccess;
using TeamBigData.Utification.DeletionService;
using TeamBigData.Utification.SQLDataAccess.FeaturesDB;

namespace TeamBigData.Utification.UserManagementTests
{
    [TestClass]
    public class UserManagementIntegrationTests
    {
        private readonly String usersString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
        private readonly String logString = "Server=.\\;Database=TeamBigData.Utification.Logs;User=AppUser; Password=t; TrustServerCertificate=True; Encrypt=True";
        private readonly String hashString = "Server=.\\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False";
        private readonly String featureString = @"Server=.\;Database=TeamBigData.Utification.Features;Integrated Security=True;Encrypt=False";
        private readonly SecurityManager securityManager;

        /*[TestMethod]
        public void AdminRestrictedViewAccess()
        {
            //Admin is only allowed to acess UserManagementView
            //Arrange
            var sysUnderTestAnonymous = new UserProfile(new GenericIdentity("username", "Anonymous User"));
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            var sysUnderTestRegular = new UserProfile(new GenericIdentity("username", "Regular User"));
            IView view = new UserManagementView();
            String userHash = "Testing";
            //Act
            Console.SetIn(new StringReader("0"));
            var logResultAnonymous = view.DisplayMenu(ref sysUnderTestAnonymous, ref userHash);
            Console.SetIn(new StringReader("0"));
            var logResultAdmin = view.DisplayMenu(ref sysUnderTestAdmin, ref userHash);
            Console.SetIn(new StringReader("0"));
            var logResultRegular = view.DisplayMenu(ref sysUnderTestRegular, ref userHash);
            //Assert
            bool pass = false;
            if (!logResultAnonymous.isSuccessful && logResultAnonymous.errorMessage == "")
                pass = true;
            else
                pass = false;
            Console.WriteLine("Anonymous User wants to see UserManagement View: " + pass);
            Assert.IsFalse(pass);
            if (!logResultAdmin.isSuccessful && logResultAdmin.errorMessage == "")
                pass = true;
            else
                pass = false;
            Console.WriteLine("Admin User wants to see UserManagement View: " + pass);
            Assert.IsTrue(pass);
            if (!logResultRegular.isSuccessful && logResultRegular.errorMessage == "")
                pass = true;
            else
                pass = false;
            Console.WriteLine("Regular User wants to see UserManagement View: " + pass);
            Assert.IsFalse(pass);
        }
        
        [TestMethod]
        public async Task CreateWithinFiveSeconds()
        {
            //Testing ability to have a task perform under 5 seconds
            //Arrange
            // Manual Dependencies
            var userDAO = new UsersSqlDAO(usersString);
            var pinsDAO = new PinsSqlDAO(featureString);
            var hashDAO = new UserhashSqlDAO(hashString);
            var logDAO = new LogsSqlDAO(logString);
            var reg = new AccountRegisterer(userDAO);
            var hash = new UserhashServices(hashDAO);
            var auth = new AccountAuthentication(userDAO);
            var rec = new RecoveryServices(userDAO);
            var del = new AccDeletionService(userDAO, pinsDAO, hashDAO);
            ILogger logger = new Logger(new LogsSqlDAO(logString));
            var securityManager = new SecurityManager(reg, hash, auth, rec, logger, del);

            Response response = new Response();
            var userAccount = new UserAccount();
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            String userPassword = "Password";
            var stopwatch = new Stopwatch();
            var expected = 5000;
            string email = "";
            var userhash = SecureHasher.HashString(email, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");

            //Act
            stopwatch.Start();
            response = await securityManager.RegisterUser(email, userPassword, userhash);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert
            if (actual < expected && (response.IsSuccessful || response.ErrorMessage.Contains("PRIMARY KEY") || response.ErrorMessage.Contains("UNIQUE KEY")))
            {
                Console.WriteLine("UM was successful");
                Assert.IsTrue(true);
            }
            else
            {
                Assert.IsTrue(false);
            }
        }
        /*
        [TestMethod]
        public async Task DeleteWithinFiveSeconds()
        {
            //Testing ability to have a task perform under 5 seconds
            //Arrange
            Response response = new Response();
            var userAccount = new UserAccount();
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            String userPassword = "Password";
            var stopwatch = new Stopwatch();
            var expected = 5000;
            string email = "";

            var userhash = SecureHasher.HashString(email, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");
            var madeUser = securityManager.RegisterUser(email, userPassword, userhash);

            //Act
            stopwatch.Start();
            response = await securityManager.DeleteProfile(email, sysUnderTestAdmin);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert

            if (actual < expected && response.IsSuccessful)
            {
                Console.WriteLine("UM was successful");
                Assert.IsTrue(true);
            }
            else
                Assert.IsTrue(false);
        }
        */

        [TestMethod]
        public async Task DisableWithinFiveSeconds()
        {
            //Testing ability to have a task perform under 5 seconds
            //Arrange
            // Manual Dependencies
            var userDAO = new UsersSqlDAO(usersString);
            var pinsDAO = new PinsSqlDAO(featureString);
            var hashDAO = new UserhashSqlDAO(hashString);
            var logDAO = new LogsSqlDAO(logString);
            var reg = new AccountRegisterer(userDAO);
            var hash = new UserhashServices(hashDAO);
            var auth = new AccountAuthentication(userDAO);
            var rec = new RecoveryServices(userDAO);
            var del = new AccDeletionService(userDAO, pinsDAO, hashDAO);
            ILogger logger = new Logger(new LogsSqlDAO(logString));
            var securityManager = new SecurityManager(reg, hash, auth, rec, logger, del);

            Response response = new Response();
            var userAccount = new UserAccount();
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            String userPassword = "Password";
            var stopwatch = new Stopwatch();
            var expected = 5000;
            string email = "disabledUser1@yahoo.com";

            var userhash = SecureHasher.HashString(email, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");
            var madeUser = securityManager.RegisterUser(email, userPassword, userhash);

            //Act
            stopwatch.Start();
            response = await securityManager.DisableAccount(email, sysUnderTestAdmin);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert

            if (actual < expected && response.IsSuccessful)
            {
                Console.WriteLine("UM was successful");
                Assert.IsTrue(true);
            }
            else
                Assert.IsTrue(false);
        }
        [TestMethod]
        public async Task EnableWithinFiveSeconds()
        {
            //Testing ability to have a task perform under 5 seconds
            //Arrange
            // Manual Dependencies
            var userDAO = new UsersSqlDAO(usersString);
            var pinsDAO = new PinsSqlDAO(featureString);
            var hashDAO = new UserhashSqlDAO(hashString);
            var logDAO = new LogsSqlDAO(logString);
            var reg = new AccountRegisterer(userDAO);
            var hash = new UserhashServices(hashDAO);
            var auth = new AccountAuthentication(userDAO);
            var rec = new RecoveryServices(userDAO);
            var del = new AccDeletionService(userDAO, pinsDAO, hashDAO);
            ILogger logger = new Logger(new LogsSqlDAO(logString));
            var securityManager = new SecurityManager(reg, hash, auth, rec, logger, del);

            Response response = new Response();
            var userAccount = new UserAccount();
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            String userPassword = "Password";
            var stopwatch = new Stopwatch();
            var expected = 5000;
            string email = "EnabledUser@yahoo.com";

            var userhash = SecureHasher.HashString(email, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");
            var madeUser = securityManager.RegisterUser(email, userPassword, userhash);
            var disabledUser = await securityManager.DisableAccount(email, sysUnderTestAdmin);

            //Act
            stopwatch.Start();
            response = await securityManager.EnableAccount(email, sysUnderTestAdmin);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert

            if (actual < expected && response.IsSuccessful)
            {
                Console.WriteLine("UM was successful");
                Assert.IsTrue(true);
            }
            else
                Assert.IsTrue(false);
        }
        //How I found to create a file for test
        //https://learn.microsoft.com/en-us/dotnet/api/system.io.streamwriter?view=net-7.0
        /*[TestMethod]
        public void BulkUploadSize()
        {
            //Testing ability to not handle large files
            //Arrange
            Response response = new Response();
            var userAccount = new UserAccount();
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            String userPassword = "Password";
            var stopwatch = new Stopwatch();
            var expected = 5000;
           /* try
            {
                /*ran into errors with getting directory path*/
        /*string directoryPath = @"C:\MyDir";
        /*Directory will be created if not existing
        Directory.CreateDirectory(directoryPath);
        //DirectoryInfo di = new DirectoryInfo(@"c:\MyDir");
        DirectoryInfo di = new DirectoryInfo(directoryPath);
        string filePath = Path.Combine(directoryPath, "testSize.csv");
        //using (StreamWriter sw = new StreamWriter("testSize.csv"))
        using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            //excel has  a limit of 1million rows. but only makes a 40k kb file
            //so it needs to be 50 times that to reach 2GB (50 was barely not enough)
            //doing this will take like 4~5min
            for (int line = 0; line < 51500000; line++)
            {
                //the $ allows me to insert info into the string
                string data = $"CREATE,testSize{line}@yahoo.com,password";
                sw.WriteLine(data);
                /* if (line == 999999)
                 {
                     Console.WriteLine(line);
                     break;
                 }
                sw.Flush();
            }
        }
        var filenameGet = "testSize.csv";
        var filename = @"C:\MyDir\" + filenameGet + "";
        CsvReader csvReader = new CsvReader();

        //Act
        stopwatch.Start();
        response = csvReader.BulkFileUpload(filename, sysUnderTestAdmin).Result;
        stopwatch.Stop();
    }
    catch (OutOfMemoryException ex) 
    {
        Console.WriteLine("Bulk UM was too big");
        Assert.IsTrue(true);
    }
    var actual = stopwatch.ElapsedMilliseconds;
    //Assert

    if (actual < expected && response.isSuccessful==false)
    {
        Console.WriteLine("Bulk UM was successful");
        Assert.IsTrue(true);
    }
    else
        Assert.IsTrue(false); 
}*/

                /*ran into errors with getting directory path
                string directoryPath = Path.Combine(Environment.CurrentDirectory, @"C:\MyDir");
                /*Directory will be created if not existing
                Directory.CreateDirectory(directoryPath);
                //DirectoryInfo di = new DirectoryInfo(@"c:\MyDir");
                DirectoryInfo di = new DirectoryInfo(directoryPath);
                string filePath = Path.Combine(directoryPath, "testSize.csv");
                //using (StreamWriter sw = new StreamWriter("testSize.csv"))
                using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    int countLines = 0;
                    int amount = 51500000;
                    //excel has  a limit of 1million rows. but only makes a 40k kb file
                    //so it needs to be 50 times that to reach 2GB (50 was barely not enough)
                    //doing this will take like 4~5min
                    for (int line = 0; line < amount; line++)
                    {
                        //the $ allows me to insert info into the string
                        string data = $"CREATE,testSize{line}@yahoo.com,password";
                        if (countLines < amount - 1)
                        {


                            //sw.Write(data);
                            sw.WriteLine(data);


                        }
                        else
                        {
                            //sw.WriteLine();
                            sw.Write(data);
                            //sw.WriteLine();
                        }
                        countLines++;
                        /* if (line == 999999)
                         {
                             Console.WriteLine(line);
                             break;
                         }
                        sw.Flush();
                    }
                }*/
               /* var filenameGet = "testSize.csv";
                var filename = @"C:\MyDir\" + filenameGet + "";
                CsvReader csvReader = new CsvReader();

                //Act
                stopwatch.Start();
                response = csvReader.BulkFileUpload(filename, sysUnderTestAdmin).Result;
                stopwatch.Stop();*/
            //}
           /* catch (OutOfMemoryException ex) 
            {
                Console.WriteLine("Bulk UM was too big");
                Assert.IsTrue(true);
            }*//*
            var actual = stopwatch.ElapsedMilliseconds;
            //Assert

            if (actual < expected && response.isSuccessful==false)
            {
                Console.WriteLine("Bulk UM was successful");
                Assert.IsTrue(true);
            }
            else
                Assert.IsTrue(false); 
        }*/

        /*[TestMethod]
        public async Task BulkUploadLength()
        {
            //Testing ability to not handle large files
            //Arrange

            Response response = new Response();
            var userAccount = new UserAccount();
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            String userPassword = "Password";
            var stopwatch = new Stopwatch();
            var expected = 5000;
            string email = "";
            SecurityManager securityManager = new SecurityManager();
            // Ran into errors with getting directory path
            string directoryPath = @"C:\MyDir";
            //Directory will be created if not existing
            Directory.CreateDirectory(directoryPath);
            //DirectoryInfo di = new DirectoryInfo(@"c:\MyDir");
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            string filePath = Path.Combine(directoryPath, "testLength.csv");
            //using (StreamWriter sw = new StreamWriter("testSize.csv"))
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                int countLines = 0;
                int amount = 11000;
                for (int line = 0; line < amount; line++)
                {

                    //the $ allows me to insert info into the string
                    string data = $"CREATE,testLength{line}@yahoo.com,password";
                    //was encountering an error with emptyspace
                    if (countLines < amount - 1)
                    {


                        //sw.Write(data);
                        sw.WriteLine(data);


                    }
                    else
                    {
                        //sw.WriteLine();
                        sw.Write(data);
                        //sw.WriteLine();
                    }
                    countLines++;
                    //the $ allows me to insert info into the string
                    /* if (line == 999999)
                     {
                         Console.WriteLine(line);
                         break;
                     }
                    sw.Flush();

                }
            }
            var filenameGet = "testLength.csv";
            var filename = @"C:\MyDir\" + filenameGet + "";
            CsvReader csvReader = new CsvReader();

            //Act
            stopwatch.Start();
            response = await csvReader.BulkFileUpload(filename, sysUnderTestAdmin);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;


            //Assert

            if (actual < expected && !response.IsSuccessful || response.ErrorMessage.Contains("Key"))
            {
                //Console.WriteLine("Bulk UM was successful");
                Assert.IsTrue(true);
                //securityManager.DeleteProfile(email, sysUnderTestAdmin);

            }
            else
                Assert.IsTrue(false);
        }
        [TestMethod]
        public async Task BulkUploadTest()
        {
            //Testing ability to not handle large files
            //Arrange

            Response response = new Response();
            var userAccount = new UserAccount();
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            String userPassword = "Password";
            var stopwatch = new Stopwatch();
            var expected = 5000;
            string email = "";
            // Ran into errors with getting directory path
            string directoryPath = @"C:\MyDir";
            // Directory will be created if not existing
            Directory.CreateDirectory(directoryPath);
            //DirectoryInfo di = new DirectoryInfo(@"c:\MyDir");
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            string filePath = Path.Combine(directoryPath, "testWork.csv");
            //using (StreamWriter sw = new StreamWriter("testSize.csv"))
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                int countLines = 0;
                int amount = 3;
                for (int line = 0; line < amount; line++)
                {

                    //the $ allows me to insert info into the string
                    string data = $"CREATE,testWork{line}@yahoo.com,password";
                    //was encountering an error with emptyspace
                    if (countLines < amount - 1)
                    {


                        //sw.Write(data);
                        sw.WriteLine(data);


                    }
                    else
                    {
                        //sw.WriteLine();
                        sw.Write(data);
                    }
                    countLines++;
                    //the $ allows me to insert info into the string    */
                    /* if (line == 999999)
                     {
                         Console.WriteLine(line);
                         break;
                     }*/
                    /*sw.Flush();

                }

            }
            var filenameGet = "testWork.csv";
            var filename = @"C:\MyDir\" + filenameGet + "";
            CsvReader csvReader = new CsvReader();

            //Act
            stopwatch.Start();
            response = await csvReader.BulkFileUpload(filename, sysUnderTestAdmin);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;

            Console.WriteLine(response.ErrorMessage);

            //Assert

            if (actual < expected && (response.IsSuccessful || response.ErrorMessage.Contains("PRIMARY KEY") || response.ErrorMessage.Contains("UNIQUE")))
            {
                Console.WriteLine("Bulk UM was successful");
                Assert.IsTrue(true);
                //securityManager.DeleteProfile("testWork0@yahoo.com", sysUnderTestAdmin);
                //securityManager.DeleteProfile("testWork1@yahoo.com", sysUnderTestAdmin);
                //securityManager.DeleteProfile("testWork2@yahoo.com", sysUnderTestAdmin);
            }
            else
                Assert.IsTrue(false);
        }
        [TestMethod]
        public async Task BulkUploadCreateAndDelete()
        {
            //Testing ability to not handle large files
            //Arrange
            Response response = new Response();
            var userAccount = new UserAccount();
            var sysUnderTestAdmin = new UserProfile(new GenericIdentity("username", "Admin User"));
            String userPassword = "Password";
            var stopwatch = new Stopwatch();
            var expected = 5000;
            string email = "";
            SecurityManager securityManager = new SecurityManager();
            // Ran into errors with getting directory path
            string directoryPath = @"C:\MyDir";
            // Directory will be created if not existing
            Directory.CreateDirectory(directoryPath);
            //DirectoryInfo di = new DirectoryInfo(@"c:\MyDir");
            DirectoryInfo di = new DirectoryInfo(directoryPath);
            string filePath = Path.Combine(directoryPath, "testCND.csv");
            //using (StreamWriter sw = new StreamWriter("testSize.csv"))
            using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                int countLines = 0;
                int amount = 3;
                for (int line = 0; line < amount; line++)
                {

                    //the $ allows me to insert info into the string
                    string create = $"CREATE,testCND{line}@yahoo.com,password";
                    string delete = $"DELETE,testCND{line - 1}@yahoo.com,password";
                    //was encountering an error with emptyspace
                    if (countLines < amount - 1)
                    {


                        //sw.Write(data);
                        sw.WriteLine(create);


                    }
                    else
                    {
                        //sw.WriteLine();
                        sw.Write(delete);
                    }
                    countLines++;
                    //the $ allows me to insert info into the string    */
                    /* if (line == 999999)
                     {
                         Console.WriteLine(line);
                         break;
                     }*/
                    /*sw.Flush();

                }

            }
            var filenameGet = "testCND.csv";
            var filename = @"C:\MyDir\" + filenameGet + "";
            CsvReader csvReader = new CsvReader();

            //Act
            stopwatch.Start();
            response = await csvReader.BulkFileUpload(filename, sysUnderTestAdmin);
            stopwatch.Stop();
            var actual = stopwatch.ElapsedMilliseconds;

            //Assert

            if (actual < expected && response.IsSuccessful)
            {
                Console.WriteLine("Bulk UM was successful");
                Assert.IsTrue(true);
            }
            else
                Assert.IsTrue(false);
        }*/
    }
}

