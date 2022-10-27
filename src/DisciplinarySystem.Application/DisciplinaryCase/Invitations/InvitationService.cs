using DisciplinarySystem.Application.Contracts.Interfaces;
using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Invitations.Interfaces;
using DisciplinarySystem.Application.Invitations.ViewModels;
using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Domain.Invitations;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Invitations
{
    public class InvitationService : IInvitationService
    {
        private readonly IRepository<Invitation> _invRepo;
        private readonly IRepository<InvitationDocument> _docRepo;
        private readonly IRepository<InvitationUser> _iurRepo;
        private readonly IRepository<User> _userRepo;
        private readonly ICaseReposiotry _caseRepo;
        private readonly ISmsService _smsService;
        private readonly ICaseStatusService _caseStatusService;
        private readonly IUserApi _userApi;
        private readonly IRepository<Plaintiff> _plaintiffRepo;
        private readonly IRepository<Complaining> _compRepo;

        public InvitationService ( IRepository<Invitation> invRepo , IRepository<InvitationDocument> docRepo , IRepository<InvitationUser> iurRepo , IRepository<User> userRepo , ICaseReposiotry caseRepo , ISmsService smsService , ICaseStatusService caseStatusService , IUserApi userApi , IRepository<Plaintiff> plaintiffRepo , IRepository<Complaining> compRepo )
        {
            _invRepo = invRepo;
            _docRepo = docRepo;
            _iurRepo = iurRepo;
            _userRepo = userRepo;
            _caseRepo = caseRepo;
            _smsService = smsService;
            _caseStatusService = caseStatusService;
            _userApi = userApi;
            _plaintiffRepo = plaintiffRepo;
            _compRepo = compRepo;
        }

        public async Task CreateAsync ( CreateInvitation command , IFormFileCollection files )
        {
            var invPersons = JsonConvert.DeserializeObject<List<InvitationsInfo>>(command.PersonsId);
            var complaining = invPersons?.Where(u => u.Optgroup == SD.ComplainingGroup).FirstOrDefault();
            var plaintiff = invPersons?.Where(u => u.Optgroup == SD.PlaitiffGroup).FirstOrDefault();
            var users = invPersons?.Where(u => u.Optgroup == SD.TajdidUserGroup || u.Optgroup == SD.BadaviUserGroup).ToList();
            var date = DateTimeConvertor.GetDateFromString(command.InviteDate);

            await SendSms(date , users , complaining , plaintiff , command.Description);
            var entity = new Invitation(command.Subject , command.Description ,
                command.CaseId , date , complaining?.Id , plaintiff?.Id);

            AddDocuments(files , entity.Id);
            AddUsers(users.Select(u => u.Id).ToList() , entity.Id);



            var caseEntity = await _caseRepo.FindAsync(command.CaseId);
            if ( ( int ) caseEntity.Status < ( int ) CaseStatus.Complete )
            {
                caseEntity.WithStatus(CaseStatus.Complete);
                _caseRepo.Update(caseEntity);
            }


            _invRepo.Add(entity);
            await _invRepo.SaveAsync();
        }

        public async Task<Invitation> GetByIdAsync ( Guid id )
        {
            return await _invRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source
                                .Include(u => u.Documents)
                                .Include(u => u.InvitationUsers)
                                    .ThenInclude(u => u.User)
                                        .ThenInclude(u => u.Role)
                                .Include(u => u.Plaintiff)
                               .Include(u => u.Complaining));
        }

        public int GetCount ( Expression<Func<Invitation , bool>> filter = null ) => _invRepo.GetCount(filter);

        public async Task<InvitationDocument> GetDocumentByIdAsync ( Guid id )
        {
            return await _docRepo.FindAsync(id);
        }

        public async Task<List<InvitationDocument>> GetDocumentsAsync ( Guid id )
        {
            var obj = await _docRepo.GetAllAsync(u => u.InvitationId == id);
            return obj.ToList();
        }

        public async Task<List<InvitationsInfo>> GetInvitePersonsAsync ( Guid id )
        {
            var entity = await _invRepo.FirstOrDefaultAsync(u => u.Id == id ,
                include: source =>
                source.Include(u => u.Plaintiff)
                      .Include(u => u.Complaining)
                      .Include(u => u.InvitationUsers)
                        .ThenInclude(u => u.User));

            var persons = entity.InvitationUsers
                .Select(u => new InvitationsInfo
                {
                    Id = u.UserId ,
                    Name = u.User.FullName ,
                    Optgroup = u.User.Type == SD.Tajdid ? SD.TajdidUserGroup : SD.BadaviUserGroup
                }).ToList();

            if ( entity.ComplainingId.HasValue )
                persons.Insert(0 , new InvitationsInfo
                {
                    Id = entity.ComplainingId.Value ,
                    Name = entity.Complaining.FullName ,
                    Optgroup = SD.ComplainingGroup
                });

            if ( entity.PlaintiffId.HasValue )
                persons.Insert(0 , new InvitationsInfo
                {
                    Id = entity.PlaintiffId.Value ,
                    Name = entity.Plaintiff.FullName ,
                    Optgroup = SD.PlaitiffGroup
                });

            return persons;
        }

        public async Task<List<SelectListItem>> GetPersonsAsync ( long caseId )
        {
            var selectListBadaviGroup = new SelectListGroup { Name = SD.BadaviUserGroup };

            var persons = _userRepo.GetAll<SelectListItem>(
                filter: u => u.Type == SD.Badavi ,
                select: entity => new SelectListItem
                {
                    Text = entity.FullName ,
                    Value = entity.Id.ToString() ,
                    Group = selectListBadaviGroup
                }).ToList();

            var selectListTajdidGroup = new SelectListGroup { Name = SD.TajdidUserGroup };
            persons.AddRange(_userRepo.GetAll<SelectListItem>(
                filter: u => u.Type == SD.Tajdid ,
                select: entity => new SelectListItem
                {
                    Text = entity.FullName ,
                    Value = entity.Id.ToString() ,
                    Group = selectListTajdidGroup
                }));

            var caseEntity = await _caseRepo.FirstOrDefaultAsync(
                    filter: u => u.Id == caseId ,
                    include: source => source.Include(u => u.Complaint).ThenInclude(u => u.Complaining)
                                                .Include(u => u.Complaint).ThenInclude(u => u.Plaintiff));

            persons.Insert(0 , new SelectListItem
            {
                Group = new SelectListGroup { Name = "متشاکی" } ,
                Text = caseEntity.Complaint.Complaining.FullName ,
                Value = caseEntity.Complaint.Complaining.Id.ToString()
            });

            persons.Insert(0 , new SelectListItem
            {
                Group = new SelectListGroup { Name = "شاکی" } ,
                Text = caseEntity.Complaint.Plaintiff.FullName ,
                Value = caseEntity.Complaint.Plaintiff.Id.ToString()
            });

            return persons;
        }

        public async Task<IEnumerable<InvitationDetails>> ListAsync ( Expression<Func<Invitation , bool>> filters = null , int skip = 0 , int take = 10 )
        {
            return await _invRepo.GetAllAsync<InvitationDetails>(
                    filter: filters ,
                    orderBy: source => source.OrderByDescending(b => b.Subject).OrderByDescending(u => u.CreateDate) ,
                    select: entity => new InvitationDetails
                    {
                        Id = entity.Id ,
                        Subject = entity.Subject ,
                        CreateDate = entity.CreateDate ,
                        UpdateDate = entity.UpdateDate ,
                        InviteDate = entity.InviteDate
                    } ,
                    skip: skip ,
                    take: take);
        }

        public async Task<bool> RemoveAsync ( Guid id )
        {
            var entity = await _invRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Documents));

            if ( entity == null )
                return false;

            entity.Documents.ToList().ForEach(doc => doc.RemoveFile());
            _invRepo.Remove(entity);
            await _invRepo.SaveAsync();
            await _caseStatusService.Fix(entity.CaseId);
            return true;
        }

        public async Task<bool> RemoveFileAsync ( Guid id )
        {
            var doc = await _docRepo.FindAsync(id);
            if ( doc == null )
                return false;

            doc.RemoveFile();
            _docRepo.Remove(doc);
            await _docRepo.SaveAsync();

            return true;
        }

        public async Task RemovePersonAsync ( Guid invId , Guid personId , string group )
        {
            if ( group.Equals(SD.UserGroup) )
            {
                var user = await _iurRepo.FirstOrDefaultAsync(u => u.UserId == personId);
                _iurRepo.Remove(user);
                await _iurRepo.SaveAsync();
                return;
            }

            var inv = _invRepo.FirstOrDefault(u => u.Id == invId);
            if ( group.Equals(SD.ComplainingGroup) )
                inv.WithComplainingId(null);
            else
                inv.WithPlaintiffId(null);

            _invRepo.Update(inv);
            await _invRepo.SaveAsync();
        }

        public async Task UpdateAsync ( UpdateInvitation command , IFormFileCollection files )
        {
            Invitation entity = await _invRepo.FirstOrDefaultAsync(u => u.Id == command.Id ,
                include: source => source.Include(u => u.InvitationUsers));

            if ( entity == null )
                return;

            var invPersons = JsonConvert.DeserializeObject<List<InvitationsInfo>>(command.NewPersonsId);
            var complaining = invPersons.Where(u => u.Optgroup == SD.ComplainingGroup).FirstOrDefault();
            var plaintiff = invPersons.Where(u => u.Optgroup == SD.PlaitiffGroup).FirstOrDefault();
            var users = invPersons.Where(u => u.Optgroup == SD.TajdidUserGroup || u.Optgroup == SD.BadaviUserGroup).ToList();
            var date = DateTimeConvertor.GetDateFromString(command.InviteDate);

            entity.WithSubject(command.Subject)
                .WithComplainingId(complaining == null ? entity.ComplainingId : complaining.Id)
                .WithPlaintiffId(plaintiff == null ? entity.PlaintiffId : plaintiff.Id)
                .WithDescription(command.Description)
                .WithUpdateDate(DateTime.Now)
                .WithInviteDate(date);

            var oldUsers = users.Where(u => entity.InvitationUsers.Select(s => s.UserId).Contains(u.Id)).ToList();
            foreach ( var user in oldUsers )
                users?.Remove(user);

            await SendSms(date , users , complaining , plaintiff , command.Description);

            AddDocuments(files , entity.Id);
            AddUsers(users?.Select(u => u.Id).ToList() , entity.Id);
            _invRepo.Update(entity);
            await _invRepo.SaveAsync();
        }

        private void AddDocuments ( IFormFileCollection files , Guid id )
        {
            if ( files == null )
                return;

            foreach ( var file in files )
            {
                var doc = new InvitationDocument(Guid.NewGuid() , id , file.FileName , new Document(file.FileName , file.ReadBytes()));
                doc.CreateFile();
                _docRepo.Add(doc);
            }
        }
        private void AddUsers ( List<Guid>? usersId , Guid invId )
        {
            if ( usersId == null )
                return;

            foreach ( var userId in usersId )
            {
                var invUser = new InvitationUser(userId , invId);
                _iurRepo.Add(invUser);
            }
        }
        private async Task SendSms ( DateTime date , List<InvitationsInfo> users , InvitationsInfo? complaining , InvitationsInfo? plaintiff , string description )
        {

            if ( date > DateTime.Now )
            {
                foreach ( var user in users )
                {
                    var userNationalCode = _userRepo.Find(user.Id).NationalCode;
                    var person = await _userApi.GetUserAsync(userNationalCode.Value);

                    await _smsService.Send(description , person.Mobile);
                }
                if ( plaintiff != null )
                {
                    var plaintiffPerson = await _plaintiffRepo.FindAsync(plaintiff.Id);
                    await _smsService.Send(description , plaintiffPerson.PhoneNumber);
                }
                if ( complaining != null )
                {
                    var complainingPreson = await _compRepo.FindAsync(complaining.Id);
                    var person = await _userApi.GetUserAsync(complainingPreson.NationalCode.Value);
                    await _smsService.Send(description , person.Mobile);
                }
            }

        }
    }
}
