using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Application.Epistles.Interfaces;
using DisciplinarySystem.Application.Epistles.ViewModels;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Domain.Complaints.Interfaces;
using DisciplinarySystem.Domain.Epistles;
using DisciplinarySystem.Domain.Epistles.Interfaces;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Epistles
{
    public class EpistleService : IEpistleService
    {
        private readonly IEpistleRepository _epistleRepo;
        private readonly IComplaintRepository _complaintRepo;
        private readonly ICaseReposiotry _caseRepo;
        private readonly IRepository<EpistleDocument> _documentRepo;
        private readonly ICaseStatusService _caseStatusService;

        public EpistleService ( IEpistleRepository epistleRepo , IRepository<EpistleDocument> documentRepo , IComplaintRepository complaintRepo , ICaseReposiotry caseRepo , ICaseStatusService caseStatusService )
        {
            _epistleRepo = epistleRepo;
            _documentRepo = documentRepo;
            _complaintRepo = complaintRepo;
            _caseRepo = caseRepo;
            _caseStatusService = caseStatusService;
        }

        public async Task<EpistleDocument> GetDocumentByIdAsync ( Guid id )
        {
            return await _documentRepo.FindAsync(id);
        }

        public async Task CreateAsync ( CreateEpistle command , IFormFileCollection files )
        {
            var epistle = new Epistle(command.Type , command.Subject , command.Sender ,
                command.Reciver , command.CaseId , command.ComplaintId , command.Description);

            var id = await _epistleRepo.CreateAsync(epistle);
            AddDocuments(files , id);
            await _documentRepo.SaveAsync();
        }

        public async Task<IEnumerable<GetEpistle>> GetAllAsync ( Expression<Func<Epistle , bool>> filter = null , int skip = 0 , int take = 10 )
        {
            var entities = await _epistleRepo.GetAllAsync(
                filter ,
                orderBy: source => source.OrderByDescending(u => u.CreateDate) ,
                skip: skip ,
                take: take);

            return entities.Select(entity => new GetEpistle
            {
                CaseId = entity.CaseId ,
                Sender = entity.Sender ,
                Subject = entity.Subject ,
                CreateDate = entity.CreateDate ,
                UpdateDate = entity.UpdateDate ,
                ComplaintId = entity.ComplaintId ,
                Reciver = entity.Reciver ,
                Type = entity.Type ,
                Id = entity.Id
            });
        }

        public Task<Epistle> GetByIdAsync ( long id )
        {
            return _epistleRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Documents));
        }

        public bool HasComplaint ( long id )
        {
            return _complaintRepo.IsExists(id);
        }

        public bool HasCase ( long id )
        {
            return _caseRepo.IsExist(id);
        }

        public async Task<bool> RemoveAsync ( long id )
        {
            var entity = await _epistleRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Documents));

            if ( entity == null )
                return false;

            _epistleRepo.Remove(entity);
            entity.Documents.ToList().ForEach(doc =>
            {
                doc.RemoveFile();
                _documentRepo.Remove(doc);
            });

            await _epistleRepo.SaveAsync();
            if ( entity.CaseId.HasValue )
                await _caseStatusService.Fix(entity.CaseId.Value);
            return true;
        }

        public async Task<bool> RemoveFileAsync ( Guid id )
        {
            var doc = await _documentRepo.FindAsync(id);
            if ( doc == null )
                return false;

            doc.RemoveFile();
            _documentRepo.Remove(doc);
            await _documentRepo.SaveAsync();

            return true;
        }

        public async Task UpdateAsync ( UpdateEpistle command , IFormFileCollection files )
        {
            Epistle entity = await _epistleRepo.FindAsync(command.Id);
            if ( entity == null )
                return;

            entity.WithSender(command.Sender)
                .WithDescription(command.Description)
                .WithReciver(command.Reciver)
                .WithCaseId(command.CaseId)
                .WithComplaintId(command.ComplaintId)
                .WithSubject(command.Subject)
                .WithType(command.Type);

            AddDocuments(files , command.Id);
            _epistleRepo.Update(entity);
            await _epistleRepo.SaveAsync();
        }

        public async Task<List<GetInformedDocument>> GetCurrentDocumentsAsync ( long epistleId )
        {
            var documents = await _documentRepo.GetAllAsync(u => u.EpistleId == epistleId);
            return documents.Select(doc => GetInformedDocument.Create(doc)).ToList();
        }

        public int GetCount ( Expression<Func<Epistle , bool>> filter = null )
        {
            return _epistleRepo.GetCount(filter);
        }

        #region Utilities
        private void AddDocuments ( IFormFileCollection files , long epistleId )
        {
            if ( files == null )
                return;

            foreach ( var file in files )
            {
                var doc = new EpistleDocument(Guid.NewGuid() , epistleId ,
                    file.FileName , new Document(file.FileName , file.ReadBytes()));

                _documentRepo.Add(doc);
                doc.CreateFile();
            }
        }
        #endregion
    }
}
