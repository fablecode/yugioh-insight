using System;
using System.Runtime.Serialization;

namespace Cards.DatabaseMigration.Exceptions
{
    public sealed class DatabaseMigrationException : Exception
    {
        public DatabaseMigrationException()
        {
        }

        public DatabaseMigrationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public DatabaseMigrationException(string? message) : base(message)
        {
        }

        public DatabaseMigrationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}