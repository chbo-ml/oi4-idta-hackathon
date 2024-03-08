using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hackathon.Database.Model
{
    public class ImportedShell
    {
        public int Id { get; set; }
        public string RemoteRegistryUrl { get; set; } = string.Empty;
        public string AasId { get; set; } = string.Empty;

    }
}