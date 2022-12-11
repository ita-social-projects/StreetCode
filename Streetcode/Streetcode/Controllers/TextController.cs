
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class TextController : ControllerBase 
{
    private readonly ITextService _textService;
    public TextController(ITextService textService)
    {
        _textService = textService;
    }
    [HttpPost("createText")]
    public void CreateText() 
    {
        // TODO implement here
    }
    [HttpGet("getText")]
    public string GetText() 
    {
        return _textService.GetTextAsync();
    }
    [HttpPut("updateText")]
    public void UpdateText() 
    {
        // TODO implement here
    }
    [HttpDelete("deleteText")]
    public void DeleteText() 
    {
        // TODO implement here
    }
    [HttpGet("getNext")]
    public void GetNext() 
    {
        // TODO implement here
    }

}