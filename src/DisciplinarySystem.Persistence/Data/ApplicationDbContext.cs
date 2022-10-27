using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.Domain.Commonications;
using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Domain.Defences;
using DisciplinarySystem.Domain.DisciplinaryCase.Cases;
using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using DisciplinarySystem.Domain.Epistles;
using DisciplinarySystem.Domain.FinalVotes;
using DisciplinarySystem.Domain.Informants;
using DisciplinarySystem.Domain.Invitations;
using DisciplinarySystem.Domain.Meetings;
using DisciplinarySystem.Domain.Objections;
using DisciplinarySystem.Domain.PrimaryVotes;
using DisciplinarySystem.Domain.RelatedInfos;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.Domain.Verdicts;
using DisciplinarySystem.Domain.Violations;
using DisciplinarySystem.SharedKernel.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace DisciplinarySystem.Persistence.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IMediator _mediator;
        public ApplicationDbContext ( DbContextOptions<ApplicationDbContext> options , IMediator mediator ) : base(options)
        {
            _mediator = mediator;
        }

        // commonication
        public DbSet<SMS> SMS { get; set; }

        // auth
        public DbSet<AuthUser> AuthUser { get; set; }
        public DbSet<AuthRole> AuthRole { get; set; }


        // app
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        public DbSet<ViolationCategory> ViolationCategories { get; set; }
        public DbSet<Violation> Violations { get; set; }
        public DbSet<ViolationDocument> ViolationDocuments { get; set; }

        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Plaintiff> Plaintiffs { get; set; }
        public DbSet<Complaining> Complainings { get; set; }
        public DbSet<ComplaintDocument> ComplaintDocuments { get; set; }

        public DbSet<Case> Cases { get; set; }

        public DbSet<Epistle> Epistles { get; set; }
        public DbSet<EpistleDocument> EpistleDocuments { get; set; }

        public DbSet<Informed> Informants { get; set; }
        public DbSet<InformedDocument> InformantsDocuments { get; set; }

        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<InvitationUser> InvitationUsers { get; set; }
        public DbSet<InvitationDocument> InvitationDocuments { get; set; }

        public DbSet<Defence> Defences { get; set; }
        public DbSet<DefenceDocument> DefenceDocuments { get; set; }

        public DbSet<Verdict> Verdicts { get; set; }
        public DbSet<PrimaryVote> PrimaryVotes { get; set; }
        public DbSet<PrimaryVoteDocument> PrimaryVoteDocument { get; set; }

        public DbSet<Objection> Objections { get; set; }
        public DbSet<ObjectionDocument> ObjectionDocuments { get; set; }

        public DbSet<FinalVote> FinalVotes { get; set; }
        public DbSet<FinalVoteDocument> FinalVoteDocuments { get; set; }

        public DbSet<RelatedInfo> RelatedInfos { get; set; }
        public DbSet<RelatedInfoDocument> RelatedInfoDocuments { get; set; }

        public DbSet<CentralCommitteeVote> CentralCommitteeVote { get; set; }
        public DbSet<CentralCommitteeVoteDocument> CentralCommitteeVoteDocuments { get; set; }


        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingUsers> MeetingUsers { get; set; }
        public DbSet<Proceedings> Proceedings { get; set; }


        protected override void OnModelCreating ( ModelBuilder modelBuilder )
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override async Task<int> SaveChangesAsync ( CancellationToken cancellationToken = new CancellationToken() )
        {
            // ignore events if no dispatcher provided
            if ( _mediator != null ) await SendRequests();

            int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return result;
        }

        public override int SaveChanges ()
        {
            return SaveChangesAsync().GetAwaiter().GetResult();
        }


        private async Task SendRequests ()
        {
            var entitiesWithEvents = ChangeTracker
              .Entries()
              .Select(e => e.Entity as BaseEntity<Guid>)
              .Where(e => e?.Events != null && e.Events.Any())
              .ToArray();

            foreach ( var entity in entitiesWithEvents )
            {
                var events = entity.Events.ToArray();
                entity.Events.Clear();
                foreach ( var domainEvent in events )
                {
                    await _mediator.Send(domainEvent).ConfigureAwait(false);
                }
            }

        }
    }
}
