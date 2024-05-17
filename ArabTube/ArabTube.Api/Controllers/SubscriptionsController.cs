using ArabTube.Entities.DtoModels.UserConnectionsDto;
using ArabTube.Entities.DtoModels.WatchedVideoDto;
using ArabTube.Entities.Models;
using ArabTube.Services.CloudServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.VideoServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public SubscriptionsController(IUnitOfWork unitOfWork,UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
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
                    var appUserConnections = await _unitOfWork.AppUserConnection.GetFollowingAsync(user.Id);
                    if (appUserConnections == null || !appUserConnections.Any())
                        return NotFound("The Current Login User Has No Following");
                  
                    var following = appUserConnections.Select(auc => new FollowingDto
                    {
                        Username = auc.Following.UserName
                    });
                    return Ok(following);
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
                    var owner = await _userManager.FindByIdAsync(ownerId);
                    if (owner == null)
                        return NotFound($"No User With Id = {ownerId}");
                    var result = await _unitOfWork.AppUserConnection.SubscribeAsync(ownerId, user.Id);
                    if (!result)
                    {
                        return Ok("You Already Subscriber To This User");
                    }

                    await _unitOfWork.Complete();
                    return Ok("Subscribe Succesfully");
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
                    var owner = await _userManager.FindByIdAsync(ownerId);
                    if (owner == null)
                    {
                        return NotFound($"No User With Id = {ownerId}");
                    }
                        
                    var result = await _unitOfWork.AppUserConnection.UnSubscribeAsync(ownerId, user.Id);
                    if (!result)
                    {
                        return NotFound($"You Already Not A Follower To User With Id = {ownerId}");
                    }
                        
                    await _unitOfWork.Complete();
                    return Ok("UnSubscribe Successfully");
                }
            }
            return Unauthorized();
        }

    }
}
