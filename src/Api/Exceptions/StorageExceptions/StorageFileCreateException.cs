using System;

namespace Api.Exceptions.StorageExceptions
{
    public class StorageFileCreateException : Exception
    {
        private readonly string _file;
        private readonly Exception _innerException;
        
        public StorageFileCreateException(string file, Exception innerException): base($"The file \"{file}\" cannot be created")
        {
            _file = file;
            _innerException = innerException;
        }
    }
}