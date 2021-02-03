namespace Bemby.AccountModule.Domain.Entities
{
    public interface IAccountEntity : IEntityBase
    {
        string Email { get; }
        string MobileNumber { get; }
        string HashedPassword { get; }
    }
}