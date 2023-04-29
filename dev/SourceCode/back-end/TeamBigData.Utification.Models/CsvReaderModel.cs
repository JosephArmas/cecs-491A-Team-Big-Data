﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using TeamBigData.Utification.Models.Abstraction;

namespace TeamBigData.Utification.Models
{
    public class CsvReaderModel //: IMyIFormFile
    {
        //autogenerated by Visual Studio
        public RequestType request { get; private set; }
        public string email { get; private set; }
        public string password { get; private set; } = null;

        public string ContentType => throw new NotImplementedException();

        public string ContentDisposition => throw new NotImplementedException();

        //public IHeaderDictionary Headers => throw new NotImplementedException();

        public long Length { get; private set; }

        public string Name => throw new NotImplementedException();

        public string FileName => throw new NotImplementedException();

        //autogenerated by Visual Studio
        public CsvReaderModel(RequestType request, string email, string password)
        {
            this.request = request;
            this.email = email;
            this.password = password;
        }
        public CsvReaderModel(String filename)
        {
            Length = filename.Length;
        }
        public enum RequestType
        {
            CREATE,
            UPDATE,
            DELETE,
            DISABLE,
            ENABLE

        }
        /*
        public CsvReaderModel()
        {
            RequestType _request;
            String _email;
            String _password;
        }
        */
        //autogenerated by Visual Studio
        public Stream OpenReadStream()
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Stream target)
        {
            throw new NotImplementedException();
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
