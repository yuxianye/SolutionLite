using Desktop.PublicControl.TreeGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.ModuleModule.Models
{
    public class TreeGridItem : TreeGridElement
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public object Value { get; set; }

        //public TreeGridItem(Guid id, string name, object value, bool hasChildren)
        //{
        //    // Initialize the item
        //    Id = id;
        //    Name = name;
        //    Value = value;
        //    HasChildren = hasChildren;
        //}
    }
}
