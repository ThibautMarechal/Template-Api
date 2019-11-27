using System;

namespace Api.Exceptions.StorageExceptions
{
    public class StorageFileNotFoundException : Exception
    {
        private readonly string _file;

        public StorageFileNotFoundException(string file): base($"The file \"{file}\" doesnt exist")
        {
            _file = file;
        }
    }
}