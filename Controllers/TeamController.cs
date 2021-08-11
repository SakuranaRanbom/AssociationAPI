using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TeamAPI.Models;
using TeamAPI.Service;

namespace TeamAPI.Controllers
{
    
    
    [Route("team")]
    
    public class TeamController:ControllerBase
    {
        
        private readonly TeamService _teamService;
        private readonly TeamUserService _teamUserService;
        private readonly UserManager<IUser> _userManager;
        public TeamController(TeamService teamService,TeamUserService teamUserService)
        {
            _teamService = teamService;
            _teamUserService = teamUserService;
        }
        [HttpPost("add")]
        [Authorize]
        public ActionResult<string> add([FromBody]SubmitTeam submitTeam)
        {
            if (_teamService.IsTeam(submitTeam.teamName))
            {
                return "[组织]已经存在。请勿重复创建";
            }
            else
            {  
                bool res2;
                var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
                var res = _teamService.CreateTeam(submitTeam.teamName, submitTeam.teamInfo, submitTeam.teamQQ, TeamStatus.not);
                var teamID = _teamService.GetTeamID(submitTeam.teamName);
                if(teamID != null)
                {
                    
                    res2 = _teamUserService.addUser(userID, teamID.GetValueOrDefault());
                }
                else
                {
                    res2 = false;
                }

                if (res && res2)
                {
                    return "[组织]添加成功";
                }
                else
                {
                    return "[组织]添加失败";
                }
            }
           
        }
        [HttpGet("all")]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<IEnumerable<Team>>> getAll()
        {
            return await _teamService.getTeams();
        }

        [HttpGet("getOK")]
        public async Task<ActionResult<IEnumerable<Team>>> getOK()
        {
            return await _teamService.getOKTeams();
        }
        
        [HttpGet("name")]
        public async Task<ActionResult<IEnumerable<Team>>> getByName([FromQuery] string name)
        {
            return await _teamService.getTeamByName(name);
        }
        
        
        [HttpGet("info")]
        public async Task<ActionResult<IEnumerable<Team>>> getByInfo([FromQuery] string info)
        {
            return await _teamService.getTeamByInfo(info);
        }
        [HttpGet("remove")]
        [Authorize]
        public ActionResult<string> Remove()
        {
            var userID =  HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            var teamID = _teamUserService.GetTeamID(userID);
            if (teamID != null)
            {
                var res = _teamService.DeleteTeam(teamID.GetValueOrDefault()) && _teamUserService.deleteUser(userID, teamID.GetValueOrDefault());
                if (res)
                {
                    return "删除成功";
                }
            }
            else
            {
                return "队伍ID获取为空,失败";
            }
            return "队伍ID获取为空,失败";
        }
        [HttpGet("hasteam")]
        [Authorize]
        public ActionResult<bool> HasTeam()
        {
            /*var userID =  HttpContext.User.Claims.AsEnumerable();
            Console.WriteLine(userID.ToString());
            foreach (var VARIABLE in userID)
            {
                Console.WriteLine(VARIABLE.Type + VARIABLE.Value);
            }*/
            
            var userID = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "UserID").Value;
            var res = _teamUserService.HasTeam(userID);
            if (res)
            {
                return true;
            }

            return false;
        }
        [HttpGet("switch")]
        [Authorize(Policy = "Admin")]
        public ActionResult<bool> SwitchStatus([FromQuery]string teamName)
        {
           // Console.WriteLine(teamName);
           var res = _teamService.SwitchTeamStatus(teamName);
           if (res)
           {
               return true;
           }
           else
           {
               return false;
           }
        }
        
    }
}