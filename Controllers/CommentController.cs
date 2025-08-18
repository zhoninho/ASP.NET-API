﻿using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CommentController: ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var comments = await _commentRepository.GetAllAsync();

        var commentDto = comments.Select(c => c.ToCommentDto()).ToList();
        
        return Ok(commentDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment == null)
        {
            return NotFound();
        }
        
        return Ok(comment.ToCommentDto());
    }

    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto commentDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _stockRepository.StockExists(stockId))
        {
            return BadRequest("Stock does not exist");
        }

        var commentModel = commentDto.ToCommentFromCreate(stockId);
        await _commentRepository.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentRequestDto updateCommentDto)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentRepository.UpdateAsync(id, updateCommentDto.ToCommentFromUpdate());

        if (comment == null)
        {
            return NotFound("Comment not found");
        }
        
        return Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var commentModel = await _commentRepository.DeleteAsync(id);

        if (commentModel == null)
        {
            return NotFound("Comment does not exist");
        }

        return NoContent();
    }
    
}