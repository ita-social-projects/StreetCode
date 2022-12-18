
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.Interfaces.AdditionalContent;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;
    public TagController(ITagService tagService)
    {
        _tagService = tagService;
    }
    [HttpPost("createTag")]
    public void CreateTag()
    {
        // TODO implement here
    }
    [HttpGet("getTag")]
    public void GetTag() 
    {
        // TODO implement here
    }
    [HttpPut("updateTag")]
    public void UpdateTag() 
    {
        // TODO implement here
    }
    [HttpDelete("deleteTag")]
    public void DeleteTag()
    {
        // TODO implement here
    }
    [HttpGet("getTagByName")]
    public string GetTagByName()
    {
        return _tagService.GetTagByNameAsync();
    }
    [HttpGet("getTagsByStreetcode")]
    public void GetTagsByStreetcode() 
    {
        // TODO implement here
    }

}