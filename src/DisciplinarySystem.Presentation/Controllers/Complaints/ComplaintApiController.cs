using DisciplinarySystem.Application.Complaints.Interfaces;
using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Presentation.Controllers.Complaints.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Complaints
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintApiController : ControllerBase
    {
        private readonly IComplaintService _compService;
        private readonly IWebHostEnvironment _hostEnv;

        public ComplaintApiController(IComplaintService compService, IWebHostEnvironment hostEnv)
        {
            _compService = compService;
            _hostEnv = hostEnv;
        }

        [HttpGet("GetByCaseId/{caseId}")]
        public async Task<GetComplaintApi> GetByCaseId(long caseId)
        {
            var complaint = await _compService.GetByCaseIdAsync(caseId);
            return GetComplaintApi.Create(complaint);
        }

        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _compService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.ComplaintDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }
        
    }
}
