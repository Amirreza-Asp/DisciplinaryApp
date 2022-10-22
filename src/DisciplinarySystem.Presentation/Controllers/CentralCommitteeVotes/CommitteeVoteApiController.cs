using DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes.Interfaces;
using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using DisciplinarySystem.Presentation.Controllers.CentralCommitteeVotes.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.CentralCommitteeVotes
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommitteeVoteApiController : ControllerBase
    {
        private readonly ICentralCommitteeVoteService _ccvService;
        private readonly IWebHostEnvironment _hostEnv;

        public CommitteeVoteApiController(ICentralCommitteeVoteService ccvService, IWebHostEnvironment hostEnv)
        {
            _ccvService = ccvService;
            _hostEnv = hostEnv;
        }


        [HttpGet("GetByCaseId/{caseId}")]
        public async Task<GetCommitteeVoteApi> GetByCaseId(long caseId)
        {
            var entity= await _ccvService.GetByCaseIdAsync(caseId);
            return GetCommitteeVoteApi.Create(entity);
        }

        [HttpGet("DownloadDocument/{id}")]
        public async Task<FileContentResult> DownloadDocument(Guid id)
        {
            var doc = await _ccvService.GetDocumentByIdAsync(id);
            if (doc == null)
                throw new Exception("document not found");

            string filePath = _hostEnv.WebRootPath + SD.CentralCommitteeVoteDocumentPath + doc.File.Name;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", doc.Name);
        }
    }
}
