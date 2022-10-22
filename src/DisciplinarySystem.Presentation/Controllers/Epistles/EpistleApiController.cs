using DisciplinarySystem.Application.Epistles.Interfaces;
using DisciplinarySystem.Application.Epistles.ViewModels;
using DisciplinarySystem.Domain.Epistles;
using DisciplinarySystem.Presentation.Controllers.Epistles.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Epistles
{
    [Route("api/[controller]")]
    [ApiController]
    public class EpistleApiController : ControllerBase
    {
        private readonly IEpistleService _epistleService;
        private readonly IWebHostEnvironment _hostEnv;

        public EpistleApiController(IEpistleService epistleService, IWebHostEnvironment hostEnv)
        {
            _epistleService = epistleService;
            _hostEnv = hostEnv;
        }

        [HttpGet("GetAllByCaseId/{caseId}/{skip}/{take}")]
        public async Task<IEnumerable<GetEpistle>> GetAllByCaseId(long caseId , int skip , int take)
        {
            return await _epistleService.GetAllAsync(u => u.CaseId == caseId, skip: skip, take: take);
        }

        [HttpGet("GetById/{id}")]
        public async Task<GetEpistleApi> GetById(long id)
        {
            var entity = await _epistleService.GetByIdAsync(id);
            return GetEpistleApi.Create(entity);
        }

        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _epistleService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.EpistleDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }
    }
}
