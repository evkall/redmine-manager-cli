using System;


namespace RedmineManagerCLI.ConnectionService
{
    [Serializable]
    public class ConnectionServiceBaseException : Exception
    {
        public ConnectionServiceBaseException() : base() { }
        public ConnectionServiceBaseException(string message) : base(message) { }
        public ConnectionServiceBaseException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class ConnectionServiceOptionsException : ConnectionServiceBaseException
    {
        public string Options { get; }

        public ConnectionServiceOptionsException() : base() { }
        public ConnectionServiceOptionsException(string message) : base(message) { }
        public ConnectionServiceOptionsException(string message, Exception inner) : base(message, inner) { }

        public ConnectionServiceOptionsException(string message, string options) : this(message)
        {
            Options = options;
        }
    }
    
    [Serializable]
    public class ConnectionServiceConnectionException : ConnectionServiceBaseException
    {
        public string Error { get; }

        public ConnectionServiceConnectionException() : base() { }
        public ConnectionServiceConnectionException(string message) : base(message) { }
        public ConnectionServiceConnectionException(string message, Exception inner) : base(message, inner) { }

        public ConnectionServiceConnectionException(string message, string error) : this(message)
        {
            Error = error;
        }
    }

}