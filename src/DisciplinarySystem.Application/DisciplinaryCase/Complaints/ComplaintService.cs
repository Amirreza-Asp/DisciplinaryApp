using DisciplinarySystem.Application.Cases.Helpers;
using DisciplinarySystem.Application.Complaints.Helpers;
using DisciplinarySystem.Application.Complaints.Interfaces;
using DisciplinarySystem.Application.Complaints.ViewModels;
using DisciplinarySystem.Application.Complaints.ViewModels.Create;
using DisciplinarySystem.Application.Complaints.ViewModels.Update;
using DisciplinarySystem.Application.DisciplinaryCase.Complaints.ViewModels.Update;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Domain.Complaints.Enums;
using DisciplinarySystem.Domain.Complaints.Interfaces;
using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Complaints
{
    public class ComplaintService : IComplaintService
    {
        private readonly IUserApi _userApi;
        private readonly IRepository<Plaintiff> _plaintiffRepo;
        private readonly IRepository<Complaining> _complainingRepo;
        private readonly IRepository<ComplaintDocument> _documentRepo;
        private readonly IComplaintRepository _complaintRepo;
        private readonly IRepository<Case> _caseRepo;

        public ComplaintService ( IUserApi userApi , IRepository<Plaintiff> plaintiffRepo , IRepository<Complaining> complainingRepo ,
            IRepository<ComplaintDocument> documentRepo , IRepository<Case> caseRepo , IComplaintRepository complaintRepo )
        {
            _userApi = userApi;
            _plaintiffRepo = plaintiffRepo;
            _complainingRepo = complainingRepo;
            _documentRepo = documentRepo;
            _complaintRepo = complaintRepo;
            _caseRepo = caseRepo;
        }


        public async Task<Complaint> GetByCaseIdAsync ( long caseId )
        {
            return await _complaintRepo.FirstOrDefaultAsync(
                filter: u => u.Case.Id == caseId ,
                include: source => source.Include(u => u.Plaintiff)
                            .Include(u => u.Complaining)
                            .Include(u => u.Documents));
        }
        public async Task<List<GetComplaint>> GetAllAsync ( Expression<Func<Complaint , bool>> filter = null , int take = 10 , int skip = 0 )
        {
            var objects = await
                _complaintRepo.GetAllAsync<GetComplaint>(
                    filter: filter ,
                    include: source => source
                        .Include(u => u.Complaining)
                        .Include(u => u.Plaintiff)
                        .Include(u => u.Case) ,
                    orderBy: entity => entity.OrderByDescending(u => u.Title)
                                    .OrderByDescending(u => u.CreateDate) ,
                    select: entity => new GetComplaint
                    {
                        Id = entity.Id ,
                        ComplainingName = entity.Complaining.FullName ,
                        PlaintiffName = entity.Plaintiff.FullName ,
                        CreateTime = entity.CreateDate ,
                        Status = entity.Case == null ? "_" : entity.Case.Status.ToPersian() ,
                        Subject = entity.Title ,
                        Result = entity.Result.ToPersian()
                    } ,
                    take: take ,
                    skip: skip
                );

            return objects.ToList();
        }

        public int GetCount ( Expression<Func<Complaint , bool>> filter = null )
        {
            return _complaintRepo.GetCount(filter);
        }

        public async Task<Complaint> GetByIdAsync ( long id )
        {
            return await _complaintRepo.FirstOrDefaultAsync(
                                filter: entity => entity.Id == id ,
                                include: source => source
                                    .Include(u => u.Complaining)
                                    .Include(u => u.Plaintiff)
                                    .Include(u => u.Documents));
        }

        public async Task<UserInfo> GetUserAsync ( string nationalCode )
        {
            return await _userApi.GetUserAsync(nationalCode);
        }

        public async Task<ComplaintDocument> GetDocumentByIdAsync ( Guid id )
        {
            return await _documentRepo.FindAsync(id);
        }


        public async Task CreateAsync ( CreateComplaint createComplaint , IFormFileCollection files )
        {
            var plaintiffId = AddPlaintiff(createComplaint.Plaintiff);
            var complainingId = AddComplainingAsync(createComplaint.Complaining);

            var complaint = new Complaint(createComplaint.Subject , plaintiffId , complainingId , createComplaint.Description);

            var complaintId = await _complaintRepo.CreateAsync(complaint);
            AddDocuments(files , complaintId);
            await _documentRepo.SaveAsync();
        }

        public async Task<bool> RemoveAsync ( long id )
        {
            var complaint = await _complaintRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source
                    .Include(u => u.Plaintiff)
                    .Include(u => u.Complaining)
                    .Include(u => u.Documents));

            if ( complaint == null )
                return false;

            _complainingRepo.Remove(complaint.Complaining);
            _plaintiffRepo.Remove(complaint.Plaintiff);
            complaint.Documents.ForEach(doc =>
            {
                doc.RemoveFile();
                _documentRepo.Remove(doc);
            });
            _complaintRepo.Remove(complaint);
            await _complaintRepo.SaveAsync();
            return true;
        }

        public async Task<UpdateResult> UpdateAsync ( UpdateComplaint updateComplaint , IFormFileCollection files )
        {
            var complaint = await GetByIdAsync(updateComplaint.Id);
            var upRes = new UpdateResult();

            if ( complaint == null )
                return upRes;

            if ( complaint.Result != ComplaintResult.Filing && ( ComplaintResult ) updateComplaint.Result == ComplaintResult.Filing )
            {
                var caseEntity = new Case(complaint.Id);
                _caseRepo.Add(caseEntity);
                complaint.WithResult(ComplaintResult.SeeCase);
                upRes.CaseList();
            }
            else
                complaint.WithResult(( ComplaintResult ) updateComplaint.Result);

            complaint
                .WithTitle(updateComplaint.Subject)
                .WithDescription(updateComplaint.Description);

            UpdateComplaining(updateComplaint.Complaining , complaint.Complaining);
            UpdatePlaintiff(updateComplaint.Plaintiff , complaint.Plaintiff);
            AddDocuments(files , updateComplaint.Id);



            _complaintRepo.Update(complaint);
            await _complaintRepo.SaveAsync();

            return upRes;
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

        public async Task CreateCaseAsync ( long id )
        {
            Complaint complaint = await _complaintRepo.FirstOrDefaultAsync(u => u.Id == id);
            if ( complaint == null )
                return;

            var caseEntity = new Case(id);
            _caseRepo.Add(caseEntity);

            complaint.WithResult(ComplaintResult.Archive);
            _complaintRepo.Update(complaint);

            await _caseRepo.SaveAsync();
        }

        #region Utilities
        private Guid AddPlaintiff ( CreatePlaintiff createPlaintiff )
        {
            var plaintiff = new Plaintiff(Guid.NewGuid() , createPlaintiff.FullName , createPlaintiff.PhoneNumber ,
                createPlaintiff.Address , createPlaintiff.NationalCode);

            _plaintiffRepo.Add(plaintiff);
            return plaintiff.Id;
        }

        private Guid AddComplainingAsync ( CreateComplaining complaining )
        {
            var entity = new Complaining(Guid.NewGuid() , complaining.FullName , complaining.StudentNumber , complaining.NationalCode ,
                complaining.Grade , complaining.EducationGroup , complaining.College , complaining.Father);

            _complainingRepo.Add(entity);
            return entity.Id;
        }

        private void AddDocuments ( IFormFileCollection files , long complaintId )
        {
            if ( files == null )
                return;

            foreach ( var file in files )
            {
                var doc = new ComplaintDocument(Guid.NewGuid() , complaintId , file.FileName ,
                    new Document(file.FileName , file.ReadBytes()));

                _documentRepo.Add(doc);
                doc.CreateFile();
            }
        }

        private void UpdatePlaintiff ( UpdatePlaintiff updatePlaintiff , Plaintiff entity )
        {
            entity.WithFullName(updatePlaintiff.FullName).WithPhoneNumber(updatePlaintiff.PhoneNumber)
                .WithAddress(updatePlaintiff.Address).WithNationalCode(updatePlaintiff.NationalCode);
            _plaintiffRepo.Update(entity);
        }

        private void UpdateComplaining ( UpdateComplaining updateComplaining , Complaining entity )
        {
            entity.WithFullName(updateComplaining.FullName)
                .WithNationalCode(updateComplaining.NationalCode)
                .WithStudentNumber(updateComplaining.StudentNumber)
                .WithCollege(updateComplaining.College)
                .WithFather(updateComplaining.Father)
                .WithGrade(updateComplaining.Grade);

            _complainingRepo.Update(entity);
        }
        #endregion
    }
}
