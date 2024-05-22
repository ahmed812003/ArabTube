﻿using ArabTube.Entities.DtoModels.UserConnectionsDto;
using ArabTube.Entities.Models;
using ArabTube.Services.ControllersServices.SubscriptionServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ArabTube.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionsController(UserManager<AppUser> userManager, ISubscriptionService subscriptionService)
        {
            _userManager = userManager;
            _subscriptionService = subscriptionService;
        }

        [HttpGet()]
        public async Task<IActionResult> Following()
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _subscriptionService.GetFollowingAsync(user.Id);

                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

                    return Ok(result.Followings);
                }
            }
            return Unauthorized();
        }

        [HttpPost("Subscribe")]
        public async Task<IActionResult> Subscribe(string ownerId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _subscriptionService.SubscribeAsync(ownerId, user.Id);

                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

                    return Ok(result.Message);
                }
            }
            return Unauthorized();
        }

        [HttpDelete("UnSubscribe")]
        public async Task<IActionResult> UnScbscribe(string ownerId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    var result = await _subscriptionService.UnScbscribeAsync(ownerId, user.Id);

                    if (!result.IsSuccesed)
                    {
                        return BadRequest(result.Message);
                    }

                    return Ok("UnSubscribe Successfully");
                }
            }
            return Unauthorized();
        }

    }
}
