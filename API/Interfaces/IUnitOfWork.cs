namespace API.Interfaces;

public interface IUnitOfWork
{
    IMemberRepository MemberRepository { get; }
    IMessageRepository MessageRepository { get; }
    IPhotoRepository PhotoRepository { get; }
    ILikesRepository LikesRepository { get; }
    IBlocksRepository BlocksRepository { get; }
    Task<bool> Complete();
    bool HasChanges();
}