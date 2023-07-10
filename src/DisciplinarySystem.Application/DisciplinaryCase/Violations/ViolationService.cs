using AutoMapper;
using DisciplinarySystem.Application.DisciplinaryCase.Cases.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Violations.Intefaces;
using DisciplinarySystem.Application.Violations.ViewModels.Violation;
using DisciplinarySystem.Domain.Violations;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Violations
{
    internal class ViolationService : IViolationService
    {
        private readonly IRepository<Violation> _violationRepo;
        private readonly IRepository<ViolationDocument> _documentRepo;
        private readonly ILogger<ViolationService> _logger;
        private readonly IMapper _mapper;
        private readonly ICaseReposiotry _caseRepo;
        private readonly ICaseStatusService _caseStatusService;

        public ViolationService(IRepository<Violation> violationRepo, IRepository<ViolationDocument> documentRepo, ILogger<ViolationService> logger, IMapper mapper, ICaseReposiotry caseRepo, ICaseStatusService caseStatusService)
        {
            _violationRepo = violationRepo;
            _documentRepo = documentRepo;
            _logger = logger;
            _mapper = mapper;
            _caseRepo = caseRepo;
            _caseStatusService = caseStatusService;
        }

        public async Task CreateAsync(CreateViolation createViolation, IFormFileCollection files)
        {
            var entity = new Violation(Guid.NewGuid(), createViolation.CategoryId,
                createViolation.Title, createViolation.Definition, createViolation.CaseId, null);


            var caseEntity = await _caseRepo.FindAsync(createViolation.CaseId);
            if ((int)caseEntity.Status < (int)CaseStatus.Complete)
            {
                caseEntity.WithStatus(CaseStatus.Complete);
                _caseRepo.Update(caseEntity);
            }


            _violationRepo.Add(entity);
            AddDocuments(files, entity.Id);
            await _violationRepo.SaveAsync();
        }

        public async Task<List<GetViolatonDetails>> GetAllAsync(Expression<Func<Violation, bool>> filter = null, int skip = 0, int take = 10)
        {
            var objs = await _violationRepo.GetAllAsync(
                    filter,
                    include: source => source
                        .Include(u => u.Category),
                    orderBy: source => source.OrderByDescending(u => u.Title).OrderByDescending(u => u.CreateDate),
                    select: violation => new GetViolatonDetails
                    {
                        Id = violation.Id,
                        Title = violation.Title,
                        Category = violation.Category.Title,
                        CreateDate = violation.CreateDate,
                        UpdateDate = violation.UpdateDate,
                        Vote = violation.Vote
                    },
                    take: take,
                    skip: skip);



            return objs.ToList();
        }

        public async Task<Violation> FindAsync(Expression<Func<Violation, bool>> filter, Func<IQueryable<Violation>, IIncludableQueryable<Violation, object>> include = null)
        {
            return await _violationRepo.FirstOrDefaultAsync(filter: filter, include: include);
        }

        public async Task<ViolationDocument> GetDocumentByIdAsync(Guid id) => await _documentRepo.FindAsync(id);

        public async Task<Violation> GetByIdAsync(Guid id)
        {
            var entity = await _violationRepo.FirstOrDefaultAsync(
                   filter: entity => entity.Id == id,
                   include: source => source
                   .Include(u => u.Documents)
                   .Include(u => u.Category));

            return entity;
        }

        public async Task<bool> RemoveAsync(Guid violationId)
        {
            var violation = _violationRepo.FirstOrDefault(
                filter: u => u.Id == violationId,
                include: source => source.Include(u => u.Documents));

            if (violation == null)
                return false;

            foreach (var item in violation.Documents)
            {
                item.RemoveFile();
                _documentRepo.Remove(item);
            }

            _violationRepo.Remove(violation);
            await _violationRepo.SaveAsync();

            await _caseStatusService.Fix(violation.CaseId);
            return true;
        }

        public async Task UpdateAsync(UpdateViolation updateViolation, IFormFileCollection files)
        {
            var violation = await _violationRepo.FindAsync(updateViolation.Id);
            if (violation == null)
                return;

            foreach (var file in files)
            {
                var doc = new ViolationDocument(Guid.NewGuid(), updateViolation.Id,
                    file.FileName, new Document(file.FileName, file.ReadBytes()));

                doc.CreateFile();
                _documentRepo.Add(doc);
            }

            violation.WithCategoryId(updateViolation.CategoryId)
                .WithDefinition(updateViolation.Definition)
                .WithTitle(updateViolation.Title)
                .WithUpdateDate(DateTime.Now);

            _violationRepo.Update(violation);
            await _violationRepo.SaveAsync();
        }

        public async Task<bool> RemoveDocument(Guid id)
        {
            var doc = await _documentRepo.FindAsync(id);
            if (doc == null)
                return false;

            doc.RemoveFile();
            _documentRepo.Remove(doc);
            await _documentRepo.SaveAsync();
            return true;
        }

        public int GetCount(Expression<Func<Violation, bool>> filter = null) => _violationRepo.GetCount(filter);

        public async Task<IEnumerable<SelectListItem>> GetSelectedListAsync(Expression<Func<Violation, bool>> filter = null)
        {
            return await _violationRepo.GetAllAsync<SelectListItem>(
                filter: filter,
                orderBy: source => source.OrderByDescending(u => u.CreateDate),
                select: u => new SelectListItem { Text = u.Title, Value = u.Id.ToString() });
        }


        private void AddDocuments(IFormFileCollection files, Guid violationId)
        {
            if (files == null)
                return;

            foreach (var file in files)
            {
                var entity = new ViolationDocument(Guid.NewGuid(), violationId, file.FileName,
                new Document(file.FileName, file.ReadBytes()));

                entity.CreateFile();
                _documentRepo.Add(entity);
            }

        }


    }
}
