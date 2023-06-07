using contest_csharp.domain;
using contest_csharp.dto;
using System.Collections.Generic;

namespace contest_csharp.mapper
{
    public class Participant2ParticipantDTO
    {
        public ParticipantDTO Convert(Participant participant)
        {
            return new ParticipantDTO(participant.Name, participant.Age);
        }

        public IList<ParticipantDTO> ConvertList(IList<Participant> participants)
        {
            IList<ParticipantDTO> participantDTOS = new List<ParticipantDTO>();
            foreach(Participant participant in participants)
            {
                participantDTOS.Add(Convert(participant));
            }
            return participantDTOS;
        }
    }
}
