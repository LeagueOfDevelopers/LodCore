﻿using LodCoreLibraryOld.Domain.UserManagement;
using NHibernate.Mapping.ByCode.Conformist;

namespace LodCoreLibraryOld.Infrastructure.DataAccess.Mappings
{
    internal class MailValidationRequestMap : ClassMapping<MailValidationRequest>
    {
        public MailValidationRequestMap()
        {
            Id(model => model.Token, mapper => mapper.Column("VerificationToken"));
            Property(model => model.UserId, mapper => mapper.Column("UserId"));
        }
    }
}