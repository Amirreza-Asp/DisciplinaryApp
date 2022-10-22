using Ardalis.GuardClauses;
using DisciplinarySystem.Application.Defences.Interfaces;
using DisciplinarySystem.Application.Defences.ViewModels;
using DisciplinarySystem.Domain.Defences;
using DisciplinarySystem.Presentation.Controllers.Defences.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Defences
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefenceApiController : ControllerBase
    {
        private readonly IDefenceService _defService;
        private readonly IWebHostEnvironment _hostEnv;

        public DefenceApiController(IDefenceService defService, IWebHostEnvironment hostEnv)
        {
            _defService = defService;
            _hostEnv = hostEnv;
        }

        [HttpGet("GetAll/{caseId}/{skip}/{take}")]
        public async Task<IEnumerable<DefenceDetails>> GetAll(long caseId, int skip, int take)
        {
            return await _defService.ListAsync(u => u.CaseId == caseId, skip: skip, take: take);
        }

        [HttpGet("GetById/{id}")]
        public async Task<GetDefenceApi> GetById(Guid id)
        {
            var entity = await _defService.GetByIdAsync(id); 
            return GetDefenceApi.Create(entity);
        }


        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _defService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.DefenceDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }

    }
}
