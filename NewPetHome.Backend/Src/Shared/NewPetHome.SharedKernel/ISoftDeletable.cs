namespace NewPetHome.SharedKernel;

public interface ISoftDeletable
{
    void SoftDelete();
    void Restore();
}