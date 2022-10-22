using DisciplinarySystem.Application.Objections.Interfaces;
using DisciplinarySystem.Application.Objections.ViewModels;
using DisciplinarySystem.Presentation.Controllers.Objections.Dtos;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Objections
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectionApiController : ControllerBase
    {
        private readonly IObjectionService _objectionService;
        private readonly IWebHostEnvironment _hostEnv;

        public ObjectionApiController(IObjectionService objectionService, IWebHostEnvironment hostEnv)
        {
            _objectionService = objectionService;
            _hostEnv = hostEnv;
        }

        [HttpGet("GetAllByCaseId/{caseId}/{skip}/{take}")]
        public async Task<IEnumerable<ObjectionDetails>> GetAllByCaseId(long caseId , int skip , int take)
        {
            return await _objectionService.ListAsync(u => u.CaseId == caseId, skip: skip, take: take);
        }

        [HttpGet("GetById/{id}")]
        public async Task<GetObjectionDto> GetById(Guid id)
        {
            var entity = await _objectionService.GetByIdAsync(id);
            return GetObjectionDto.Create(entity);
        }

        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _objectionService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.ObjectionDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }
    }
}
