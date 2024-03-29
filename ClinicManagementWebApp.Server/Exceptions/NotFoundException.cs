namespace ClinicManagementWebApp.Server.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("The request resource was not found") { }

        public NotFoundException(string message) : base(message) { }
    }
}
