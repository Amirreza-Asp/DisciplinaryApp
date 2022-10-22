using DisciplinarySystem.Application.Defences.Interfaces;
using DisciplinarySystem.Application.Defences.ViewModels;
using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Domain.Defences;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Defences
{
    public class DefenceService : IDefenceService
    {
        private readonly IRepository<Defence> _defRepo;
        private readonly IRepository<DefenceDocument> _docRepo;
        private readonly ICaseReposiotry _caseRepo;
        private readonly ICaseStatusService _caseStatusService;

        public DefenceService(IRepository<Defence> defRepo, IRepository<DefenceDocument> docRepo, ICaseReposiotry caseRepo, ICaseStatusService caseStatusService)
        {
            _defRepo = defRepo;
            _docRepo = docRepo;
            _caseRepo = caseRepo;
            _caseStatusService = caseStatusService;
        }
        public int GetCount(Expression<Func<Defence, bool>> filter = null) => _defRepo.GetCount(filter);

        public async Task<IEnumerable<DefenceDetails>> ListAsync(Expression<Func<Defence, bool>> filters = null, int skip = 0, int take = 10)
        {
            return await _defRepo.GetAllAsync(
                filter: filters,
                orderBy: source => source.OrderByDescending(u => u.CreateDate),
                select: entity => new DefenceDetails
                {
                    Subject = entity.Subject,
                    CreateDate = entity.CreateDate,
                    Id = entity.Id,
                    UpdateDate = entity.UpdateDate
                },
                skip: skip,
                take: take);
        }

        public async Task<Defence> GetByIdWithCaseAsync(Guid id)
        {
            return await _defRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id,
                include: source => source
                    .Include(u => u.Documents)
                    .Include(u => u.Case)
                    .ThenInclude(u => u.Complaint)
                    .ThenInclude(u => u.Complaining));
        }

        public async Task<Defence> GetByIdAsync(Guid id)
        {
            return await _defRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id,
                include: source => source
                    .Include(u => u.Documents));
        }
        public async Task<DefenceDocument> GetDocumentByIdAsync(Guid id)
        {
            return await _docRepo.FindAsync(id);
        }

        public async Task CreateAsync(CreateDefence command, IFormFileCollection files)
        {
            var entity = new Defence(command.Subject, command.Description, command.CaseId);
            AddDocuments(entity.Id, files);
            _defRepo.Add(entity);

            var caseEntity = await _caseRepo.FindAsync(command.CaseId);
            if ((int)caseEntity.Status < (int)CaseStatus.Complete)
            {
                caseEntity.WithStatus(CaseStatus.Complete);
                _caseRepo.Update(caseEntity);
            }

            await _defRepo.SaveAsync();
        }

        public async Task UpdateAsync(UpdateDefence command, IFormFileCollection files)
        {
            var entity = await _defRepo.FindAsync(command.Id);
            if (entity == null)
                return;

            entity.WithSubject(command.Subject)
                .WithDescription(command.Description)
                .WithUpdateDate(DateTime.Now);

            AddDocuments(entity.Id, files);
            _defRepo.Update(entity);
            await _defRepo.SaveAsync();
        }

        public async Task<IEnumerable<DefenceDocument>> GetDocumentsAsync(Guid id)
        {
            return await _docRepo.GetAllAsync(u => u.DefenceId == id);
        }

        public async Task<bool> RemoveAsync(Guid id)
        {
            var defence = await _defRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id,
                include: source => source.Include(u => u.Documents));

            if (defence == null)
                return false;


            defence.Documents.ToList().ForEach(doc => doc.RemoveFile());
            _defRepo.Remove(defence);

            await _defRepo.SaveAsync();

            await _caseStatusService.Fix(defence.CaseId);
            return true;
        }

        public async Task<bool> RemoveFileAsync(Guid id)
        {
            var doc = await _docRepo.FindAsync(id);

            if (doc == null)
                return false;

            doc.RemoveFile();
            _docRepo.Remove(doc);
            await _docRepo.SaveAsync();

            return true;
        }


        private void AddDocuments(Guid defenceId, IFormFileCollection files)
        {
            if (files == null)
                return;

            foreach (var file in files)
            {
                var doc = new DefenceDocument(defenceId, file.FileName, new Document(file.FileName, file.ReadBytes()));
                doc.CreateFile();
                _docRepo.Add(doc);
            }
        }

    }
}
