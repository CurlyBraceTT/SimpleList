using System.Collections.Generic;

namespace SimpleList.Models
{
    public class BaseListItemComparer : IEqualityComparer<ListItem>
    {
        public bool IncludeId { get; set; }

        public BaseListItemComparer(bool includeId = true)
        {
            IncludeId = includeId;
        }

        public bool Equals(ListItem x, ListItem y)
        {
            return (!IncludeId || x.Id == y.Id ) && x.Done == y.Done && x.Description == y.Description;
        }

        public int GetHashCode(ListItem obj)
        {
            if(IncludeId)
            {
                return new { obj.Id, obj.Description, obj.Done }.GetHashCode();
            }
            else
            {
                return new { obj.Description, obj.Done }.GetHashCode();
            }
        }
    }
}
