using System.Collections.Generic;
using contestDomain;
using Type = contestDomain.enums.Type;
using AgeGroup = contestDomain.enums.AgeGroup;

namespace contestRepoDB.participants
{
    public interface ParticipantsRepo : IRepository<int, Participant>
    {
        List<Participant> FindParticipantsByAgeAndType(Type type, AgeGroup ageGroup);
    }
}
