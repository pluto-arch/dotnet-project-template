namespace PlutoNetCoreTemplate.Domain.Aggregates.DemoTree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Folder Parent { get; set; }
        public int? ParentId { get; set; }
        public ICollection<Folder> SubFolders { get; } = new List<Folder>();
    }
}
