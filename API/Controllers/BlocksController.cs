using System;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class BlocksController(IUnitOfWork unitOfWork) : BaseApiController
{
    [Authorize(Policy = "RequireVIPRole")]
    [HttpPost("{targetMemberId}")]
    public async Task<ActionResult> BlockWithReason(string targetMemberId, [FromBody] BlockRequestDto blockRequestDto)
    {
        var sourceMemberId = User.GetMemberId();

        if (sourceMemberId == targetMemberId) return BadRequest("You cannot block yourself");

        var block = new MemberBlock
        {
            SourceMemberId = sourceMemberId,
            TargetMemberId = targetMemberId,
            Reason = blockRequestDto.Reason
        };

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        unitOfWork.BlocksRepository.AddBlock(block);

        if (await unitOfWork.Complete()) return Ok();

        return BadRequest("Failed to block user");
    }

    [Authorize(Policy = "RequireVIPRole")]
    [HttpGet]
    public async Task<ActionResult<PaginatedResult<Member>>> GetMemberBlocks(
    [FromQuery] BlocksParams blocksParams
        )
    {
        blocksParams.MemeberId = User.GetMemberId();
        var members = await unitOfWork.BlocksRepository.GetMemberBlocks(blocksParams);

        return Ok(members);
    }

    [Authorize(Policy = "RequireVIPRole")]
    [HttpDelete("{targetMemberId}")]
    public async Task<ActionResult> Unblock(string TargetMemberId)
    {
        var SourceMemberId = User.GetMemberId();

        var block = await unitOfWork.BlocksRepository.GetMemberBlock(SourceMemberId, TargetMemberId);
        if (block == null) return NotFound("Block record not found");
        unitOfWork.BlocksRepository.DeleteBlock(block);
        if (await unitOfWork.Complete()) return Ok();

        return BadRequest("Failed to unblock user");
    }

    [Authorize(Policy = "RequireVIPRole")]
    [HttpPut("{targetMemberId}")]
    public async Task<ActionResult> UpdateReason(string targetMemberId, BlockRequestDto blockRequestDto)
    {
        var memberId = User.GetMemberId();

        var block = await unitOfWork.BlocksRepository.GetMemberBlock(memberId, targetMemberId);

        if (block == null) return BadRequest("Could not get block");

        block.Reason = blockRequestDto.Reason;

        unitOfWork.BlocksRepository.UpdateBlock(block);

        if (await unitOfWork.Complete()) return NoContent();

        return BadRequest("Failed to update reason");
    }

}
