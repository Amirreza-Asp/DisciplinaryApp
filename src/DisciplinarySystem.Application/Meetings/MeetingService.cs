using DisciplinarySystem.Application.Contracts.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Meetings.Interfaces;
using DisciplinarySystem.Application.Meetings.ViewModels;
using DisciplinarySystem.Domain.Meetings;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Meetings
{
    public class MeetingService : IMeetingService
    {
        private readonly IRepository<Meeting> _meetRepo;
        private readonly IRepository<MeetingUsers> _meetUsersRepo;
        private readonly ISmsService _smsService;
        private readonly IRepository<User> _userRepo;
        private readonly IUserApi _userApi;

        public MeetingService ( IRepository<Meeting> meetRepo , IRepository<MeetingUsers> meetUsersRepo , ISmsService smsService , IRepository<User> userRepo , IUserApi userApi )
        {
            _meetRepo = meetRepo;
            _meetUsersRepo = meetUsersRepo;
            _smsService = smsService;
            _userRepo = userRepo;
            _userApi = userApi;
        }

        public int GetCount ( Expression<Func<Meeting , bool>> filters = null )
        {
            return _meetRepo.GetCount(filters);
        }

        public async Task<IEnumerable<Meeting>> ListAsync ( Expression<Func<Meeting , bool>> filters = null )
        {
            return await _meetRepo.GetAllAsync(filter: filters);
        }

        public async Task<Meeting> GetByIdAsync ( Guid id )
        {
            return await _meetRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.MeetingUsers).ThenInclude(u => u.User));
        }

        public async Task CreateAsync ( CreateMeeting command )
        {
            var userIds = command.InvitedUsers.Split("\t").ToList();

            int startHours = int.Parse(command.Start.Split(':')[0]);
            int startMins = int.Parse(command.Start.Split(':')[1]);
            int endHours = int.Parse(command.End.Split(':')[0]);
            int endMins = int.Parse(command.End.Split(':')[1]);



            DateTime from = ConvertToDateTime(command.MeetingDate , new TimeOnly(startHours , startMins));
            DateTime to = ConvertToDateTime(command.MeetingDate , new TimeOnly(endHours , endMins));
            var entity = new Meeting(command.Title , new DateTimeRange(from , to) , command.Description);

            if ( from > DateTime.Now )
            {
                foreach ( var userId in userIds )
                {
                    var nationalCode = _userRepo.Find(Guid.Parse(userId)).NationalCode;
                    var user = await _userApi.GetUserAsync(nationalCode);
                    await _smsService.Send(command.Description ?? $"جلسه حراست دانشگاه در تاریخ {from.ToShamsi()} ساعت {from.TimeOfDay.ToString().Substring(0 , 5)}" , user.Mobile);
                }
            }

            userIds.ForEach(id => _meetUsersRepo.Add(new MeetingUsers(Guid.Parse(id) , entity.Id)));

            _meetRepo.Add(entity);
            await _meetRepo.SaveAsync();
        }

        public async Task UpdateAsync ( UpdateMeeting command )
        {
            Meeting entity = await _meetRepo.FirstOrDefaultAsync(u => u.Id == command.Id ,
                include: source => source.Include(u => u.MeetingUsers));

            if ( entity == null )
                return;

            var userIds = command.InvitedUsers.Trim().Split("\t").ToList();
            var startTime = ConvertToDateTime(command.MeetingDate , command.GetStartTime());
            var endTime = ConvertToDateTime(command.MeetingDate , command.GetEndTime());

            if ( startTime > DateTime.Now )
            {
                foreach ( var userId in userIds )
                {
                    var nationalCode = _userRepo.Find(Guid.Parse(userId)).NationalCode;
                    var user = await _userApi.GetUserAsync(nationalCode);
                    await _smsService.Send(command.Description ?? $"جلسه حراست دانشگاه در تاریخ {startTime.ToShamsi()} ساعت {startTime.TimeOfDay.ToString().Substring(0 , 5)}" , user.Mobile);
                }
            }

            if ( userIds.Count() > 0 )
                userIds = userIds.Where(id => !entity.MeetingUsers.Select(u => u.UserId).Contains(Guid.Parse(id))).ToList();


            entity.WithTitle(command.Title)
                .WithDescription(command.Description)
                .WithEndDate(endTime)
                .WithStartDate(startTime);




            userIds.ForEach(id => _meetUsersRepo.Add(new MeetingUsers(Guid.Parse(id) , entity.Id)));

            _meetRepo.Update(entity);
            await _meetRepo.SaveAsync();
        }

        public async Task<bool> RemoveAsync ( Guid id )
        {
            var meet = await _meetRepo.FirstOrDefaultAsync(u => u.Id == id);
            if ( meet == null )
                return false;

            _meetRepo.Remove(meet);
            await _meetRepo.SaveAsync();
            return true;
        }

        public async Task<bool> RemoveUserAsync ( Guid userId )
        {
            var user = await _meetUsersRepo.FirstOrDefaultAsync(u => u.UserId == userId);
            if ( user == null )
                return false;

            _meetUsersRepo.Remove(user);
            await _meetUsersRepo.SaveAsync();
            return true;
        }

        private DateTime ConvertToDateTime ( DateTime dateTime , TimeOnly time )
        {
            return new DateTime(dateTime.Year , dateTime.Month ,
                dateTime.Day , time.Hour , time.Minute , time.Second);
        }
    }
}
