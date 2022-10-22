using DisciplinarySystem.Application.Verdicts.Interfaces;
using DisciplinarySystem.Application.Verdicts.ViewModels;
using DisciplinarySystem.Domain.Verdicts;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Verdicts
{
    public class VerdictService : IVerdictService
    {
        private readonly IRepository<Verdict> _repo;

        public VerdictService(IRepository<Verdict> repository)
        {
            _repo = repository;
        }

        public async Task<IEnumerable<VotesNumberForFinalVote>> GetFinalVoteCountAsync()
        {
            return await _repo.GetAllAsync(
                 select: verdict => new VotesNumberForFinalVote
                 {
                     Vote = verdict.Title,
                     Count = verdict.FinalVotes.Count()
                 });
        }

        public async Task CreateAsync(CreateVerdict command)
        {
            var entity = new Verdict(command.Title, command.Description);
            _repo.Add(entity);
            await _repo.SaveAsync();
        }

        public async Task<Verdict> GetByTitleAsync(string title)
        {
            return await _repo.FirstOrDefaultAsync(u => u.Title == title);
        }

        public async Task UpdateAsync(UpdateVerdict command)
        {
            var entity = await _repo.FindAsync(command.Id);
            entity.WithTitle(command.Title)
                .WithDescription(command.Description);

            _repo.Update(entity);
            await _repo.SaveAsync();
        }

        public async Task<Verdict> GetByIdAsync(long id)
        {
            return await _repo.FindAsync(id);
        }

        public int GetCount(Expression<Func<Verdict, bool>> filter = null)
        {
            return _repo.GetCount(filter);
        }

        public async Task<List<Verdict>> GetListAsync(int skip = 0, int take = 10)
        {
            var objs = await _repo.GetAllAsync(skip: skip, take: take);
            return objs.ToList();
        }

        public async Task<bool> RemoveAsync(long id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
                return false;

            _repo.Remove(entity);
            await _repo.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectListAsync()
        {
            return await _repo.GetAllAsync<SelectListItem>(
                select: u => new SelectListItem
                {
                    Text = u.Title,
                    Value = u.Id.ToString()
                });
        }
    }
}
