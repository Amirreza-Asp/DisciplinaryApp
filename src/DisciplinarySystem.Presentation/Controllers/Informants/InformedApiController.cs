using DisciplinarySystem.Application.Informants.Interfaces;
using DisciplinarySystem.Application.Informants.ViewModels;
using DisciplinarySystem.Domain.Informants;
using DisciplinarySystem.Presentation.Controllers.Informants.Dtos;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Informants
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformedApiController : ControllerBase
    {
        private readonly IInformedService _infService;
        private readonly IWebHostEnvironment _hostEnv;

        public InformedApiController(IInformedService infService, IWebHostEnvironment hostEnv)
        {
            _infService = infService;
            _hostEnv = hostEnv;
        }

        [HttpGet("GetAllByCaseId/{caseId}/{skip}/{take}")]
        public async Task<IEnumerable<InformedDetails>> GetAllByCaseId(long caseId , int skip , int take)
        {
            return await _infService.GetListAsync(u => u.CaseId == caseId, skip: skip, take: take);
        }


        [HttpGet("GetById/{id}")]
        public async Task<GetInformedDto> GetById(Guid id)
        {
            var entity = await _infService.GetByIdAsync(id);
            return GetInformedDto.Create(entity);
        }


        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _infService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.InformedDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }
    }
}
