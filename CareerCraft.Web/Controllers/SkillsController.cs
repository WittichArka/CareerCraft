using AutoMapper;
using CareerCraft.Core.Entities;
using CareerCraft.Core.Services;
using CareerCraft.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareerCraft.Web.Controllers;

public class SkillsController : Controller
{
    private readonly ISkillService _skillService;
    private readonly IMapper _mapper;

    public SkillsController(ISkillService skillService, IMapper mapper)
    {
        _skillService = skillService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var skills = await _skillService.GetAllAsync();
        var skillViewModels = _mapper.Map<IEnumerable<SkillViewModel>>(skills);
        return View(skillViewModels);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SkillViewModel skillViewModel)
    {
        if (ModelState.IsValid)
        {
            var skill = _mapper.Map<Skill>(skillViewModel);
            await _skillService.CreateAsync(skill);
            return RedirectToAction(nameof(Index));
        }
        return View(skillViewModel);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var skill = await _skillService.GetByIdAsync(id);
        if (skill == null) return NotFound();
        var skillViewModel = _mapper.Map<SkillViewModel>(skill);
        return View(skillViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SkillViewModel skillViewModel)
    {
        if (id != skillViewModel.Id) return BadRequest();

        if (ModelState.IsValid)
        {
            var skill = _mapper.Map<Skill>(skillViewModel);
            await _skillService.UpdateAsync(skill);
            return RedirectToAction(nameof(Index));
        }
        return View(skillViewModel);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var skill = await _skillService.GetByIdAsync(id);
        if (skill == null) return NotFound();
        var skillViewModel = _mapper.Map<SkillViewModel>(skill);
        return View(skillViewModel);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _skillService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
