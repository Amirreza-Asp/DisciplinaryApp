using DisciplinarySystem.Application.Invitations.Interfaces;
using DisciplinarySystem.Application.Invitations.ViewModels;
using DisciplinarySystem.Presentation.Controllers.Invitations.Dtos;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Invitations
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvitationApiController : ControllerBase
    {
        private readonly IInvitationService _invService;
        private readonly IWebHostEnvironment _hostEnv;

        public InvitationApiController(IInvitationService invService, IWebHostEnvironment hostEnv)
        {
            _invService = invService;
            _hostEnv = hostEnv;
        }

        [HttpGet("GetAllByCaseId/{caseId}/{skip}/{take}")]
        public async Task<IEnumerable<InvitationDetails>> GetAllByCaseId(long caseId, int? skip, int? take)
        {
            return await _invService.ListAsync(u => u.CaseId == caseId, skip: skip.HasValue ? skip.Value : 0, take: take.HasValue ? take.Value : 0);
        }

        [HttpGet("GetById/{id}")]
        public async Task<GetInvitationDto> GetById(Guid id)
        {
            var invitation = await _invService.GetByIdAsync(id);
            return GetInvitationDto.Create(invitation);
        }

        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _invService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.InvitationDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }

    }
}
