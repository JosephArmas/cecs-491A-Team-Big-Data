﻿using Azure.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Diagnostics;
using System.Security.Principal;
using TeamBigData.Utification.Manager;
using TeamBigData.Utification.SQLDataAccess.UsersDB;
using TeamBigData.Utification.SQLDataAccess.UserhashDB;
using TeamBigData.Utification.SQLDataAccess.LogsDB;
using TeamBigData.Utification.Logging;
using TeamBigData.Utification.Logging.Abstraction;
using TeamBigData.Utification.AccountServices;
using TeamBigData.Utification.Cryptography;
using TeamBigData.Utification.Models;
using TeamBigData.Utification.ErrorResponse;
using TeamBigData.Utification.SQLDataAccess;
using System.IO;


namespace TeamBigData.Utification.Manager
{
    //https://www.c-sharpcorner.com/article/working-with-c-sharp-streamreader/#:~:text=StreamReader%20code%20example.-,C%23%20StreamReader%20is%20used%20to%20read%20characters%20to%20a%20stream,%2C%20line%2C%20or%20all%20content.
    //code was inspired by looking at this webpage

    public class CsvReader
    {
        private readonly String usersString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security=True;Encrypt=False";
        private readonly String logString = "Server=.\\;Database=TeamBigData.Utification.Logs;User=AppUser; Password=t; TrustServerCertificate=True; Encrypt=True";
        private readonly String hashString = "Server=.\\;Database=TeamBigData.Utification.UserHash;Integrated Security=True;Encrypt=False";

        public RequestType request { get; private set; }
        public string email { get; private set; }
        public string? password { get; private set; } = null;

        //autogenerated by Visual Studio
        public CsvReader(RequestType request, string email, string password)
        {
            this.request = request;
            this.email = email;
            this.password = password;
        }
        public enum RequestType
        {
            CREATE,
            UPDATE,
            DELETE,
            DISABLE,
            ENABLE

        }
        public CsvReader()
        {
            RequestType request;
            String email;
            String password;
        }
        public async Task<Response> BulkFileUpload(String filename, UserProfile userProfile)
        {
            var tcs = new TaskCompletionSource<Response>();
            var response = new Response();
            CsvReaderModel csvModel = new CsvReaderModel(filename);
            List<CsvReader> requests = await ReadFileCsv(filename);
            long fileSize = new FileInfo(filename).Length;


            if (fileSize > 2147483648)
            {
                response.ErrorMessage = "File Size is too Big";
                response.IsSuccessful = false;
                return response;
            }
            response = CheckFor10kLines(filename).Result;
            if(!response.IsSuccessful) 
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "File has too many lines";
                tcs.SetResult(response);
                return response;
            }
            // Manual DI
            var userDAO = new UsersSqlDAO(usersString);
            var hashDAO = new UserhashSqlDAO(hashString);
            var logDAO = new LogsSqlDAO(logString);
            var reg = new AccountRegisterer(userDAO, userDAO);
            var hash = new UserhashServices(hashDAO);
            var auth = new AccountAuthentication(userDAO);
            var rec = new RecoveryServices(userDAO, userDAO, userDAO);
            ILogger logger = new Logger(new LogsSqlDAO(logString));
            var securityManager = new SecurityManager(reg, hash, auth, rec, logger);
            //SecurityManager securityManager = new SecurityManager();

            //Switch cases will handle bulk cases better than ifelse
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            foreach (var line in requests)
            {
                switch (line.request)
                {
                    case RequestType.CREATE:
                        var userhash = SecureHasher.HashString(line.email, "5j90EZYCbgfTMSU+CeSY++pQFo2p9CcI");
                        response = securityManager.RegisterUser(line.email, line.password, userhash).Result;
                        break;
                    case RequestType.DELETE:
                        line.password = null;
                        response = securityManager.DeleteProfile(line.email, userProfile).Result;
                        break;
                    case RequestType.UPDATE:
                        var hasher = new SecureHasher();
                        var connectionString = @"Server=.\;Database=TeamBigData.Utification.Users;Integrated Security = True;Encrypt=False";
                        var userDao = new SqlDAO(connectionString);
                        var digest = SecureHasher.HashString(line.email, line.password);
                        response = userDao.ChangePassword(line.email, digest).Result;
                        break;
                    case RequestType.ENABLE:
                        response = securityManager.EnableAccount(line.email, userProfile).Result;
                        break;
                    case RequestType.DISABLE:
                        response = securityManager.DisableAccount(line.email, userProfile).Result;
                        break;
                    
                }
            }
            stopwatch.Stop();
            if (stopwatch.ElapsedMilliseconds > 60000)
            {
                response.IsSuccessful = false;
                response.ErrorMessage = "Bulk UM operation was NOT successful";
            }
                tcs.SetResult(response);
            return response;

        }
        public async Task<List<CsvReader>> ReadFileCsv(string filename)
        {
            var csvReader = new List<CsvReader>();
            //https://stackoverflow.com/questions/1862982/c-sharp-filestream-optimal-buffer-size-for-writing-large-files
            //bufferSize of 4 megabytes
            var bufferSize = 4194304;
            /*FileStrea needs a path: filename
             *Mode: Open- opens a file
             *Access: Read access
             *Share: None - security 
             *BufferSize: file sent over packets instead a one big payload
             *Options: Asynchronus - want to read the file with async
             */
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize, FileOptions.Asynchronous))
            using (StreamReader reader = new StreamReader(fs))
            {
                //reads until end of line
                //ReadToEnd was reccommended during office hours
                var endLineReader = await reader.ReadToEndAsync();
                //foreach is easier to iterate through an array of values
                //Split at the end of each line so that ReadToEnd knows to stop
                foreach (var line in endLineReader.Split('\n'))
                {
                    //need to be able to store each value so that each individul operation can happen
                    //CSV-> Commma Separated Values
                    String[] csvValues = line.Split(',');
                    var request = csvValues[0];
                    string email = csvValues[1];
                    var password = csvValues[2];
                    if (request == "CREATE")
                    {
                        //csvReader.Add(new CsvReader { RequestType.Create, email, password });
                        var insert = new CsvReader(RequestType.CREATE, email, password);
                        csvReader.Add(insert);
                    }
                    else if (request == "UPDATE")
                    {
                        password = null;
                        var insert = new CsvReader(RequestType.UPDATE, email, password);
                        csvReader.Add(insert);
                    }
                    else if (request == "DELETE")
                    {
                        password = null;
                        var insert = new CsvReader(RequestType.DELETE, email, password);
                        csvReader.Add(insert);
                    }
                    else if (request == "ENABLE")
                    {
                        password = null;
                        var insert = new CsvReader(RequestType.ENABLE, email, password);
                        csvReader.Add(insert);
                    }
                    else if (request == "DISABLE")
                    {
                        password = null;
                        var insert = new CsvReader(RequestType.DISABLE, email, password);
                        csvReader.Add(insert);
                    }

                }
                return csvReader;
            }

        }
        //ReadAllLines might be better for bigger files so I wanted to see
        public async Task<List<CsvReader>> ReadFileCsvReadAll(string filename)
        {
            //var taskReader = TaskCompletionSource < List<CsvReader>();
            var readAll = await File.ReadAllLinesAsync(filename);
            var csvReader = new List<CsvReader>();
            using (StreamReader reader = new StreamReader(filename))
            {

                foreach (var line in readAll)
                {
                    //need to be able to store each value so that each individul operation can happen
                    //CSV-> Commma Separated Values
                    String[] csvValues = line.Split(',');
                    var request = csvValues[0];
                    var email = csvValues[1];
                    var password = csvValues[2];
                    if (request == "CREATE")
                    {
                        //csvReader.Add(new CsvReader { RequestType.Create, email, password });
                        var insert = new CsvReader(RequestType.CREATE, email, password);
                        csvReader.Add(insert);
                    }
                    else if (request == "UPDATE")
                    {
                        password = null;
                        var insert = new CsvReader(RequestType.UPDATE, email, password);
                        csvReader.Add(insert);
                    }
                    else if (request == "DELETE")
                    {
                        password = null;
                        var insert = new CsvReader(RequestType.DELETE, email, password);
                        csvReader.Add(insert);
                    }
                    else if (request == "ENABLE")
                    {
                        password = null;
                        var insert = new CsvReader(RequestType.ENABLE, email, password);
                        csvReader.Add(insert);
                    }
                    else if (request == "DISABLE")
                    {
                        password = null;
                        var insert = new CsvReader(RequestType.DISABLE, email, password);
                        csvReader.Add(insert);
                    }

                }
                return csvReader;
            }
        }
        public async Task<Response> CheckFor10kLines(string filename)
        {
            //var tcs = new TaskCompletionSource<Response>();
            var readAll = await File.ReadAllLinesAsync(filename);
            var response = new Response();
            int lines = 0;
            //https://stackoverflow.com/questions/1862982/c-sharp-filestream-optimal-buffer-size-for-writing-large-files
            //resource looked at when learning about buffer sizes
            /* decided to move this check to the CsvReader*/
            //var bufferSize = 2 * *22;
            //using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Write, FileShare.None, bufferSize))
            //{
                using (StreamReader reader = new StreamReader(filename))
                {
                    foreach (var line in readAll)
                    {
                        lines++;
                        if (lines > 10000)
                        {
                            response.IsSuccessful = false;
                            response.ErrorMessage = "File has too many lines";
                            return response;
                        }
                    }
                }
           // }
            response.IsSuccessful = true;
            //tcs.SetResult(response);
            return response;
        }
    }

    /*
     * Used links:
     * https://www.tutorialspoint.com/how-to-read-a-csv-file-and-store-the-values-into-an-array-in-chash#:~:text=In%20C%23%2C%20StreamReader%20class%20is,used%20to%20read%20its%20contents.
     * https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum
     * https://www.w3schools.com/cs/cs_foreach_loop.php
     * https://www.w3schools.com/cs/cs_arrays.php
     * https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.add?view=net-7.0
     * https://www.geeksforgeeks.org/switch-vs-else/
     */
}
