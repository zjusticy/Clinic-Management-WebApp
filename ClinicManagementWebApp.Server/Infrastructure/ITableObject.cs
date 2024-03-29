namespace ClinicManagementWebApp.Server.Infrastructure
{
    public interface ITableObject<T> where T : IEquatable<T>
    {
        T Id { get; set; }
    }
}
