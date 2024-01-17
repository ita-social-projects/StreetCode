﻿namespace Streetcode.BLL.DTO.Streetcode.CatalogItem;

public class CatalogItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Url { get; set; }
    public string? Alias { get; set; }
    public int ImageId { get; set; }
}