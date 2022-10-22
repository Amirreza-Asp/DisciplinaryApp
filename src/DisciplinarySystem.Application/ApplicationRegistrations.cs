using DisciplinarySystem.Application.Authentication;
using DisciplinarySystem.Application.Authentication.Interfaces;
using DisciplinarySystem.Application.Cases;
using DisciplinarySystem.Application.Cases.Interfaces;
using DisciplinarySystem.Application.Complaints;
using DisciplinarySystem.Application.Complaints.Interfaces;
using DisciplinarySystem.Application.Defences;
using DisciplinarySystem.Application.Defences.Interfaces;
using DisciplinarySystem.Application.DisciplinaryCase.Cases;
using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes;
using DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes.Interfaces;
using DisciplinarySystem.Application.Epistles;
using DisciplinarySystem.Application.Epistles.Interfaces;
using DisciplinarySystem.Application.FinalVotes;
using DisciplinarySystem.Application.FinalVotes.Interfaces;
using DisciplinarySystem.Application.Informants;
using DisciplinarySystem.Application.Informants.Interfaces;
using DisciplinarySystem.Application.Invitations;
using DisciplinarySystem.Application.Invitations.Interfaces;
using DisciplinarySystem.Application.Meetings;
using DisciplinarySystem.Application.Meetings.Interfaces;
using DisciplinarySystem.Application.Objections;
using DisciplinarySystem.Application.Objections.Interfaces;
using DisciplinarySystem.Application.PrimaryVotes;
using DisciplinarySystem.Application.PrimaryVotes.Interfaces;
using DisciplinarySystem.Application.RelatedInfos;
using DisciplinarySystem.Application.RelatedInfos.Interfaces;
using DisciplinarySystem.Application.Users;
using DisciplinarySystem.Application.Users.Interfaces;
using DisciplinarySystem.Application.Verdicts;
using DisciplinarySystem.Application.Verdicts.Interfaces;
using DisciplinarySystem.Application.Violations;
using DisciplinarySystem.Application.Violations.Intefaces;
using Microsoft.Extensions.DependencyInjection;

namespace DisciplinarySystem.Persistence
{
    public static class ApplicationRegistrations
    {
        public static IServiceCollection AddApplicationLayerDependencies (
            this IServiceCollection services )
        {

            // Auth
            services.AddScoped<IPasswordHasher , PasswordHasher>();
            services.AddScoped<IAuthService , AuthService>();

            // Users
            services.AddScoped<IRoleService , RoleService>();
            services.AddScoped<IUserService , UserService>();

            // Violations
            services.AddScoped<IViolationCategoryService , ViolationCategoryService>();
            services.AddScoped<IViolationService , ViolationService>();
            // Complaints
            services.AddScoped<IComplaintService , ComplaintService>();
            services.AddScoped<IComplainingService , ComplainingService>();
            // Cases
            services.AddScoped<ICaseService , CaseService>();
            services.AddScoped<ICaseStatusService , CaseStatusService>();
            // Epistles
            services.AddScoped<IEpistleService , EpistleService>();
            // Informants
            services.AddScoped<IInformedService , InformedService>();
            // Invitations
            services.AddScoped<IInvitationService , InvitationService>();
            // Defence
            services.AddScoped<IDefenceService , DefenceService>();
            // Verdict
            services.AddScoped<IVerdictService , VerdictService>();
            // PrimaryVote
            services.AddScoped<IPrimaryVoteService , PrimaryVoteService>();
            // Objection
            services.AddScoped<IObjectionService , ObjectionService>();
            // FinalVote
            services.AddScoped<IFinalVoteService , FinalVoteService>();
            // Related Info
            services.AddScoped<IRelatedInfoService , RelatedInfoService>();
            // Central Committee Vote
            services.AddScoped<ICentralCommitteeVoteService , CentralCommitteeVoteService>();

            // Metting
            services.AddScoped<IMeetingService , MeetingService>();
            services.AddScoped<IProceedingsService , ProceedingsService>();


            return services;
        }
    }
}
