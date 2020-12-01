﻿using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Agent
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ContactInformation { get; set; }
        public List<Agency> Agencies { get; } = new List<Agency>();
    }
}
