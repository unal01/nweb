namespace CoreBuilder.Services
{
    public interface ITenantService
    {
        int? GetCurrentTenantId();
        string GetCurrentDomain();
    }
}