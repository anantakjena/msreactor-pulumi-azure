using System;
using System.Collections.Generic;

namespace msreactor.common
{
    public class Config
    {
        public string Environment { get; set; }
        public List<Service> Services { get; set; }
    }

    public class Service
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Tier { get; set; }

        public string Kind { get; set; }

        public Sku Sku { get; set; }

        public Dictionary<string, string> Settings { get; set; }
    }

    public class Sku
    {
        public string Tier { get; set; }

        public string Size { get; set; }
    }
}
