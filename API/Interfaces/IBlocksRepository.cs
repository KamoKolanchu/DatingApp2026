using System;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;

public interface IBlocksRepository
{
    Task<MemberBlock?> GetMemberBlock(string sourceMemberId, string targetMemberId);
    Task<PaginatedResult<Member>> GetMemberBlocks(BlocksParams blocksParams);
    Task<IReadOnlyList<string>> GetCurrentMemberBlockIds(string memberId);
    void UpdateBlock(MemberBlock block);
    void DeleteBlock(MemberBlock block);
    void AddBlock(MemberBlock block);
}
