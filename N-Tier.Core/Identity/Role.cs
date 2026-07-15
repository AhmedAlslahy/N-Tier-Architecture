using N_Tier.Core.Common;
using System.Xml.Linq;

namespace N_Tier.Core.Identity;

public class Role : BaseEntity<int>
{
    private string _name = string.Empty;

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            NormalizedName = value.ToUpperInvariant();
        }
    }

    public string NormalizedName { get; private set; } = string.Empty;

    //Relations
    public ICollection<UserRole> UserRoles { get; set; } = [];
}