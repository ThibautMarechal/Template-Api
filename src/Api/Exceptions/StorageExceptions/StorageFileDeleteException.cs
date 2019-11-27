using System;

namespace Api.Exceptions.StorageExceptions
{
    public class StorageFileDeleteException : Exception
    {
        private readonly string _file;
        private readonly Exception _innerException;

        public StorageFileDeleteException(string file, Exception innerException): base($"The file \"{file}\" cannot be deleted")
        {
            _file = file;
            _innerException = innerException;
        }
    }
}