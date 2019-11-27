using System;

namespace Api.Exceptions.StorageExceptions
{
    public class StorageFileCreateException : Exception
    {
        public string File { get; }
        
        public StorageFileCreateException(string file, Exception innerException): base($"The file \"{file}\" cannot be created", innerException)
        {
            File = file;
        }
    }
}