using System.Collections.Generic;
using contest_csharp.domain;
using Type = contest_csharp.domain.enums.Type;
using AgeGroup = contest_csharp.domain.enums.AgeGroup;

namespace contest_csharp.repo.participants
{
    public interface ParticipantsRepo : IRepository<int, Participant>
    {
        IList<Participant> FindParticipantsByAgeAndType(Type type, AgeGroup ageGroup);
    }
}
