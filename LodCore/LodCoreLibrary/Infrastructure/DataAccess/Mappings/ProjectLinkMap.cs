using LodCoreLibrary.Common;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LodCoreLibrary.Infrastructure.DataAccess.Mappings
{
    public class ProjectLinkMap : ClassMapping<ProjectLink>
    {
        public ProjectLinkMap()
        {
            Table("ProjectLinks");
            Lazy(true);
            Property(link => link.Name, mapper => mapper.Column("Name"));
            Property(link => link.Uri, mapper => mapper.Column("Uri"));
        }
    }
}
