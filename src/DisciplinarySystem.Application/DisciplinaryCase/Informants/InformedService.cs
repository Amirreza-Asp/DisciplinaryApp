using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Informants.Interfaces;
using DisciplinarySystem.Application.Informants.ViewModels;
using DisciplinarySystem.Domain.Informants;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Informants
{
    public class InformedService : IInformedService
    {
        private readonly IRepository<Informed> _infRepo;
        private readonly IRepository<InformedDocument> _docRepo;
        private readonly ICaseReposiotry _caseRepo;
        private readonly ICaseStatusService _caseStatusService;

        public InformedService ( IRepository<Informed> infRepo , IRepository<InformedDocument> docRepo , ICaseReposiotry caseRepo , ICaseStatusService caseStatusService )
        {
            _infRepo = infRepo;
            _docRepo = docRepo;
            _caseRepo = caseRepo;
            _caseStatusService = caseStatusService;
        }

        public async Task CreateAsync ( CreateInformed command , IFormFileCollection files )
        {
            var entity = new Informed(command.FullName , command.PhoneNumber ,
                command.Subject , command.Statements , command.CaseId , command.NationalCode , command.Father);
            _infRepo.Add(entity);


            var caseEntity = await _caseRepo.FindAsync(command.CaseId);
            if ( ( int ) caseEntity.Status < ( int ) CaseStatus.Complete )
            {
                caseEntity.WithStatus(CaseStatus.Complete);
                _caseRepo.Update(caseEntity);
            }

            AddDocuments(entity.Id , files);
            await _infRepo.SaveAsync();
        }

        public Task<Informed> GetByIdAsync ( Guid id )
        {
            return _infRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Documents));
        }

        public async Task<InformedDocument> GetDocumentByIdAsync ( Guid id )
        {
            return await _docRepo.FindAsync(id);
        }

        public int GetCount ( Expression<Func<Informed , bool>> filter = null )
        {
            return _infRepo.GetCount(filter);
        }

        public async Task<List<InformedDocumentDto>> GetDocuments ( Guid id )
        {
            var docs = await _docRepo.GetAllAsync(
                filter: entity => entity.InformedId == id ,
                select: entity => InformedDocumentDto.Create(entity));

            return docs.ToList();
        }

        public async Task<IEnumerable<InformedDetails>> GetListAsync ( Expression<Func<Informed , bool>> filter = null , int skip = 0 , int take = 10 )
        {
            return await _infRepo.GetAllAsync(
                filter ,
                orderBy: source => source.OrderByDescending(b => b.FullName)
                .OrderByDescending(b => b.Subject)
                .OrderByDescending(u => u.CreateDate) ,
                select: entity => new InformedDetails
                {
                    FullName = entity.FullName ,
                    CreateDate = entity.CreateDate ,
                    Subject = entity.Subject ,
                    Id = entity.Id
                } ,
                take: take ,
                skip: skip);
        }

        public async Task<bool> RemoveAsync ( Guid id )
        {
            var entity = await GetByIdAsync(id);
            if ( entity == null )
                return false;

            entity.Documents.ToList().ForEach(doc => doc.RemoveFile());
            _infRepo.Remove(entity);
            await _infRepo.SaveAsync();
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

        public async Task UpdateAsync ( UpdateInformed command , IFormFileCollection files )
        {
            var entity = await _infRepo.FindAsync(command.Id);

            if ( entity == null )
                return;

            entity.WithSubject(command.Subject)
                .WithFullName(command.FullName)
                .WithPhoneNumber(command.PhoneNumber)
                .WithStatements(command.Statements)
                .WithFather(command.Father)
                .WithNationalCode(command.NationalCode);

            AddDocuments(entity.Id , files);

            _infRepo.Update(entity);
            await _infRepo.SaveAsync();
        }

        private void AddDocuments ( Guid id , IFormFileCollection files )
        {
            if ( files == null || !files.Any() )
                return;

            foreach ( var file in files )
            {
                var doc = new InformedDocument(Guid.NewGuid() , id , file.FileName ,
                    new Document(file.FileName , file.ReadBytes()));

                doc.CreateFile();
                _docRepo.Add(doc);
            }
        }
    }
}
