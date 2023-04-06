using System.Diagnostics.CodeAnalysis;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.BLL.Util
{
    public class IdComparer : IEqualityComparer<PartnerSourceLink>
    {
        public bool Equals(PartnerSourceLink? x, PartnerSourceLink? y)
        {
            if(x == null || y == null)
            {
                return false;
            }

            return x.Id == y.Id;
        }

        public int GetHashCode([DisallowNull] PartnerSourceLink obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
