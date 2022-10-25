using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Objections.Interfaces;
using DisciplinarySystem.Application.Objections.ViewModels;
using DisciplinarySystem.Domain.Objections;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Objections
{
    public class ObjectionService : IObjectionService
    {
        private readonly IRepository<Objection> _objRepo;
        private readonly IRepository<ObjectionDocument> _docRepo;
        private readonly ICaseReposiotry _caseRepo;
        private readonly ICaseStatusService _caseStatusService;

        public ObjectionService ( IRepository<Objection> objRepo , IRepository<ObjectionDocument> docRepo , ICaseReposiotry caseRepo , ICaseStatusService caseStatusService )
        {
            _objRepo = objRepo;
            _docRepo = docRepo;
            _caseRepo = caseRepo;
            _caseStatusService = caseStatusService;
        }

        public async Task CreateAsync ( CreateObjection command , IFormFileCollection files )
        {
            var entity = new Objection(command.Subject , command.Description , command.Result , command.CaseId);
            AddDocuments(entity.Id , files);
            _objRepo.Add(entity);


            var caseEntity = await _caseRepo.FindAsync(command.CaseId);
            if ( ( int ) caseEntity.Status < ( int ) CaseStatus.Objection )
            {
                caseEntity.WithStatus(CaseStatus.Objection);
                _caseRepo.Update(caseEntity);
            }


            await _objRepo.SaveAsync();
        }

        public async Task<ObjectionDocument> GetDocumentByIdAsync ( Guid id )
        {
            return await _docRepo.FindAsync(id);
        }

        public int GetCount ( Expression<Func<Objection , bool>> filter = null ) => _objRepo.GetCount(filter);

        public async Task<IEnumerable<ObjectionDetails>> ListAsync ( Expression<Func<Objection , bool>> filters = null , int skip = 0 , int take = 10 )
        {
            return await _objRepo.GetAllAsync(
                filter: filters ,
                orderBy: source => source.OrderByDescending(b => b.Subject).OrderByDescending(u => u.CreateDate) ,
                select: entity => new ObjectionDetails
                {
                    Subject = entity.Subject ,
                    CreateDate = entity.CreateDate ,
                    Id = entity.Id ,
                    Result = entity.Result
                } ,
                skip: skip ,
                take: take);
        }

        public async Task<Objection> GetByIdAsync ( Guid id )
        {
            return await _objRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source
                    .Include(u => u.Documents));
        }

        public async Task UpdateAsync ( UpdateObjection command , IFormFileCollection files )
        {
            var entity = await _objRepo.FindAsync(command.Id);
            if ( entity == null )
                return;


            entity.WithSubject(command.Subject)
                .WithDescription(command.Description)
                .WithResult(command.Result);

            AddDocuments(entity.Id , files);
            _objRepo.Update(entity);
            await _objRepo.SaveAsync();
        }

        public async Task<IEnumerable<ObjectionDocument>> GetDocumentsAsync ( Guid id )
        {
            return await _docRepo.GetAllAsync(u => u.ObjectionId == id);
        }

        public async Task<bool> RemoveAsync ( Guid id )
        {
            var obj = await _objRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Documents));

            if ( obj == null )
                return false;

            obj.Documents.ToList().ForEach(doc => doc.RemoveFile());
            _objRepo.Remove(obj);
            await _objRepo.SaveAsync();
            await _caseStatusService.Fix(obj.CaseId);
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


        private void AddDocuments ( Guid defenceId , IFormFileCollection files )
        {
            if ( files == null )
                return;

            foreach ( var file in files )
            {
                var doc = new ObjectionDocument(defenceId , file.FileName , new Document(file.FileName , file.ReadBytes()));
                doc.CreateFile();
                _docRepo.Add(doc);
            }
        }
    }
}
