using System;
using Microsoft.EntityFrameworkCore;
using API.Entities;
using API.Helpers;
using API.Interfaces;

namespace API.Data;

public class BlocksRepository(AppDbContext context) : IBlocksRepository
{
    public void AddBlock(MemberBlock block)
    {
        context.Blocks.Add(block);
    }

    public void DeleteBlock(MemberBlock block)
    {
        context.Blocks.Remove(block);
    }

    public async Task<IReadOnlyList<string>> GetCurrentMemberBlockIds(string memberId)
    {
        return await context.Blocks
            .Where(x => x.SourceMemberId == memberId)
            .Select(x => x.TargetMemberId)
            .ToListAsync();
    }

    public async Task<MemberBlock?> GetMemberBlock(string sourceMemberId, string targetMemberId)
    {
        return await context.Blocks.FindAsync(sourceMemberId, targetMemberId);
    }

    public async Task<PaginatedResult<Member>> GetMemberBlocks(BlocksParams blocksParams)
    {
        var query = context.Blocks.AsQueryable();
        IQueryable<Member> result;

        result = query
            .Where(x => x.SourceMemberId == blocksParams.MemeberId)
            .Select(x => x.TargetMember);

        return await PaginationHelper.CreateAsync(result, blocksParams.PageNumber, blocksParams.PageSize);
    }

    public void UpdateBlock(MemberBlock block)
    {
        context.Blocks.Update(block); 
    }
}
