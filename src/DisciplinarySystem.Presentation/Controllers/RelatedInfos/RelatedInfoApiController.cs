using DisciplinarySystem.Application.RelatedInfos.Interfaces;
using DisciplinarySystem.Application.RelatedInfos.ViewModels;
using DisciplinarySystem.Presentation.Controllers.RelatedInfos.Dtos;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.RelatedInfos
{
    [Route("api/[controller]")]
    [ApiController]
    public class RelatedInfoApiController : ControllerBase
    {
        private readonly IRelatedInfoService _relService;
        private readonly IWebHostEnvironment _hostEnv;

        public RelatedInfoApiController(IRelatedInfoService relService, IWebHostEnvironment hostEnv)
        {
            _relService = relService;
            _hostEnv = hostEnv;
        }

        [HttpGet("GetAllByCaseId/{caseId}")]
        public async Task<IEnumerable<RelatedInfoDetails>> GetAllByCaseId(long caseId)
        {
            return await _relService.ListAsync(u => u.CaseId == caseId);
        }

        [HttpGet("GetById/{id}")]
        public async Task<GetRelatedInfoDto> GetById(Guid id)
        {
            var entity = await _relService.GetByIdAsync(id);
            if (entity == null)
                throw new Exception("RelatedInfo is not found");

            return GetRelatedInfoDto.Create(entity);
        }

        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _relService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.RelatedInfoDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }

    }
}
