using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RfidAPI.Models;
using RfidAPI.Service;

namespace RfidAPI.Controllers
{
    [Route("team")]
    [Authorize]
    public class TeamController:ControllerBase
    {
        private readonly TeamService _teamService;

        public TeamController(TeamService teamService)
        {
            _teamService = teamService;
        }
        [HttpPost("add")]
        public ActionResult<string> add(string teamName, string teamInfo, string teamQQ)
        {
            var res = _teamService.CreateTeam(teamName, teamInfo, teamQQ, TeamStatus.not);
            if (res)
            {
                return "[组织]添加成功";
            }
            else
            {
                return "[组织]添加失败";
            }
        }
        
        
        
    }
}