using System;

namespace Api.Exceptions.StorageExceptions
{
    public class StorageFileDeleteException : Exception
    {
        public string File { get; }

        public StorageFileDeleteException(string file, Exception innerException): base($"The file \"{file}\" cannot be deleted", innerException)
        {
            File = file;
        }
    }
}