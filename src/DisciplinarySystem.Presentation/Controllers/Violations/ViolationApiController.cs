using DisciplinarySystem.Application.Violations.Intefaces;
using DisciplinarySystem.Application.Violations.ViewModels.Violation;
using DisciplinarySystem.Presentation.Controllers.Violations.Dtos;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DisciplinarySystem.Presentation.Controllers.Violations
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViolationApiController : ControllerBase
    {
        private readonly IViolationService _violationService;
        private readonly IWebHostEnvironment _hostEnv;

        public ViolationApiController(IViolationService violationService, IWebHostEnvironment hostEnv)
        {
            _violationService = violationService;
            _hostEnv = hostEnv;
        }


        [HttpGet("GetAllByCaseId/{caseId}")]
        public async Task<IEnumerable<GetViolatonDetails>> GetAll(long caseId)
        {
            return await _violationService.GetAllAsync(
                filter: u => u.CaseId == caseId);
        }

        [HttpGet("GetById/{id}")]
        public async Task<GetViolationDto> GetById(Guid id)
        {
            var entity = await _violationService.FindAsync(
                filter: u => u.Id == id,
                include: source => source
                    .Include(u=>u.Category)
                    .Include(u => u.CentralCommitteeVote)
                        .ThenInclude(u => u.Verdict)
                    .Include(u => u.PrimaryVote)
                        .ThenInclude(u => u.Verdict)
                    .Include(u => u.FinalVote)
                        .ThenInclude(u => u.Verdict)
                    .Include(u => u.Documents));

            if (entity == null)
                throw new Exception("Violation not found");

            return GetViolationDto.Create(entity);
        }



        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _violationService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.ViolationDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }
    }
}
