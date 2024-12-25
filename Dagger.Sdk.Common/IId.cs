namespace Dagger;

public interface IId<TId> where TId : Scalar
{
    Task<TId> IdAsync();
}