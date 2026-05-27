using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Extensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository(AppDbContext context) : ILikesRepository
{
    public void AddLike(MemberLike like)
    {
        context.Likes.Add(like);
    }

    public void DeleteLike(MemberLike like)
    {
        context.Likes.Remove(like);
    }

    public async Task<IReadOnlyList<string>> GetCurrentMemberLikeIds(string memberId)
    {
        return await context.Likes
            .Where(x => x.SourceMemberId == memberId)
            .Select(x => x.TargetMemberId)
            .ToListAsync();
    }

    public async Task<MemberLike?> GetMemberLike(string sourceMemberId, string targetMemberId)
    {
        return await context.Likes.FindAsync(sourceMemberId, targetMemberId);
    }

    public async Task<PaginatedResult<Member>> GetMemberLikes(LikesParams likesParams)
    {
        var query = context.Likes.AsQueryable();
        IQueryable<Member> result;

        switch (likesParams.Predicate)
        {
            case "liked":
                result = query
                    .Where(x => x.SourceMemberId == likesParams.MemeberId)
                    .Select(x => x.TargetMember)
                    .ExcludeBlocked(context, likesParams.MemeberId);
                break;

            case "likedBy":
                result = query
                    .Where(x => x.TargetMemberId == likesParams.MemeberId)
                    .Select(x => x.SourceMember)
                    .ExcludeBlocked(context, likesParams.MemeberId);
                break;

            default: // mutual
                var likeIds = await GetCurrentMemberLikeIds(likesParams.MemeberId);

                result = query
                    .Where(x => x.TargetMemberId == likesParams.MemeberId
                        && likeIds.Contains(x.SourceMemberId))
                    .Select(x => x.SourceMember)
                    .ExcludeBlocked(context, likesParams.MemeberId);
                break;
        }

        return await PaginationHelper.CreateAsync(result, likesParams.PageNumber, likesParams.PageSize);  
    }
}


