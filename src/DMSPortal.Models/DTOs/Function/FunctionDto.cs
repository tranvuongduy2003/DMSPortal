﻿namespace DMSPortal.Models.DTOs.Function;

public class FunctionDto
{
    public string Id { get; set; }

    public string Name { get; set; }

    public string Url { get; set; }

    public int SortOrder { get; set; }

    public string ParentId { get; set; }
}