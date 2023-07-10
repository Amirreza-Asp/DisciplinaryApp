using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Domain.Defences;
using DisciplinarySystem.Domain.Epistles;
using DisciplinarySystem.Domain.Informants;
using DisciplinarySystem.Domain.Invitations;
using DisciplinarySystem.Domain.Objections;
using DisciplinarySystem.Domain.RelatedInfos;
using DisciplinarySystem.Domain.Violations;

namespace DisciplinarySystem.Domain.DisciplinaryCase.Cases
{
    public class Case : BaseEntity<long>
    {
        public Case(long id, long complaintId)
        {
            Id = Guard.Against.Negative(id);
            ComplaintId = Guard.Against.NegativeOrZero(complaintId);
            Status = CaseStatus.Filing;
        }

        public CaseStatus Status { get; private set; }
        public long ComplaintId { get; private set; }


        public Complaint Complaint { get; private set; }
        public ICollection<Epistle> Epistles { get; private set; }
        public ICollection<Violation> Violations { get; private set; }
        public ICollection<Informed> Informants { get; private set; }
        public ICollection<Invitation> Invitations { get; private set; }
        public ICollection<Defence> Defences { get; private set; }
        public ICollection<Objection> Objections { get; set; }
        public ICollection<RelatedInfo> RelatedInfos { get; set; }


        public Case WithStatus(CaseStatus status)
        {
            Status = status;
            return this;
        }
    }
}
