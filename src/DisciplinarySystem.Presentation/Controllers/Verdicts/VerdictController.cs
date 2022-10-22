using AutoMapper;
using DisciplinarySystem.Application.Verdicts.Interfaces;
using DisciplinarySystem.Application.Verdicts.ViewModels;
using DisciplinarySystem.Presentation.Controllers.Verdicts.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Verdicts
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class VerdictController : Controller
    {
        private readonly IVerdictService _service;
        private readonly IMapper _mapper;

        private static VerdictFilter _filters = new VerdictFilter();

        public VerdictController ( IVerdictService service , IMapper mapper )
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index ( VerdictFilter filters )
        {
            _filters = filters;
            var vm = new GetAllVerdicts
            {
                Verdicts = await _service.GetListAsync(skip: filters.Skip , take: filters.Take) ,
                TotalCount = _service.GetCount() ,
                Filters = filters
            };
            return View(vm);
        }

        public async Task<IActionResult> Details ( long id )
        {
            var entity = await _service.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Success] = "حکم مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            return View(entity);
        }

        public IActionResult Create () => View();
        [HttpPost]
        public async Task<IActionResult> Create ( CreateVerdict command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            if ( await _service.GetByTitleAsync(command.Title) != null )
            {
                TempData[SD.Warning] = "عنوان وارد شده تکراری است";
                return View(command);
            }

            await _service.CreateAsync(command);
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<ViewResult> Update ( long id )
        {
            var entity = await _service.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "حکم مورد نظر وجود ندارد";
                return View(nameof(Index) , _filters);
            }

            var command = _mapper.Map<UpdateVerdict>(entity);
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateVerdict command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            if ( await _service.GetByTitleAsync(command.Title) != null )
            {
                TempData[SD.Warning] = "عنوان وارد شده تکراری است";
                return View(command);
            }

            await _service.UpdateAsync(command);
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<JsonResult> Remove ( long id )
        {
            if ( await _service.RemoveAsync(id) )
                return Json(new { Success = true , Message = "عملیات با موفقیت انجام شد" });
            return Json(new { Success = false , Message = "عملیات با شکست مواجه شد" });
        }
    }
}
