﻿namespace GameZoneApp.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        [MaxLength(250)]
        public string? Name { get; set; }
    }
}
