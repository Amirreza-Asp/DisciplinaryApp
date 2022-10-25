using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.RelatedInfos.Interfaces;
using DisciplinarySystem.Application.RelatedInfos.ViewModels;
using DisciplinarySystem.Domain.RelatedInfos;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.RelatedInfos
{
    public class RelatedInfoService : IRelatedInfoService
    {
        private readonly IRepository<RelatedInfo> _infoRepo;
        private readonly IRepository<RelatedInfoDocument> _docRepo;
        private readonly ICaseReposiotry _caseRepo;

        public RelatedInfoService ( IRepository<RelatedInfo> infoRepo , IRepository<RelatedInfoDocument> docRepo , ICaseReposiotry caseRepo )
        {
            _infoRepo = infoRepo;
            _docRepo = docRepo;
            _caseRepo = caseRepo;
        }

        public async Task<RelatedInfoDocument> GetDocumentByIdAsync ( Guid id ) => await _docRepo.FindAsync(id);

        public int GetCount ( Expression<Func<RelatedInfo , bool>> filter = null ) => _infoRepo.GetCount(filter);

        public async Task<IEnumerable<RelatedInfoDetails>> ListAsync ( Expression<Func<RelatedInfo , bool>> filters = null , int skip = 0 , int take = 10 )
        {
            return await _infoRepo.GetAllAsync(
                filter: filters ,
                orderBy: source => source.OrderByDescending(b => b.Subject).OrderByDescending(u => u.CreateDate) ,
                select: entity => new RelatedInfoDetails
                {
                    Subject = entity.Subject ,
                    CreateDate = entity.CreateDate ,
                    Id = entity.Id
                } ,
                skip: skip ,
                take: take);
        }

        public async Task<RelatedInfo> GetByIdAsync ( Guid id )
        {
            return await _infoRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source
                    .Include(u => u.Documents));
        }

        public async Task<IEnumerable<RelatedInfoDocument>> GetDocumentsAsync ( Guid id ) => await _docRepo.GetAllAsync(u => u.RelatedInfoId == id);


        public async Task CreateAsync ( CreateRelatedInfo command , IFormFileCollection files )
        {
            var entity = new RelatedInfo(command.Subject , command.Description , command.CaseId);
            AddDocuments(entity.Id , files);

            _infoRepo.Add(entity);
            await _infoRepo.SaveAsync();
        }


        public async Task UpdateAsync ( UpdateRelatedInfo command , IFormFileCollection files )
        {
            var entity = await _infoRepo.FindAsync(command.Id);
            if ( entity == null )
                return;

            entity.WithSubject(command.Subject)
                .WithDescription(command.Description);

            AddDocuments(entity.Id , files);
            _infoRepo.Update(entity);
            await _infoRepo.SaveAsync();
        }

        public async Task<bool> RemoveAsync ( Guid id )
        {
            var info = await _infoRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Documents));

            if ( info == null )
                return false;

            info.Documents.ToList().ForEach(doc => doc.RemoveFile());
            _infoRepo.Remove(info);
            await _infoRepo.SaveAsync();
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
                var doc = new RelatedInfoDocument(defenceId , file.FileName , new Document(file.FileName , file.ReadBytes()));
                doc.CreateFile();
                _docRepo.Add(doc);
            }
        }

    }
}
