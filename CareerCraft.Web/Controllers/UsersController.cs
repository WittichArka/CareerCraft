using AutoMapper;
using CareerCraft.Core.Entities;
using CareerCraft.Core.Services;
using CareerCraft.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CareerCraft.Web.Controllers;

public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }

    // GET: Users
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();
        var viewModels = _mapper.Map<IEnumerable<UserViewModel>>(users);
        return View(viewModels);
    }

    // GET: Users/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Users/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserViewModel viewModel)
    {
        if (ModelState.IsValid)
        {
            var user = _mapper.Map<User>(viewModel);
            await _userService.CreateAsync(user);
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    // GET: Users/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var viewModel = _mapper.Map<UserViewModel>(user);
        return View(viewModel);
    }

    // POST: Users/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            var user = _mapper.Map<User>(viewModel);
            await _userService.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
        return View(viewModel);
    }

    // GET: Users/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        var viewModel = _mapper.Map<UserViewModel>(user);
        return View(viewModel);
    }

    // POST: Users/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _userService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
