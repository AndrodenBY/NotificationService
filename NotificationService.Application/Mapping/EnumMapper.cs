using SubsTrackerEnum = SubsTracker.Messaging.Enums;
using DomainMemberRoleEnum = NotificationService.Domain.Enums;

namespace NotificationService.Application.Mapping;

public class EnumMapper
{
    public static DomainMemberRoleEnum.MemberRole? MapMemberRole(SubsTrackerEnum.MemberRole originalEnum)
    {
        return originalEnum switch
        {
            SubsTrackerEnum.MemberRole.Admin => DomainMemberRoleEnum.MemberRole.Admin,
            SubsTrackerEnum.MemberRole.Moderator => DomainMemberRoleEnum.MemberRole.Moderator,
            SubsTrackerEnum.MemberRole.Participant => DomainMemberRoleEnum.MemberRole.Participant,
            _ => null
        };
    }
}
