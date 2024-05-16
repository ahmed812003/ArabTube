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
                    if(appUserConnections != null)
                    {
                        var following = appUserConnections.Select(auc => new FollowingDto
                        {
                            Username = auc.Following.UserName
                        });
                        return Ok(following);
                    }
                }
            }
            return Ok();
        }

        [HttpPost("Subscribe/{ownerId}")]
        public async Task<IActionResult> Subscribe(string ownerId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    await _unitOfWork.AppUserConnection.SubscribeAsync(ownerId, user.Id);
                    await _unitOfWork.Complete();
                }
            }
            return Ok("Subscribe Succesfully");
        }

        [HttpDelete("UnSubscribe/{ownerId}")]
        public async Task<IActionResult> UnScbscribe(string ownerId)
        {
            var userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    await _unitOfWork.AppUserConnection.UnSubscribeAsync(ownerId, user.Id);
                    await _unitOfWork.Complete();
                }
            }
            return Ok("UnSubscribe Successfully");
        }

    }
}
